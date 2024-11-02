using System.Collections.Generic;
using AClass;
using DataClass;

namespace Turrets
{
    public class Fan : ATurret
    {
        private const int Height = 1;
        private const float SlowPercentage = 0.8f;
        private const int Duration = 1;
        private const int Interval = 1;
        private const string TurretName = "Fan";

        private int _angle;

        public override float GetHeight()
        {
            return Height;
        }

        protected override void AwakeTurret(List<AEnemy> enemies)
        {
            foreach (var enemy in enemies) enemy.Slow(SlowPercentage, Duration);
        }

        public override List<TilePosition> GetEffectArea()
        {
            var BaseList = new List<TilePosition>()
            {
                new(0, 1),
                new(0, 2),
                new(0, 3),
                new(0, 4)
            };

            var result = new List<TilePosition>();

            // 角度によってエフェクトエリアを変更
            foreach (var position in BaseList) result.Add(position.Rotate(_angle));

            return result;
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
            _angle = angle;
        }
    }
}