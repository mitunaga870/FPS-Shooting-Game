using System;
using DataClass;
using Enums;
using UnityEngine;

namespace Map
{
    [Serializable]
    public class MapWrapper
    {
        /**
         * 各行のマスリスト
         */
        private MapTile[][] mapTiles;

        /**
         * マップの行数
         */
        public int rowCount => mapTiles.Length;

        /**
         * マップの列数
         */
        public int columnCount => mapTiles[(int)Math.Round(rowCount / 2.0)].Length;

        /**
         * マップの中心座標
         */
        public Vector2 centerPosition => new(columnCount / 2.0f, rowCount / 2.0f);

        private MapWrapper()
        {
        }

        public MapWrapper(int row, int column)
        {
            GenerateMap(row, column);
        }

        public MapWrapper(MapData mapData)
        {
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

            // 予備計算
            var middleRow = (int)Math.Round((row - 1) / 2.0);

            mapTiles = new MapTile[row][];
            for (var i = 0; i < row; i++)
            {
                // その行の列数を計算
                var columnCount = column - Math.Abs(middleRow - i);
                mapTiles[i] = new MapTile[columnCount];

                for (var j = 0; j < columnCount; j++)
                    // 左端はスタート
                    if (i == middleRow && j == 0)
                        mapTiles[i][j] = new MapTile(MapTileType.Start);
                    // 右端はボス
                    else if (i == middleRow && j == columnCount - 1)
                        mapTiles[i][j] = new MapTile(MapTileType.Boss);
                    // それ以外はランダム
                    else
                        mapTiles[i][j] = new MapTile();
            }
        }

        /**
         * マップを文字列化
         * 保存とデバッグ用
         */
        public override string ToString()
        {
            var result = "";

            for (var i = 0; i < rowCount; i++)
            {
                // その行の列数を取得
                var columnCount = mapTiles[i].Length;

                for (var j = 0; j < columnCount; j++)
                {
                    result += mapTiles[i][j].ToString();

                    // 最後の要素以外はカンマを追加
                    if (j != columnCount - 1) result += ",";
                }

                ;

                // 最後の要素以外は改行を追加
                if (i != rowCount - 1) result += "\n";
            }

            return result;
        }

        public MapTile[] GetRow(int i)
        {
            return mapTiles[i];
        }
    }
}