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

        private Vector3 _mazeOrigin;


        public void Create(TileData[][] tiles, TrapData[] trapData)
        {
            var mazeRows = tiles.Length;
            var mazeColumns = tiles[0].Length;

            // 原点を設定
            _mazeOrigin = new Vector3(-(mazeColumns - 1) / 2.0f, 0, -(mazeRows - 1) / 2.0f);
            // すべてのタイルを生成し、初期化する
            // 行の初期化
            Maze = new ATile[mazeRows][];
            for (var row = 0; row < mazeRows; row++)
            {
                // 列の初期化
                Maze[row] = new ATile[mazeColumns];
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
                    Maze[row][column] = newTile;
                }
            }
        }

        protected override void Sync()
        {
            throw new System.NotImplementedException();
        }
    }
}