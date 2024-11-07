using AClass;
using DataClass;
using UnityEngine;

namespace Traps
{
    public class LandMine : ATrap
    {
        private const string TrapName = "LandMine";
        
        private int Damage => trapObject.LandMineDamage;
        private float Height => trapObject.LandMineHeight;
        private int CoolDown => trapObject.LandMineCoolDown;
        private int SetRange => trapObject.LandMineSetRange;
        
        [SerializeField]
        TrapMine_IgnitionAction trapMineIgnitionAction;

        public override void AwakeTrap(TilePosition position)
        {
            if (EnemyController == null) return;
            
            // クールダウン中は処理しない
            if (ChargeTime > 0) return;

            // クールダウン設定
            ChargeTime = CoolDown;

            // アニメーション発火
            trapMineIgnitionAction.StartAction();

            var damage = GetDamage();

            // 周囲１マスの敵にダメージ
            EnemyController.DamageEnemy(position, damage);
            EnemyController.DamageEnemy(position.GetUp(), damage);
            EnemyController.DamageEnemy(position.GetDown(), damage);
            EnemyController.DamageEnemy(position.GetLeft(), damage);
            EnemyController.DamageEnemy(position.GetRight(), damage);
            EnemyController.DamageEnemy(position.GetRightUp(), damage);
            EnemyController.DamageEnemy(position.GetRightDown(), damage);
            EnemyController.DamageEnemy(position.GetLeftUp(), damage);
            EnemyController.DamageEnemy(position.GetLeftDown(), damage);
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