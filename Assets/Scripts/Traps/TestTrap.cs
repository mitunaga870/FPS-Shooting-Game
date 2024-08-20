using AClass;
using UnityEngine;

namespace Traps
{
    public class TestTrap : ATrap
    {
        public static readonly string TrapName = "TestTrap";

        public override void Awake()
        {
            Debug.Log("Damage: " + trapData.TestTrapAtk);
        }

        public override float GetHeight()
        {
            return trapData.TestTrapHeight;
        }

        public override string GetTrapName()
        {
            return TrapName;
        }
    }
}