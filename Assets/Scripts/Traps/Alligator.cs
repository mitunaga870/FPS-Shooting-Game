using AClass;
using DataClass;

namespace Traps
{
    public class Alligator : ATrap
    {
        private const int Damage = 10;
        private const int Height = 1;
        private const int CoolDown = 100;
        private const int KnockBack = 3;
        private const int SetRange = 1;

        public override void AwakeTrap(TilePosition position)
        {
            // クールダウン中は処理しない
            if (ChargeTime > 0) return;

            // クールダウン設定
            ChargeTime = CoolDown;

            if (enemyController == null) return;

            // 周囲1マスの敵にダメージ
            enemyController.DamageEnemy(position, Damage);
            enemyController.DamageEnemy(position.GetUp(), Damage);
            enemyController.DamageEnemy(position.GetDown(), Damage);
            enemyController.DamageEnemy(position.GetLeft(), Damage);
            enemyController.DamageEnemy(position.GetRight(), Damage);
            enemyController.DamageEnemy(position.GetRightUp(), Damage);
            enemyController.DamageEnemy(position.GetRightDown(), Damage);
            enemyController.DamageEnemy(position.GetLeftUp(), Damage);
            enemyController.DamageEnemy(position.GetLeftDown(), Damage);

            // 周囲1マスの敵を跳ね飛ばす
            enemyController.KnockBackEnemy(position, KnockBack);
            enemyController.KnockBackEnemy(position.GetUp(), KnockBack);
            enemyController.KnockBackEnemy(position.GetDown(), KnockBack);
            enemyController.KnockBackEnemy(position.GetLeft(), KnockBack);
            enemyController.KnockBackEnemy(position.GetRight(), KnockBack);
            enemyController.KnockBackEnemy(position.GetRightUp(), KnockBack);
            enemyController.KnockBackEnemy(position.GetRightDown(), KnockBack);
            enemyController.KnockBackEnemy(position.GetLeftUp(), KnockBack);
            enemyController.KnockBackEnemy(position.GetLeftDown(), KnockBack);
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
            return "Alligator";
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