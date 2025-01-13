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
        [SerializeField]
        private AudioSource audioSource;
        
        [SerializeField]
        private AudioClip awakeSound;
        
        private const string TrapName = "Car";
        private int Damage => trapObject.CarDamage;
        private float Height => trapObject.CarHeight;
        private int CoolDown => trapObject.CarCoolDown;
        private int AttackRange => trapObject.CarAttackRange;
        private int SetRange => trapObject.CarSetRange;
        
        [SerializeField]
        private TrapBeetleIgnitionAction trapBeetleIgnitionAction;


        private int _angle = -1;

        /** アニメーション全体の時間 */
        private const int Duration = 100;

        public override void AwakeTrap(TilePosition position)
        {
            if (EnemyController == null || MazeController == null) return;
            
            // チャージ中は無効
            if (0 < ChargeTime) return;
            
            // 音声再生
            audioSource.PlayOneShot(awakeSound);

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
            
            // ダメージ取得
            var damage = GetDamage();
            
            var firstCoroutine = General.DelayCoroutineByGameTime(
                SceneController,
                time,
                () => { EnemyController.DamageEnemy(targetTiles[1], damage); });
            var secondCoroutine = General.DelayCoroutineByGameTime(
                SceneController,
                time * 2,
                () => EnemyController.DamageEnemy(targetTiles[2], damage)
            );
            var thirdCoroutine = General.DelayCoroutineByGameTime(
                SceneController,
                time * 3,
                () => EnemyController.DamageEnemy(targetTiles[3], damage)
            );
            
            // 車の最後のタイルの座標を取得
            var lastTilePosition = targetTiles[^1].ToVector3(MazeController.MazeOrigin);

            Debug.Log("mazeOrigin: " + MazeController.MazeOrigin);
            Debug.Log("destination: " + lastTilePosition);

            // 車のアニメーション
            trapBeetleIgnitionAction.IgnitionAction(lastTilePosition, Duration * 0.02f);

            EnemyController.DamageEnemy(targetTiles[0], damage);
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

        public override int GetDefaultDamage()
        {
            return Damage;
        }
    }
}