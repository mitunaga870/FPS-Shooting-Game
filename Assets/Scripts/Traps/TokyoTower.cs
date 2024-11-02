using AClass;
using DataClass;
using UnityEngine;

namespace Traps
{
    public class TokyoTower : ATrap
    {
        private const int SetRange = 2;
        private const int Damage = 10;
        private const int KnockBack = 5;
        private const int Height = 1;
        private const int CoolDown = 500;
        private const string TrapName = "Tokyo Tower";

        public override void AwakeTrap(TilePosition position)
        {
            // エネミーコントローラーがない場合は何もしない
            if (enemyController == null) return;

            // クールダウン中は何もしない
            if (ChargeTime > 0) return;

            // クールダウンをセット
            ChargeTime = CoolDown;

            enemyController.DamageEnemy(position, Damage);
            enemyController.KnockBackEnemy(position, KnockBack);
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