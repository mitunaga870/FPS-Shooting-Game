using System.Collections.Generic;
using AClass;
using DataClass;
using UnityEngine;

namespace Turrets
{
    public class BeansGatling : ATurret
    {
        private const string TurretName = "BeansGatling";
        
        [SerializeField]
        private AudioSource audioSource;
        
        [SerializeField]
        private AudioClip awakeSound;
        
        private int Damage => turretObject.BeansGatlingDamage;
        private float Height => turretObject.BeansGatlingHeight;
        private int Interval => turretObject.BeansGatlingInterval;
        
        [SerializeField]
        private GatlingGun gatlingGun;
        
        public override float GetHeight()
        {
            return Height;
        }

        protected override void AwakeTurret(List<AEnemy> enemies)
        {
            // 効果音再生
            audioSource.PlayOneShot(awakeSound);
            
            // 最も近い敵に対して攻撃
            AEnemy target = null;
            var minDistance = float.MaxValue;
            foreach (var enemy in enemies)
            {
                var enemyPosition = enemy.CurrentPosition;

                var distance = TilePosition.GetDistance(enemyPosition, SetPosition);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = enemy;
                }
            }

            if (target == null)
            {
                // アニメーションを止める
                gatlingGun.StopFiring();
                return;
            }
            
            // アニメーションを再生する
            gatlingGun.SetTarget(target);

            // 敵にダメージを与える
            target.Damage(GetDamage());
        }

        public override List<TilePosition> GetEffectArea()
        {
            return null;
        }

        public override string GetTurretName()
        {
            return TurretName;
        }

        public override int GetInterval()
        {
            return Interval;
        }

        public override void SetAngle(int angle)
        {
        }

        protected override void AsleepTurret()
        {
        }

        protected override int GetDuration()
        {
            return 0;
        }

        public override int GetDefaultDamage()
        {
            return Damage;
        }
    }
}