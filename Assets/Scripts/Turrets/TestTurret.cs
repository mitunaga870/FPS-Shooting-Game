using System.Collections.Generic;
using AClass;
using DataClass;
using UnityEngine;

namespace Turrets
{
    public class TestTurret : ATurret
    {
        private const string TurretName = "TestTurret";
        private const int Interval = 10;
        private const float Height = 0.5f;

        public override float GetHeight()
        {
            return Height;
        }

        protected override void AwakeTurret(List<AEnemy> enemies)
        {
            foreach (var enemy in enemies) enemy.Damage(10);
        }

        public override List<TilePosition> GetEffectArea()
        {
            var area = new List<TilePosition>()
            {
                new(0, 1), new(0, 2), new(0, 3), new(0, 4), new(0, 5)
            };
            var result = new List<TilePosition>();

            foreach (var position in area) result.Add(position.Rotate(Angle));

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
            Angle = angle;
        }
    }
}