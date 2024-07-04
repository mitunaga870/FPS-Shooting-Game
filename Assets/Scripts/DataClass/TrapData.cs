using Traps;

namespace DataClass
{
    public class TrapData
    {
        public readonly int Column;
        public readonly int Row;
        public readonly ATrap Trap;

        public TrapData(int row, int column, ATrap trap)
        {
            Row = row;
            Column = column;
            Trap = trap;
        }

        public void Dispose()
        {
            Trap.Destroy();
        }
    }
}