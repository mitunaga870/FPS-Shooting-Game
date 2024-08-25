using UnityEngine;

namespace Pallab.TinyTank
{
    [RequireComponent(typeof(TinyTankHoldFireTimer))]
    public class TinyTankHoldInputApplier : MonoBehaviour
    {
        [SerializeField] TinyTankHoldFireTimer holdFireTimer;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                holdFireTimer.StartHold();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                holdFireTimer.StopHold();
            }
        }
    }
}
