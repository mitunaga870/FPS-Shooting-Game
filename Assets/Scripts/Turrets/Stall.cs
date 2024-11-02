using System.Collections.Generic;
using AClass;
using DataClass;
using JetBrains.Annotations;
using lib;
using UnityEngine;

namespace Turrets
{
    public class Stall : ATurret
    {
        private const int Height = 1;
        private const int Interval = 500;
        private const string TurretName = "Stall";
        private const int Duration = 200;

        public override float GetHeight()
        {
            return Height;
        }

        protected override void AwakeTurret(List<AEnemy> enemies)
        {
            // 自身から最も近い道路の座標を取得
            var destination = MazeController.GetClosestRoadTilePosition(SetPosition);
            foreach (var enemy in enemies)
            {
                // 自分の位置を敵の目的地に設定
                enemy.SetDestination(destination);

                // 指定時間後目的地を外す
                var delay = General.DelayCoroutineByGameTime(
                    sceneController,
                    Duration,
                    () => enemy.ReleaseDestination());
                StartCoroutine(delay);
            }
        }

        [CanBeNull]
        public override List<TilePosition> GetEffectArea()
        {
            return null;
        }

        public override string GetTurretName()
        {
            return TurretName;
        }

        public override int GetInterval()
        {
            return Interval;
        }

        public override void SetAngle(int angle)
        {
        }
    }
}