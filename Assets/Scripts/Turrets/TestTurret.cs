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
            return new List<TilePosition>() { new(0, 1) };
        }
    }
}