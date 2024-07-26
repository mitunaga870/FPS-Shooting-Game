using AClass;
using Traps;
using UnityEngine;

namespace DataClass
{
    public class TrapData
    {
        public readonly int Column;
        public readonly int Row;
        public readonly string Trap;

        public TrapData(int row, int column, ATrap trap)
        {
            Row = row;
            Column = column;
            Trap = trap.GetTrapName();
        }

        public TrapData(string trapData)
        {
            var data = trapData.Split("%%");

            Row = int.Parse(data[0]);
            Column = int.Parse(data[1]);
            Trap = data[2];
        }

        public override string ToString()
        {
            return $"{Row}%%{Column}%%{Trap}";
        }
    }
}