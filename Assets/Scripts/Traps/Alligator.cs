using AClass;
using DataClass;
using Ignition_action;
using UnityEngine;

namespace Traps
{
    public class Alligator : ATrap
    {
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip awakeSound;
        
        private int Damage => trapObject.AlligatorDamage;
        private float Height => trapObject.AlligatorHeight;
        private int CoolDown => trapObject.AlligatorCoolDown;
        private int KnockBack => trapObject.AlligatorKnockBack;
        private int SetRange => trapObject.AlligatorSetRange;
        
        [SerializeField]
        private CrocTrap_ActionIgnition crocTrapActionIgnition;

        public override void AwakeTrap(TilePosition position)
        {
            // クールダウン中は処理しない
            if (ChargeTime > 0) return;

            // クールダウン設定
            ChargeTime = CoolDown;

            if (EnemyController == null) return;
            
            // 効果音再生
            audioSource.PlayOneShot(awakeSound);
            
            // アニメーション再生
            crocTrapActionIgnition.IgnitionAction();
            
            // ダメージ取得
            var damage = GetDamage();

            // 周囲1マスの敵にダメージ
            EnemyController.DamageEnemy(position, damage);
            EnemyController.DamageEnemy(position.GetUp(), damage);
            EnemyController.DamageEnemy(position.GetDown(), damage);
            EnemyController.DamageEnemy(position.GetLeft(), damage);
            EnemyController.DamageEnemy(position.GetRight(), damage);
            EnemyController.DamageEnemy(position.GetRightUp(), damage);
            EnemyController.DamageEnemy(position.GetRightDown(), damage);
            EnemyController.DamageEnemy(position.GetLeftUp(), damage);
            EnemyController.DamageEnemy(position.GetLeftDown(), damage);

            // 周囲1マスの敵を跳ね飛ばす
            EnemyController.KnockBackEnemy(position, KnockBack);
            EnemyController.KnockBackEnemy(position.GetUp(), KnockBack);
            EnemyController.KnockBackEnemy(position.GetDown(), KnockBack);
            EnemyController.KnockBackEnemy(position.GetLeft(), KnockBack);
            EnemyController.KnockBackEnemy(position.GetRight(), KnockBack);
            EnemyController.KnockBackEnemy(position.GetRightUp(), KnockBack);
            EnemyController.KnockBackEnemy(position.GetRightDown(), KnockBack);
            EnemyController.KnockBackEnemy(position.GetLeftUp(), KnockBack);
            EnemyController.KnockBackEnemy(position.GetLeftDown(), KnockBack);
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
            return 180;
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