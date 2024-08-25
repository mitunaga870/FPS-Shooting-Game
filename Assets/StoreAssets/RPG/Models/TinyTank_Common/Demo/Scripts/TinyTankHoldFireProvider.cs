using UnityEngine;

namespace Pallab.TinyTank
{
    [RequireComponent(typeof(TinyTankHoldFireTimer))]
    public class TinyTankHoldFireProvider : MonoBehaviour
    {
        [SerializeField] TinyTankHoldFireTimer holdFireTimer;
        [SerializeField] Animator animator;

        void OnEnable() => holdFireTimer.OnFire += OnFire;

        void OnDisable() => holdFireTimer.OnFire -= OnFire;

        void OnFire(int index) => animator.SetTrigger($"Fire{index}");
    }
}
