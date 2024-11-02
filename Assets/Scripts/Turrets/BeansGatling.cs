using System.Collections.Generic;
using AClass;
using DataClass;

namespace Turrets
{
    public class BeansGatling : ATurret
    {
        private const int Damage = 10;
        private const int Height = 1;
        private const int Interval = 1;
        private const string TurretName = "BeansGatling";

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

            // 敵にダメージを与える
            target.Damage(Damage);
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