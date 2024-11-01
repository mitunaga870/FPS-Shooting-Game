using System.Collections.Generic;
using AClass;
using DataClass;

namespace Turrets
{
    public class TestTurret : ATurret
    {
        private const string TurretName = "TestTurret";

        public override float GetHeight()
        {
            return 0.5f;
        }

        public override void AwakeTurret()
        {
            throw new System.NotImplementedException();
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

        public override void SetAngle(int angle)
        {
            Angle = angle;
        }
    }
}