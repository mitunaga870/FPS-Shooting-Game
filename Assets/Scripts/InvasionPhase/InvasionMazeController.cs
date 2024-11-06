using System;
using System.Collections.Generic;
using AClass;
using DataClass;
using Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace InvasionPhase
{
    public class InvasionMazeController : AMazeController
    {
        /** タイルのプレハブ */
        [FormerlySerializedAs("tilePrefab")]
        [SerializeField]
        private InvasionPhaseTile createPhaseTilePrefab;

        [SerializeField]
        private InvasionController sceneController;

        [SerializeField]
        private InvasionEnemyController enemyController;

        /**
         * 迷路の配列
         */
        private InvasionPhaseTile[][] _maze;

        public Vector3 MazeOrigin { private set; get; }
        private TileData[][] TileData { get; set; }
        private TrapData[] TrapData { get; set; }

        public void Create(TileData[][] tiles, TrapData[] trapData, TurretData[] turretData)
        {
            var mazeRows = tiles.Length;
            var mazeColumns = tiles[0].Length;

            // 原点を設定
            MazeOrigin = new Vector3(-(mazeColumns - 1) / 2.0f, 0, -(mazeRows - 1) / 2.0f);

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
                    var tilePosition = new Vector3(column, 0, row) * Environment.TileSize + MazeOrigin;
                    var tileRotation = Quaternion.Euler(-90, 0, 0);
                    // タイルを生成し、初期化する
                    var newTile = Instantiate(createPhaseTilePrefab, tilePosition, tileRotation);
                    newTile.Initialize(
                        row, column,
                        tileData.TileType,
                        tileData.RoadAdjust,
                        sceneController,
                        this
                    );

                    // タイルを迷路に追加する
                    _maze[row][column] = newTile;
                }
            }

            // トラップを設定
            foreach (var trap in trapData)
                _maze[trap.Row][trap.Column].SetInvasionTrap(
                    trap.Trap,
                    trap.Angle,
                    sceneController,
                    this,
                    enemyController
                );

            // タレットを設定
            foreach (var turret in turretData)
                _maze[turret.Row][turret.Column].SetInvasionTurret(
                    turret.Turret,
                    turret.angle,
                    sceneController,
                    this,
                    enemyController
                );

            // スタート・ゴールのタイルを設定
            _maze[StartPosition.Row][StartPosition.Col].SetStart();
            _maze[GoalPosition.Row][GoalPosition.Col].SetGoal();

            // データを保存
            TileData = tiles;
            TrapData = trapData;
            TurretData = new List<TurretData>(turretData);
        }

        protected override void Sync()
        {
            var syncTiles = new ATile[_maze.Length][];
            for (var i = 0; i < MazeRows; i++)
            {
                syncTiles[i] = new ATile[_maze[i].Length];
                for (var j = 0; j < MazeColumns; j++) syncTiles[i][j] = _maze[i][j];
            }

            SyncMazeData(syncTiles);
        }

        public override ATile GetTile(int row, int column)
        {
            if (row < 0 || row >= MazeRows || column < 0 || column >= MazeColumns) return null;

            return _maze[row][column];
        }

        public void AwakeTrap(TilePosition position)
        {
            var tile = _maze[position.Row][position.Col];

            tile.AwakeTrap();
        }

        /**
         * 指定タイルから最も近い道路のタイルの座標を取得
         */
        public TilePosition GetClosestRoadTilePosition(TilePosition originPosition)
        {
            // ベース値作成
            var minDistance = float.MaxValue;
            var result = new TilePosition(0, 0);

            // すべてのタイルを検索
            foreach (var row in _maze)
            foreach (var tile in row)
            {
                // 道路でない場合はスキップ
                if (tile.TileType != TileTypes.Road) continue;

                var tilePosition = tile.GetPosition();

                // 距離を計算
                var distance = TilePosition.GetDistance(
                    tilePosition,
                    originPosition
                );

                // 最小値を更新
                if (distance >= minDistance) continue;

                minDistance = distance;
                result = tilePosition;
            }

            return result;
        }


        public void IgniteFloor(TilePosition targetCurrentPosition, int igniteDamage, int igniteDuration)
        {
            if (targetCurrentPosition.Row < 0 || targetCurrentPosition.Row >= MazeRows ||
                targetCurrentPosition.Col < 0 || targetCurrentPosition.Col >= MazeColumns) return;

            var tile = _maze[targetCurrentPosition.Row][targetCurrentPosition.Col];

            tile.IgniteFloor(sceneController, igniteDamage, igniteDuration);
        }

        public bool IsIgnite(TilePosition currentPosition)
        {
            if (currentPosition.Row < 0 || currentPosition.Row >= MazeRows ||
                currentPosition.Col < 0 || currentPosition.Col >= MazeColumns) return false;

            var tile = _maze[currentPosition.Row][currentPosition.Col];

            return tile.IsIgniteFloor;
        }

        private void OnApplicationQuit()
        {
            // シーン遷移で読み込んだデータをそのまま保存
            SaveController.SaveTileData(TileData);
            SaveController.SaveTrapData(TrapData);
            SaveController.SaveStageData(StageData);
            SaveController.SaveTurretData(TurretData);
        }

        public void PreviewSkillEffectArea(TilePosition tilePosition, ASkill skill, float duration)
        {
            // スキルの効果範囲を取得
            var effectArea = skill.GetSkillEffectArea(this, tilePosition);
            
            ShowEffectRange(effectArea?.ToArray(), duration);
        }

        public void SetWarpHole(TilePosition sourcePosition, TilePosition destinationPosition, int duration)
        {
            _maze[sourcePosition.Row][sourcePosition.Col].SetWarpHoleSource(destinationPosition, duration);
            _maze[destinationPosition.Row][destinationPosition.Col].SetWarpHoleDestination(sourcePosition, duration);
        }

        public bool IsTeleport(TilePosition currentPosition)
        {
            return _maze[currentPosition.Row][currentPosition.Col].IsWarpHole;
        }

        /**
         * 指定範囲のturret・トラップにダメージを追加
         */
        public void AddDamage(List<TilePosition> effectArea, int addDamage, int duration)
        {
            foreach (var position in effectArea)
            {
                if (position.Row < 0 || position.Row >= MazeRows ||
                    position.Col < 0 || position.Col >= MazeColumns) continue;

                var tile = _maze[position.Row][position.Col];

                tile.AddDamage(addDamage, duration);
            }
        }

        public void OverrideSkillTime(List<TilePosition> effectArea, int duration)
        {
            foreach (var position in effectArea)
            {
                if (position.Row < 0 || position.Row >= MazeRows ||
                    position.Col < 0 || position.Col >= MazeColumns) continue;

                var tile = _maze[position.Row][position.Col];

                tile.OverrideSkillTime(duration);
            }
        }

        public void SetSlowArea(
            List<TilePosition> targetPosition,
            int duration,
            float slowRate
        ) {
            // タイルを走査
            foreach (var position in targetPosition)
            {
                if (position.Row < 0 || position.Row >= MazeRows ||
                    position.Col < 0 || position.Col >= MazeColumns) continue;

                var tile = _maze[position.Row][position.Col];

                tile.SetSlowArea(duration, slowRate);
            }
        }

        public bool IsSlow(TilePosition currentPosition)
        {
            if (currentPosition.Row < 0 || currentPosition.Row >= MazeRows ||
                currentPosition.Col < 0 || currentPosition.Col >= MazeColumns) return false;

            var tile = _maze[currentPosition.Row][currentPosition.Col];

            return tile.IsSlowArea;
        }

        public void SetBlockArea(List<TilePosition> target, int duration, bool reCalculationPath)
        {
            // タイルを走査
            foreach (var position in target)
            {
                // 範囲外の場合はスキップ
                if (position.Row < 0 || position.Row >= MazeRows ||
                    position.Col < 0 || position.Col >= MazeColumns) continue;

                var tile = _maze[position.Row][position.Col];

                tile.SetBlockArea(duration);
            }
            
            // パスの再計算
            if (reCalculationPath) enemyController.ReCalculationPath();
        }

        public void SetNockBackArea(List<TilePosition> targetTiles, int distance, int stunTime, Action callback)
        {
            // タイルを走査
            foreach (var position in targetTiles)
            {
                // 範囲外の場合はスキップ
                if (position.Row < 0 || position.Row >= MazeRows ||
                    position.Col < 0 || position.Col >= MazeColumns) continue;

                var tile = _maze[position.Row][position.Col];

                tile.SetNockBackArea(distance, stunTime, callback);
            }
        }

        public bool IsKnockBack(TilePosition currentPosition)
        {
            if (currentPosition.Row < 0 || currentPosition.Row >= MazeRows ||
                currentPosition.Col < 0 || currentPosition.Col >= MazeColumns) return false;

            var tile = _maze[currentPosition.Row][currentPosition.Col];

            return tile.IsKnockBackArea;
        }
    }
}