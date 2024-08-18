using AClass;
using UnityEngine;

namespace Traps
{
    public class TestTrap : ATrap
    {
        public static readonly string TrapName = "TestTrap";

        public override void Awake()
        {
        }

        public override float GetHeight()
        {
            return trapObject.TestTrapHeight;
        }

        public override string GetTrapName()
        {
            return TrapName;
        }
    }
}