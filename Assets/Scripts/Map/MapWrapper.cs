using System;
using System.Linq;
using DataClass;
using Enums;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;


namespace Map
{
    [Serializable]
    public class MapWrapper
    {
        /**
         * 各行のマスリスト
         */
        private MapTile[][] _mapTiles;

        private MapData _mapData;

        /**
         * マップの行数
         */
        public int RowCount => _mapTiles.Length;

        /**
         * マップの列数
         */
        public int ColumnCount { get; private set; }

        // マップ状態系
        private int _middleRow;

        public MapWrapper(MapData mapData)
        {
            _mapData = mapData;
            GenerateMap(mapData.Row, mapData.Column);
        }

        /**
     * マップの生成
     * @param row 行数
     * @param column 列数
     */
        public void GenerateMap(int row, int column)
        {
            // バリデーション
            if (row <= 0 || column <= 0) throw new ArgumentException("row and column must be greater than 0");

            if (row % 2 == 0) throw new ArgumentException("row must be even number");

            ColumnCount = column;

            // 予備計算
            _middleRow = (int)Math.Round((row - 1) / 2.0);
            // トータルのマス数計算（スタート・ボスは抜く）
            var totalTileCount = row; // まずは中心の文
            for (var i = 1; i <= _middleRow; i++)
                // 左右の列の数を足す
                totalTileCount += 2 * (column - i);

            // 乱数配列を作成
            var randomArray = new int[totalTileCount - 2];
            for (var i = 0; i < randomArray.Length; i++) randomArray[i] = i;

            var random = new System.Random();
            for (var i = 0; i < randomArray.Length; i++)
            {
                var temp = randomArray[i];
                var randomIndex = random.Next(i, randomArray.Length);
                randomArray[i] = randomArray[randomIndex];
                randomArray[randomIndex] = temp;
            }

            // マップの属性位置を決定
            var eliteIndex = new int[_mapData.EliteCount];
            Array.Copy(randomArray, eliteIndex, _mapData.EliteCount);
            var eventIndex = new int[_mapData.EventCount];
            Array.Copy(
                randomArray,
                _mapData.EliteCount,
                eventIndex,
                0,
                _mapData.EventCount
            );
            var shopIndex = new int[_mapData.ShopCount];
            Array.Copy(randomArray,
                _mapData.EliteCount + _mapData.EventCount,
                shopIndex,
                0,
                _mapData.ShopCount
            );

            var index = 0;

            _mapTiles = new MapTile[row][];
            for (var i = 0; i < row; i++)
            {
                // その行の列数を計算
                var columnCount = column - Math.Abs(_middleRow - i);
                _mapTiles[i] = new MapTile[columnCount];

                for (var j = 0; j < columnCount; j++)
                {
                    // アドレス変換
                    var (customRow, customColumn) = ConvertToCustomAddress(i, j);

                    // 左端はスタート
                    if (customRow == 0 && customColumn == 0)
                    {
                        _mapTiles[i][j] = new MapTile(MapTileType.Start, customRow, customColumn);
                    }
                    // 右端はボス
                    else if (i == _middleRow && j == columnCount - 1)
                    {
                        _mapTiles[i][j] = new MapTile(MapTileType.Boss, customRow, customColumn);
                    }
                    else
                    {
                        // それ以外はさっき振ったindexに応じて属性を振る
                        if (Array.IndexOf(eliteIndex, index) != -1)
                            _mapTiles[i][j] = new MapTile(MapTileType.Elite, customRow, customColumn);
                        else if (Array.IndexOf(eventIndex, index) != -1)
                            _mapTiles[i][j] = new MapTile(MapTileType.Event, customRow, customColumn);
                        else if (Array.IndexOf(shopIndex, index) != -1)
                            _mapTiles[i][j] = new MapTile(MapTileType.Shop, customRow, customColumn);
                        else
                            _mapTiles[i][j] = new MapTile(MapTileType.Normal, customRow, customColumn);

                        index++;
                    }
                }
            }
        }

        /**
     * マップを文字列化
     * 保存とデバッグ用
     */
        public override string ToString()
        {
            var result = "";

            for (var i = 0; i < RowCount; i++)
            {
                // その行の列数を取得
                var columnCount = _mapTiles[i].Length;

                for (var j = 0; j < columnCount; j++)
                {
                    result += _mapTiles[i][j].ToString();

                    // 最後の要素以外はカンマを追加
                    if (j != columnCount - 1) result += ",";
                }

                ;

                // 最後の要素以外は改行を追加
                if (i != RowCount - 1) result += "\n";
            }

            return result;
        }

        public MapTile[] GetRow(int i)
        {
            return _mapTiles[i];
        }

        /** 2次元配列のアドレスから独自形式のアドレスに変換 */
        private (int, int ) ConvertToCustomAddress(int arrayRow, int arrayColumn)
        {
            // 上下に難行追加されてるか計算
            var subRowCount = (RowCount - 1) / 2;

            // 列を考える
            // オフセットを計算
            var offset = (int)Math.Abs(_middleRow - arrayRow);

            // 列数を計算
            var convertedColumn = offset + arrayColumn * 2;


            // 合計で何列か計算
            var totalColumnCount = ColumnCount * 2 - 1;

            // 現在列に空白行がなんこあるか
            var blankRowCount = Math.Max(
                0,
                Math.Max(
                    (int)Math.Floor((subRowCount - convertedColumn) / 2.0),
                    (int)Math.Ceiling((convertedColumn + subRowCount - totalColumnCount) / 2.0)
                )
            );

            // 現在の行数を計算
            var convertedRow = arrayRow / 2 - blankRowCount;


            return (convertedRow, convertedColumn);
        }

        /** 独自形式のアドレスから2次元配列のアドレスに変換 */
        public (int, int ) ConvertToArrayAddress(int customRow, int customColumn)
        {
            // 上下に難行追加されてるか計算
            var subRowCount = (RowCount - 1) / 2;
            // カスタム形式で何列あるか
            var customColumnCount = ColumnCount * 2 - 1;

            // 列数確認
            if (customColumn < 0 || customColumn >= customColumnCount)
                throw new ArgumentException("customColumn is out of range");

            //  偶数列か奇数列か
            var isEvenColumn = customColumn % 2 == 0;

            // 現在行に何個空行があるか
            var blankRowCount = Math.Max(
                0,
                Math.Max(
                    (int)Math.Floor((subRowCount - customColumn) / 2.0),
                    (int)Math.Ceiling((customColumn + subRowCount - customColumnCount) / 2.0)
                )
            );

            // 何個目の行なのかを計算
            var convertedRow = isEvenColumn ? (customRow + blankRowCount) * 2 : (customRow + blankRowCount) * 2 + 1;

            // 何個目の帰趨・偶数列なのかを計算
            var currentColumnCount = (int)Math.Floor(customColumn / 2.0);

            // 行がどれだけ離れてるか
            var distanceFromMiddleRow = Math.Abs(_middleRow - convertedRow);

            // 列のオフセットを計算
            var columnOffset = (int)Math.Min(
                Math.Floor(distanceFromMiddleRow / 2.0),
                subRowCount / 2.0
            );

            // 変換
            var convertedColumn = currentColumnCount - columnOffset;

            return (convertedRow, convertedColumn);
        }
    }
}