using System;
using System.Collections.Generic;
using AClass;
using DataClass;
using Ignition_action;
using lib;
using UnityEngine;

namespace Traps
{
    public class Car : ATrap
    {
        private const string TrapName = "Car";
        private const int SetRange = 1;
        private const float Height = 0.18f;
        private const int Damage = 1;
        private const int CoolDown = 5000;
        private const int AttackRange = 4;
        
        [SerializeField]
        private TrapBeetleIgnitionAction trapBeetleIgnitionAction;


        private int _angle = -1;

        /** アニメーション全体の時間 */
        private const int Duration = 100;

        public override void AwakeTrap(TilePosition position)
        {
            if (enemyController == null || mazeController == null) return;
            
            var currentPos = transform.position;

            // チャージ中は無効
            if (0 < ChargeTime) return;

            // CD設定
            ChargeTime = CoolDown;

            // 一タイル辺りの時間
            var time = (int)Math.Round(Duration / (float)AttackRange);

            // ターゲットタイルを取る
            var targetTiles = new List<TilePosition>() { position };
            for (var i = 1; i <= AttackRange; i++)
            {
                // 角度の正規化
                var normalizedAngle = _angle % 360;

                switch (normalizedAngle)
                {
                    case 0:
                        targetTiles.Add(targetTiles[^1].GetUp());
                        break;
                    case 90:
                        targetTiles.Add(targetTiles[^1].GetRight());
                        break;
                    case 180:
                        targetTiles.Add(targetTiles[^1].GetDown());
                        break;
                    case 270:
                        targetTiles.Add(targetTiles[^1].GetLeft());
                        break;
                }
            }
            
            var firstCoroutine = General.DelayCoroutineByGameTime(
                sceneController,
                time,
                () => { enemyController.DamageEnemy(targetTiles[1], Damage); });
            var secondCoroutine = General.DelayCoroutineByGameTime(
                sceneController,
                time * 2,
                () => enemyController.DamageEnemy(targetTiles[2], Damage)
            );
            var thirdCoroutine = General.DelayCoroutineByGameTime(
                sceneController,
                time * 3,
                () => enemyController.DamageEnemy(targetTiles[3], Damage)
            );
            
            // 車の最後のタイルの座標を取得
            var lastTilePosition = targetTiles[^1].ToVector3(mazeController.MazeOrigin);

            Debug.Log("mazeOrigin: " + mazeController.MazeOrigin);
            Debug.Log("destination: " + lastTilePosition);

            // 車のアニメーション
            trapBeetleIgnitionAction.IgnitionAction(lastTilePosition, Duration * 0.02f);

            enemyController.DamageEnemy(targetTiles[0], Damage);
            StartCoroutine(firstCoroutine);
            StartCoroutine(secondCoroutine);
            StartCoroutine(thirdCoroutine);
        }

        public override float GetHeight()
        {
            return Height;
        }

        public override int GetSetRange()
        {
            return SetRange;
        }

        public override string GetTrapName()
        {
            return TrapName;
        }

        public override int GetTrapAngle()
        {
            // -1の場合はランダム
            if (_angle == -1) _angle = UnityEngine.Random.Range(0, 4) * 90;

            return _angle;
        }

        public override void SetAngle(int trapAngle)
        {
            _angle = trapAngle;
        }
    }
}