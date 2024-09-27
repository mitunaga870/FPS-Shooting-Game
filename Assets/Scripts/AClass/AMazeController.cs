using System;
using System.Collections.Generic;
using DataClass;
using Enums;
using JetBrains.Annotations;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace AClass
{
    public abstract class AMazeController : MonoBehaviour
    {
        /** 各迷路の行列数等の情報格納するスクリプタブルオブジェクト */
        [Header("迷路データ")] [SerializeField] private StageObject stageObject;

        /** 汎用情報 */
        [FormerlySerializedAs("GeneralS2SData")] [SerializeField]
        protected GeneralS2SData generalS2SData;

        /** 迷路タイル配列 */
        protected ATile[][] Maze { get; set; }

        /**
         * 全体を同期する
         */
        protected abstract void Sync();

        // TODO: 進捗と選択状況からステージデータをとる。いったんノーマルを取っておく
        [NonSerialized] public StageData StageData;
        public int Level => generalS2SData.Level;
        public int Stage => generalS2SData.Stage;
        public int MazeRows => StageData.mazeRow;
        public int MazeColumns => StageData.mazeColumn;
        public TilePosition StartPosition => StageData.start;
        public TilePosition GoalPosition => StageData.goal;
        public int ReRollWaitTime => StageData.reRollWaitTime;

        public int TrapCount
        {
            get => _placedTrapCount == -1
                ? StageData.trapCount
                : _placedTrapCount;
            set => _placedTrapCount = value;
        }

        private int _placedTrapCount = -1;

        void Awake()
        {
            // セーブデータからステージデータをとる
            StageData =
                SaveController.LoadStageData(stageObject)
                ?? stageObject.getNormalStageData();
        }

        /**
         * ベースの迷路配列と指定配列を同期する
         */
        protected void SyncMazeData(ATile[][] maze)
        {
            Maze = new ATile[maze.Length][];
            for (var i = 0; i < maze.Length; i++)
            {
                Maze[i] = new ATile[maze[i].Length];
                for (var j = 0; j < maze[i].Length; j++)
                {
                    Maze[i][j] = maze[i][j];
                }
            }
        }

        /**
     * 現在の迷路のスタートからゴールまでの最短経路を出す
     */
        [CanBeNull]
        public Path GetShortestS2GPath()
        {
            if (StartPosition == null || GoalPosition == null)
                throw new ArgumentNullException();

            return GetShortestPath(StartPosition, GoalPosition);
        }

        /**
         * 現在の迷路での指定地点間の最短経路を出す
         * ないときはnull
         */
        [CanBeNull]
        public Path GetShortestPath(TilePosition start, TilePosition destination)
        {
            Sync();

            // 検索中のパスを保持するリスト
            var pathList = new List<Path>();
            if (pathList == null) throw new ArgumentNullException(nameof(pathList));

            // スタートを登録
            pathList.Add(new Path(start));

            while (true)
            {
                // 次用のパスを保持するリスト
                var nextPathList = new List<Path>();
                if (nextPathList == null) throw new ArgumentNullException(nameof(nextPathList));

                // すべてのパスに対して次のステップを取得
                foreach (var path in pathList)
                {
                    // 次のステップのパスを取得
                    var paths = GetNextStepPaths(path);

                    // 次のステップのパスがない場合別を当たる
                    if (paths == null) continue;

                    // ゴールにたどりついた者があればそれを返す
                    foreach (var newPath in paths)
                    {
                        if (newPath.GetLast().Equals(destination))
                        {
                            return newPath;
                        }
                    }

                    // 次のステップのパスをリストに追加
                    nextPathList.AddRange(paths);
                }

                // 次のステップのパスがない場合はnullを返す
                if (nextPathList.Count == 0) return null;

                // 現在入手したパスを次の検索用に更新
                pathList = nextPathList;
            }
        }

        /**
     * 隣接した道を取得する
     */
        [CanBeNull]
        private Path[] GetNextStepPaths(Path path)
        {
            var tilePosition = path.GetLast();

            var nextTile = new TilePosition[4];
            var index = 0;

            // 隣接が道かつ、パスに含まれていない場合のみ追加
            // 上
            if (tilePosition.Row - 1 >= 0 && Maze[tilePosition.Row - 1][tilePosition.Col].TileType == TileTypes.Road &&
                !path.Contains(tilePosition.Row - 1, tilePosition.Col))
            {
                nextTile[index] = new TilePosition(tilePosition.Row - 1, tilePosition.Col);
                index++;
            }

            // 下
            if (tilePosition.Row + 1 < MazeRows &&
                Maze[tilePosition.Row + 1][tilePosition.Col].TileType == TileTypes.Road &&
                !path.Contains(tilePosition.Row + 1, tilePosition.Col))
            {
                nextTile[index] = new TilePosition(tilePosition.Row + 1, tilePosition.Col);
                index++;
            }

            // 左
            if (tilePosition.Col - 1 >= 0 && Maze[tilePosition.Row][tilePosition.Col - 1].TileType == TileTypes.Road &&
                !path.Contains(tilePosition.Row, tilePosition.Col - 1))
            {
                nextTile[index] = new TilePosition(tilePosition.Row, tilePosition.Col - 1);
                index++;
            }

            // 右
            if (tilePosition.Col + 1 < MazeColumns &&
                Maze[tilePosition.Row][tilePosition.Col + 1].TileType == TileTypes.Road &&
                !path.Contains(tilePosition.Row, tilePosition.Col + 1))
            {
                nextTile[index] = new TilePosition(tilePosition.Row, tilePosition.Col + 1);
                index++;
            }

            // 隣接する道がない場合はnullを返す
            if (index == 0) return null;

            var result = new Path[index];
            for (var i = 0; i < index; i++)
            {
                result[i] = path.Add(nextTile[i]);
            }

            return result;
        }


        /**
         * タイルのつながり肩を取得
         */
        protected RoadAdjust GetRoadAdjust(int col, int row, List<Dictionary<string, int>> roadTileAddress)
        {
            // 上下左右のタイルがつながっているか
            var bottom = row - 1 >= 0 &&
                         roadTileAddress.Exists(address => address["col"] == col && address["row"] == row - 1);
            var left = col - 1 >= 0 &&
                       roadTileAddress.Exists(address => address["col"] == col - 1 && address["row"] == row);
            var right = col + 1 < MazeColumns &&
                        roadTileAddress.Exists(address => address["col"] == col + 1 && address["row"] == row);
            var top = row + 1 < MazeRows &&
                      roadTileAddress.Exists(address => address["col"] == col && address["row"] == row + 1);
            // 斜めのタイルがつながっているか
            var topLeft = row + 1 < MazeRows && col - 1 >= 0 &&
                          roadTileAddress.Exists(address =>
                              address["col"] == col - 1 && address["row"] == row + 1);
            var topRight = row + 1 < MazeRows && col + 1 < MazeColumns &&
                           roadTileAddress.Exists(
                               address => address["col"] == col + 1 && address["row"] == row + 1);
            var bottomLeft = row - 1 >= 0 && col - 1 >= 0 &&
                             roadTileAddress.Exists(address =>
                                 address["col"] == col - 1 && address["row"] == row - 1);
            var bottomRight = row - 1 >= 0 && col + 1 < MazeColumns &&
                              roadTileAddress.Exists(address =>
                                  address["col"] == col + 1 && address["row"] == row - 1);

            // 壁なし
            if (top && left && right && bottom && topLeft && topRight && bottomLeft && bottomRight)
            {
                return RoadAdjust.NoWall;
            }
            // 太い道路のL字内側
            else if (top && left && right && bottom && topRight && bottomLeft && bottomRight)
            {
                return RoadAdjust.TopLeftDot;
            }
            else if (top && left && right && bottom && topLeft && topRight && bottomRight)
            {
                return RoadAdjust.BottomLeftDot;
            }
            else if (top && left && right && bottom && topLeft && bottomRight && bottomLeft)
            {
                return RoadAdjust.TopRightDot;
            }
            else if (top && left && right && bottom && topLeft && bottomLeft && topRight)
            {
                return RoadAdjust.BottomRightDot;
            }
            // 太い道路から細い道路分岐
            else if (left && right && bottom && top && topLeft && topRight)
            {
                return RoadAdjust.TopDoubleDot;
            }
            else if (left && right && bottom && top && bottomLeft && bottomRight)
            {
                return RoadAdjust.BottomDoubleDot;
            }
            else if (left && right && bottom && top && topLeft && bottomLeft)
            {
                return RoadAdjust.LeftDoubleDot;
            }
            else if (left && right && bottom && top && topRight && bottomRight)
            {
                return RoadAdjust.RightDoubleDot;
            }
            // 斜め点　２個ない
            else if (left && right && top && bottom && topRight && bottomLeft)
            {
                return RoadAdjust.TopRightAndBottomLeftDot;
            }
            else if (left && right && top && bottom && topLeft && bottomRight)
            {
                return RoadAdjust.TopLeftAndBottomRightDot;
            }
            // 3つ点
            else if (left && right && top && bottom && bottomRight)
            {
                return RoadAdjust.ExpectBottomRightDot;
            }
            else if (left && right && top && bottom && bottomLeft)
            {
                return RoadAdjust.ExpectBottomLeftDot;
            }
            else if (left && right && top && bottom && topRight)
            {
                return RoadAdjust.ExpectTopRightDot;
            }
            else if (left && right && top && bottom && topLeft)
            {
                return RoadAdjust.ExpectTopLeftDot;
            }
            // 太い道路の直線片側
            else if (left && right && bottom && bottomLeft && bottomRight)
            {
                return RoadAdjust.TopWall;
            }
            else if (left && right && top && topLeft && topRight)
            {
                return RoadAdjust.BottomWall;
            }
            else if (top && bottom && left && topLeft && bottomLeft)
            {
                return RoadAdjust.RightWall;
            }
            else if (top && bottom && right && topRight && bottomRight)
            {
                return RoadAdjust.LeftWall;
            }
            // 太い道路の直線片側＋右角の内側（壁を下として）
            else if (left && right && top && topLeft)
            {
                return RoadAdjust.BottomWallWithRightDot;
            }
            else if (left && right && top && topRight)
            {
                return RoadAdjust.BottomWallWithLeftDot;
            }
            else if (top && right && bottom & topRight)
            {
                return RoadAdjust.LeftWallWithBottomDot;
            }
            else if (top && right && bottom && bottomRight)
            {
                return RoadAdjust.LeftWallWithTopDot;
            }
            else if (right && bottom && left && bottomRight)
            {
                return RoadAdjust.TopWallWithLeftDot;
            }
            else if (right && bottom && left && bottomLeft)
            {
                return RoadAdjust.TopWallWithRightDot;
            }
            else if (bottom && left && top && topLeft)
            {
                return RoadAdjust.RightWallWithBottomDot;
            }
            else if (bottom && left && top && bottomLeft)
            {
                return RoadAdjust.RightWallWithTopDot;
            }
            // 太い道路のL字外側
            else if (top && left && topLeft)
            {
                return RoadAdjust.TopLeftHalfOnce;
            }
            else if (top && right && topRight)
            {
                return RoadAdjust.TopRightHalfOnce;
            }
            else if (bottom && left && bottomLeft)
            {
                return RoadAdjust.BottomLeftHalfOnce;
            }
            else if (right && bottom && bottomRight)
            {
                return RoadAdjust.BottomRightHalfOnce;
            }
            // 十字
            else if (top && left && right && bottom)
            {
                return RoadAdjust.Cross;
            }
            // T字
            else if (top && left && right)
            {
                return RoadAdjust.TopRightLeft;
            }
            else if (top && left && bottom)
            {
                return RoadAdjust.LeftTopBottom;
            }
            else if (top && right && bottom)
            {
                return RoadAdjust.RightBottomTop;
            }
            else if (left && right && bottom)
            {
                return RoadAdjust.BottomLeftRight;
            }
            // L字
            else if (top && left)
            {
                return RoadAdjust.TopLeft;
            }
            else if (top && right)
            {
                return RoadAdjust.TopRight;
            }
            else if (left && bottom)
            {
                return RoadAdjust.BottomLeft;
            }
            else if (right && bottom)
            {
                return RoadAdjust.RightBottom;
            }
            // 直線
            else if (top && bottom)
            {
                return RoadAdjust.TopBottom;
            }
            else if (left && right)
            {
                return RoadAdjust.LeftRight;
            }
            // 行き止まり
            else if (top)
            {
                return RoadAdjust.TopDeadEnd;
            }
            else if (left)
            {
                return RoadAdjust.LeftDeadEnd;
            }
            else if (right)
            {
                return RoadAdjust.RightDeadEnd;
            }
            else if (bottom)
            {
                return RoadAdjust.BottomDeadEnd;
            }
            else
            {
                return RoadAdjust.NoAdjust;
            }
        }
    }
}