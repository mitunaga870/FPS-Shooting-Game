using AClass;

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
    }
}