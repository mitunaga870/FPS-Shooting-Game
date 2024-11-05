using System.Collections.Generic;
using AClass;
using DataClass;
using lib;
using Pallab.TinyTank;
using UnityEngine;

namespace Turrets
{
    public class Missile : ATurret
    {
        private const float Height = 0.13f;
        private const int Damage = 1;
        private const int ObjectDuration = 10;
        private const int IgniteDamage = 0;
        private const int IgniteDuration = 100;
        private const int Interval = 500;
        private const string TurretName = "Missile";
        
        [SerializeField]
        TinyTankAnimController animController;
        [SerializeField]
        TinyTankTurretRotator turretRotator;
        
        public override float GetHeight()
        {
            return Height;
        }

        protected override void AwakeTurret(List<AEnemy> enemies)
        {
            // 最も近い敵に対して攻撃
            AEnemy target = null;
            var minDistance = float.MaxValue;
            foreach (var enemy in enemies)
            {
                var enemyPosition = enemy.CurrentPosition;

                var distance = TilePosition.GetDistance(enemyPosition, SetPosition);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = enemy;
                }
            }

            if (target == null) return;

            // 発射物系なのでディレイをかける
            var delay = General.DelayCoroutineByGameTime(
                SceneController,
                ObjectDuration,
                () =>
                {
                    // 敵にダメージを与える
                    target.Damage(GetDamage());

                    var targetPosition = target.CurrentPosition;
                    if (targetPosition == null) return;
                    
                    // 燃焼床のダメージもバフする
                    var igniteDamage = IgniteDamage + AmpDamage;

                    // 燃焼床生成
                    MazeController.IgniteFloor(targetPosition, igniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetUp(), igniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetDown(), igniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetLeft(), igniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetRight(), igniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetLeftUp(), igniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetLeftDown(), igniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetRightUp(), igniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetRightDown(), igniteDamage, IgniteDuration);
                });
            
            // 敵との角度を計算
            var targetPosition = target.CurrentPosition;
            if (targetPosition == null) return;
            var angle = SetPosition.GetAngle(targetPosition);
            // 上が0度なので90度ずらす
            angle -= 90;
            
            // 砲塔を回転
            turretRotator.Rotate(-angle);
            
            // 回転を待つ
            var rotateDelay = General.DelayCoroutine(0.5f, () =>
            {
                // 発射アニメーション
                animController.Fire();
                
                StartCoroutine(delay);
            });
            
            StartCoroutine(rotateDelay);
        }

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

        protected override void AsleepTurret()
        {
        }

        protected override int GetDuration()
        {
            return 0;
        }

        public override int GetDefaultDamage()
        {
            return Damage;
        }
    }
}