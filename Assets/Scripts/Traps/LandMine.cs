using AClass;
using DataClass;
using UnityEngine;

namespace Traps
{
    public class LandMine : ATrap
    {
        private const int Damage = 10;
        private const float Height = 0.05f;
        private const int CoolDown = 100;
        private const int SetRange = 1;
        private const string TrapName = "LandMine";
        
        [SerializeField]
        TrapMine_IgnitionAction trapMineIgnitionAction;

        public override void AwakeTrap(TilePosition position)
        {
            if (enemyController == null) return;
            
            // クールダウン中は処理しない
            if (ChargeTime > 0) return;

            // クールダウン設定
            ChargeTime = CoolDown;

            // アニメーション発火
            trapMineIgnitionAction.StartAction();

            // 周囲１マスの敵にダメージ
            enemyController.DamageEnemy(position, Damage);
            enemyController.DamageEnemy(position.GetUp(), Damage);
            enemyController.DamageEnemy(position.GetDown(), Damage);
            enemyController.DamageEnemy(position.GetLeft(), Damage);
            enemyController.DamageEnemy(position.GetRight(), Damage);
            enemyController.DamageEnemy(position.GetRightUp(), Damage);
            enemyController.DamageEnemy(position.GetRightDown(), Damage);
            enemyController.DamageEnemy(position.GetLeftUp(), Damage);
            enemyController.DamageEnemy(position.GetLeftDown(), Damage);
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