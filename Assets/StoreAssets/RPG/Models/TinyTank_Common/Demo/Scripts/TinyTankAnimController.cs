using UnityEngine;

namespace Pallab.TinyTank
{
    public class TinyTankAnimController : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] Animator trackAnimatorL;
        [SerializeField] Animator trackAnimatorR;
        [SerializeField] float trackAnimSpeed = 1.0f;
        [SerializeField] float animationDamping = 10.0f;

        Vector3 movementSource;
        Vector3 movement;
        static readonly int speed = Animator.StringToHash("Speed");
        static readonly int turn = Animator.StringToHash("Turn");
        static readonly int fire = Animator.StringToHash("Fire");
        static readonly int isFlying = Animator.StringToHash("IsFlying");

        public void SetMovement(float forward, float rotate) => movementSource.Set(rotate, 0.0f, forward);

        public void Fire() => animator.SetTrigger(fire);

        public void SetIsFlying(bool isFlyingVal) => animator.SetBool(isFlying, isFlyingVal);

        void Update()
        {
            movement = Vector3.MoveTowards(movement, movementSource, Time.deltaTime * animationDamping);

            animator.SetFloat(speed, movement.z);
            animator.SetFloat(turn, movement.x);

            switch (movement.x)
            {
                case > 0 when movement.z > 0:
                    trackAnimatorL.SetFloat(speed, trackAnimSpeed);
                    trackAnimatorR.SetFloat(speed, trackAnimSpeed * 0.5f);
                    break;
                case > 0 when movement.z < 0:
                    trackAnimatorL.SetFloat(speed, -trackAnimSpeed * 0.5f);
                    trackAnimatorR.SetFloat(speed, -trackAnimSpeed);
                    break;
                case > 0:
                    trackAnimatorL.SetFloat(speed, trackAnimSpeed);
                    trackAnimatorR.SetFloat(speed, -trackAnimSpeed);
                    break;
                case < 0 when movement.z > 0:
                    trackAnimatorL.SetFloat(speed, trackAnimSpeed * 0.5f);
                    trackAnimatorR.SetFloat(speed, trackAnimSpeed);
                    break;
                case < 0 when movement.z < 0:
                    trackAnimatorL.SetFloat(speed, -trackAnimSpeed);
                    trackAnimatorR.SetFloat(speed, -trackAnimSpeed * 0.5f);
                    break;
                case < 0:
                    trackAnimatorL.SetFloat(speed, -trackAnimSpeed);
                    trackAnimatorR.SetFloat(speed, trackAnimSpeed);
                    break;
                default:
                    trackAnimatorL.SetFloat(speed, movement.z * trackAnimSpeed);
                    trackAnimatorR.SetFloat(speed, movement.z * trackAnimSpeed);
                    break;
            }
        }
    }
}
