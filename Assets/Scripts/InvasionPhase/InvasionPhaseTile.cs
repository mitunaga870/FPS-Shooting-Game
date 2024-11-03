using System;
using AClass;
using DataClass;
using Enums;
using UnityEngine;

namespace InvasionPhase
{
    public class InvasionPhaseTile : ATile
    {
        private InvasionController _sceneController;

        private int prevTime = 0;

        // ======== 燃焼床系の処理 =========
        public bool IsIgniteFloor { get; private set; }
        public int IgniteDamage { get; private set; }
        public int IgniteDuration { get; private set; }
        // =================================

        /** 初期化処理 */
        public void Initialize(int row, int column, TileTypes tileType, RoadAdjust roadAdjust,
            InvasionController sceneController)
        {
            _sceneController = sceneController;
            Row = row;
            Column = column;

            // タイルのステータスによって処理を変える
            switch (tileType)
            {
                case TileTypes.Nothing:
                    SetNone();
                    break;
                case TileTypes.Road:
                case TileTypes.Start:
                case TileTypes.Goal:
                    SetRoad(roadAdjust);
                    break;
                default:
                    throw new Exception("未対応のタイルタイプです" + tileType);
            }
        }

        /**
         * 侵攻phaseようにセットアップしたトラップを設置する
         */
        public void SetInvasionTrap(
            string trapName,
            int trapAngle,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController
        )
        {
            var result = SetTrap(mazeController, trapName, trapAngle);

            // 侵攻phase用に初期化
            Trap.InvasionInitialize(sceneController, mazeController, enemyController);
        }

        public void SetInvasionTurret(
            string turretTurret,
            int angle,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController
        )
        {
            SetTurret(turretTurret, angle);

            // 侵攻phase用に初期化
            Turret.InvasionInitialize(
                new TilePosition(Row, Column),
                sceneController,
                mazeController,
                enemyController
            );
        }

        public TilePosition getPosition()
        {
            return new TilePosition(Row, Column);
        }

        // ======== 燃焼床系の処理 =========
        public void IgniteFloor(InvasionController sceneController, int igniteDamage, int igniteDuration)
        {
            IsIgniteFloor = true;
            IgniteDamage = igniteDamage;
            IgniteDuration = igniteDuration;

            // とりあえずタイル赤くする
            SetColor(Color.red);
        }

        private void Update()
        {
            // 時間計算
            var currentTime = _sceneController.GameTime;
            var deltaTime = currentTime - prevTime;
            prevTime = currentTime;

            // 燃焼床の処理
            if (!IsIgniteFloor) return;

            IgniteDuration -= deltaTime;

            if (IgniteDuration <= 0)
            {
                IsIgniteFloor = false;
                ResetColor();
            }
        }
    }
}