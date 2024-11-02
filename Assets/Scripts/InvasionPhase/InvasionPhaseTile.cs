using System;
using AClass;
using DataClass;
using Enums;
using lib;
using Unity.VisualScripting;
using UnityEngine;

namespace InvasionPhase
{
    public class InvasionPhaseTile : ATile
    {
        /** 初期化処理 */
        public void Initialize(int row, int column, TileTypes tileType, RoadAdjust roadAdjust)
        {
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
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController
        )
        {
            var result = SetTrap(mazeController, trapName);

            // 侵攻phase用に初期化
            _trap.InvasionInitialize(sceneController, enemyController);
        }

        public void SetInvasionTurret(
            string turretTurret,
            int angle,
            InvasionController sceneController,
            InvasionEnemyController enemyController
        )
        {
            SetTurret(turretTurret, angle);

            // 侵攻phase用に初期化
            Turret.InvasionInitialize(
                new TilePosition(Row, Column),
                sceneController,
                enemyController
            );
        }
    }
}