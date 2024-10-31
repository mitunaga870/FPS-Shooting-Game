using System.Collections.Generic;
using AClass;
using DataClass;

namespace Turrets
{
    public class TestTurret : ATurret
    {
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
            var area = new List<TilePosition>() { new(0, 1) };
            var result = new List<TilePosition>();

            foreach (var position in area) result.Add(position.Rotate(angle));

            return result;
        }
    }
}