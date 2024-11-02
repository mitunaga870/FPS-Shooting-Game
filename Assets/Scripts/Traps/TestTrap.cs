using AClass;
using DataClass;
using UnityEngine;

namespace Traps
{
    public class TestTrap : ATrap
    {
        private const string TrapName = "TestTrap";
        private const float Height = 0.5f;
        private const int SetRange = 1;

        public override void AwakeTrap(TilePosition position)
        {
            // とりあえず色を変える
            GetComponent<Renderer>().material.color = Color.red;

            // テスト動作として踏んだ敵に999ダメージを与える
            enemyController.DamageEnemy(position, 999);
        }

        public override float GetHeight()
        {
            return trapObject.TestTrapHeight;
        }

        public override int GetSetRange()
        {
            return SetRange;
        }

        public override string GetTrapName()
        {
            return TrapName;
        }

        public override int GetTrapAngle()
        {
            return 0;
        }

        public override void SetAngle(int trapAngle)
        {
        }
    }
}