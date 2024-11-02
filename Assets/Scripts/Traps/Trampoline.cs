using AClass;
using DataClass;

namespace Traps
{
    public class Trampoline : ATrap
    {
        private const int Height = 1;
        private readonly int Damage = 10;
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
            enemyController.JumpEnemy(position, Height, Damage);
            enemyController.JumpEnemy(position.GetUp(), Height, Damage);
            enemyController.JumpEnemy(position.GetDown(), Height, Damage);
            enemyController.JumpEnemy(position.GetLeft(), Height, Damage);
            enemyController.JumpEnemy(position.GetRight(), Height, Damage);
            enemyController.JumpEnemy(position.GetRightUp(), Height, Damage);
            enemyController.JumpEnemy(position.GetRightDown(), Height, Damage);
            enemyController.JumpEnemy(position.GetLeftUp(), Height, Damage);
            enemyController.JumpEnemy(position.GetLeftDown(), Height, Damage);
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
    }
}