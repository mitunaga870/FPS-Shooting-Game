using UnityEngine;

namespace Traps
{
    public class TestTrap : ATrap
    {
        public override void Awake()
        {
            Debug.Log("Damage: " + trapData.TestTrapAtk);
        }
    }
}