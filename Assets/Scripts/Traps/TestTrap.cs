using AClass;
using UnityEngine;

namespace Traps
{
    public class TestTrap : ATrap
    {
        public static readonly string TrapName = "TestTrap";

        public override void AwakeTrap()
        {
            Debug.Log("TestTrapが発火しました");
            // とりあえず色を変える
            GetComponent<Renderer>().material.color = Color.red;
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