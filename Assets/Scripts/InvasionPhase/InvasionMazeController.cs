using AClass;
using DataClass;
using UnityEngine;
using UnityEngine.Serialization;

namespace InvasionPhase
{
    public class InvasionMazeController : AMazeController
    {
        /** タイルのプレハブ */
        [FormerlySerializedAs("tilePrefab")] [SerializeField]
        private InvasionPhaseTile createPhaseTilePrefab;

        /**
         * 迷路の配列
         */
        private InvasionPhaseTile[][] _maze;

        private Vector3 _mazeOrigin;


        public void Create(TileData[][] tiles, TrapData[] trapData)
        {
            var mazeRows = tiles.Length;
            var mazeColumns = tiles[0].Length;

            // 原点を設定
            _mazeOrigin = new Vector3(-(mazeColumns - 1) / 2.0f, 0, -(mazeRows - 1) / 2.0f);

            // すべてのタイルを生成し、初期化する
            // 行の初期化
            _maze = new InvasionPhaseTile[mazeRows][];
            for (var row = 0; row < mazeRows; row++)
            {
                // 列の初期化
                _maze[row] = new InvasionPhaseTile[mazeColumns];
                for (var column = 0; column < mazeColumns; column++)
                {
                    var tileData = tiles[row][column];

                    // タイルの位置と回転を設定
                    var tilePosition = new Vector3(column, 0, row) * Environment.TileSize + _mazeOrigin;
                    var tileRotation = Quaternion.Euler(-90, 0, 0);
                    // タイルを生成し、初期化する
                    var newTile = Instantiate(createPhaseTilePrefab, tilePosition, tileRotation);
                    newTile.Initialize(row, column, tileData.TileType, tileData.RoadAdjust);

                    // タイルを迷路に追加する
                    _maze[row][column] = newTile;
                }
            }

            // スタート・ゴールのタイルを設定
            _maze[StartPosition.Row][StartPosition.Col].SetStart();
            _maze[GoalPosition.Row][GoalPosition.Col].SetGoal();
        }

        protected override void Sync()
        {
            var syncTiles = new ATile[_maze.Length][];
            for (var i = 0; i < MazeRows; i++)
            {
                syncTiles[i] = new ATile[_maze[i].Length];
                for (var j = 0; j < MazeColumns; j++)
                {
                    syncTiles[i][j] = _maze[i][j];
                }
            }

            SyncMazeData(syncTiles);
        }
    }
}