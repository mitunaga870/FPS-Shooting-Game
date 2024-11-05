using AClass;
using DataClass;
using DG.Tweening;
using lib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Traps
{
    public class TokyoTower : ATrap
    {
        private const int SetRange = 2;
        private const int Damage = 1;
        private const int KnockBack = 5;
        private const float CreateHeight = 0.15f;
        private const float InvadeHeight = -2.7f;
        private const float MovedHeight = 0.15f;
        private const int CoolDown = 500;
        private const string TrapName = "Tokyo Tower";

        public override void AwakeTrap(TilePosition position)
        {
            // エネミーコントローラーがない場合は何もしない
            if (EnemyController == null) return;

            // クールダウン中は何もしない
            if (ChargeTime > 0) return;

            // クールダウンをセット
            ChargeTime = CoolDown;
            
            // アニメーションを再生
            IgnitionAction();

            var damage = GetDamage();

            EnemyController.DamageEnemy(position, damage);
            EnemyController.KnockBackEnemy(position, KnockBack);
        }

        private void IgnitionAction()
        {
            var prevPos = transform.position;
            
            // 突き上げる
            transform.DOMove(new Vector3(0, MovedHeight - InvadeHeight, 0), 0.5f).SetRelative(true).SetEase(Ease.InOutQuart)
                .onComplete = () =>
                {
                    // 1秒後に元の位置に戻す
                    var delay = General.DelayCoroutine(
                        1f,
                        () =>
                        {
                            transform.DOMove(prevPos, 2f).SetRelative(false).SetEase(Ease.InOutQuart);
                        }
                    );
                    
                    StartCoroutine(delay);
                };
        }

        public override float GetHeight()
        {
            var isInvade = SceneManager.GetActiveScene().name == "InvasionPhase";
            
            if (isInvade)
            {
                return InvadeHeight;
            }
            
            return CreateHeight;
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