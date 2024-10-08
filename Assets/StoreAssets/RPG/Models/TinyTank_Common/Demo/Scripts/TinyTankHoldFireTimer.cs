using UnityEngine;
using UnityEngine.Events;

namespace Pallab.TinyTank
{
    public class TinyTankHoldFireTimer : MonoBehaviour
    {
        [SerializeField] int count = 4;
        [SerializeField] float cooltime = 0.1f;

        bool isHolding;
        int currentIndex;
        float currentCooltime;

        public event UnityAction<int> OnFire;

        public void StartHold() => isHolding = true;

        public void StopHold()
        {
            isHolding = false;
            currentIndex = 0;
        }

        void Update()
        {
            if (currentCooltime > 0.0f)
            {
                currentCooltime -= Time.deltaTime;
                return;
            }

            if (!isHolding)
            {
                return;
            }

            OnFire?.Invoke(currentIndex);
            currentIndex = (currentIndex + 1) % count;
            currentCooltime = cooltime;
        }
    }
}
