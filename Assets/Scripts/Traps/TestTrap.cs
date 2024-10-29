using AClass;
using DataClass;
using UnityEngine;

namespace Traps
{
    public class TestTrap : ATrap
    {
        public static readonly string TrapName = "TestTrap";

        public override void AwakeTrap(TilePosition position)
        {
            Debug.Log("TestTrapが発火しました");

            // とりあえず色を変える
            GetComponent<Renderer>().material.color = Color.red;

            // テスト動作として踏んだ敵に999ダメージを与える
            enemyController.DamageEnemy(position, 999);
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