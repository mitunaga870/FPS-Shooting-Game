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

            if (EnemyController == null) return;

            var damage = GetDamage();

            // 周囲１マスの敵を跳ね飛ばす
            EnemyController.JumpEnemy(position, JumpHeight, damage);
            EnemyController.JumpEnemy(position.GetUp(), JumpHeight, damage);
            EnemyController.JumpEnemy(position.GetDown(), JumpHeight, damage);
            EnemyController.JumpEnemy(position.GetLeft(), JumpHeight, damage);
            EnemyController.JumpEnemy(position.GetRight(), JumpHeight, damage);
            EnemyController.JumpEnemy(position.GetRightUp(), JumpHeight, damage);
            EnemyController.JumpEnemy(position.GetRightDown(), JumpHeight, damage);
            EnemyController.JumpEnemy(position.GetLeftUp(), JumpHeight, damage);
            EnemyController.JumpEnemy(position.GetLeftDown(), JumpHeight, damage);
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

        public override int GetDefaultDamage()
        {
            return Damage;
        }
    }
}