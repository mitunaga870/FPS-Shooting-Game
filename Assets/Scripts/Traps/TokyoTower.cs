using AClass;
using DataClass;

namespace Traps
{
    public class TokyoTower : ATrap
    {
        private const int SetRange = 2;
        private const int Damage = 10;
        private const int Height = 1;
        private const string TrapName = "Tokyo Tower";

        public override void AwakeTrap(TilePosition position)
        {
            throw new System.NotImplementedException();
        }

        public override float GetHeight()
        {
            return Height;
        }

        public override int GetSetRange()
        {
            return SetRange;
        }

        public override string GetTrapName()
        {
            return TrapName;
        }
    }
}