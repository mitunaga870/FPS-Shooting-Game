using System.Collections.Generic;
using AClass;
using DataClass;
using lib;

namespace Turrets
{
    public class RPG : ATurret
    {
        private const int Height = 1;
        private const int Damage = 0;
        private const int ObjectDuration = 10;
        private const int IgniteDamage = 10;
        private const int IgniteDuration = 100;
        private const int Interval = 500;
        private const string TurretName = "RPG";

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
                sceneController,
                ObjectDuration,
                () =>
                {
                    // 敵にダメージを与える
                    target.Damage(Damage);

                    var targetPosition = target.CurrentPosition;
                    if (targetPosition == null) return;

                    // 燃焼床生成
                    MazeController.IgniteFloor(targetPosition, IgniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetUp(), IgniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetDown(), IgniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetLeft(), IgniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetRight(), IgniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetLeftUp(), IgniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetLeftDown(), IgniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetRightUp(), IgniteDamage, IgniteDuration);
                    MazeController.IgniteFloor(targetPosition.GetRightDown(), IgniteDamage, IgniteDuration);
                });
            StartCoroutine(delay);
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
    }
}