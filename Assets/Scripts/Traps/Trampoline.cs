using AClass;
using DataClass;
using Ignition_action;
using UnityEngine;

namespace Traps
{
    public class Trampoline : ATrap
    {
        private const float Height = 0.04f;
        private const int JumpHeight = 10;
        private const int Damage = 10;
        private const int CoolDown = 100;
        private const string TrapName = "Trampoline";
        private const int SetRange = 1;
        
        public override void AwakeTrap(TilePosition position)
        {
            // クールダウン中は処理しない
            if (ChargeTime > 0) return;

            // クールダウン設定
            ChargeTime = CoolDown;

            if (enemyController == null) return;

            // 周囲１マスの敵を跳ね飛ばす
            enemyController.JumpEnemy(position, JumpHeight, Damage);
            enemyController.JumpEnemy(position.GetUp(), JumpHeight, Damage);
            enemyController.JumpEnemy(position.GetDown(), JumpHeight, Damage);
            enemyController.JumpEnemy(position.GetLeft(), JumpHeight, Damage);
            enemyController.JumpEnemy(position.GetRight(), JumpHeight, Damage);
            enemyController.JumpEnemy(position.GetRightUp(), JumpHeight, Damage);
            enemyController.JumpEnemy(position.GetRightDown(), JumpHeight, Damage);
            enemyController.JumpEnemy(position.GetLeftUp(), JumpHeight, Damage);
            enemyController.JumpEnemy(position.GetLeftDown(), JumpHeight, Damage);
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

        public override int GetTrapAngle()
        {
            return 0;
        }

        public override void SetAngle(int trapAngle)
        {
        }
    }
}