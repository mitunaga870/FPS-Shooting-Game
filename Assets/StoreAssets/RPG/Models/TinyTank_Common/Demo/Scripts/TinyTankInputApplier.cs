using UnityEngine;

namespace Pallab.TinyTank
{
    [RequireComponent(typeof(TinyTankMover), typeof(TinyTankAnimController), typeof(TinyTankTurretRotator))]
    public class TinyTankInputApplier : MonoBehaviour
    {
        [SerializeField] TinyTankMover mover;
        [SerializeField] TinyTankAnimController animController;
        [SerializeField] TinyTankTurretRotator turretRotator;
        [SerializeField] bool disableFire; // 後方互換性のため否定形bool

        void Update()
        {
            var moveHorizontal = Input.GetAxis("Horizontal");
            var moveVertical = Input.GetAxis("Vertical");

            mover.SetMovement(moveVertical, moveHorizontal);
            animController.SetMovement(moveVertical, moveHorizontal);

            if (Input.GetKeyDown(KeyCode.Space) && !disableFire)
            {
                animController.Fire();
            }

            if (Input.GetKey(KeyCode.Q))
            {
                turretRotator.Rotate(-90.0f * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                turretRotator.Rotate(90.0f * Time.deltaTime);
            }
        }
    }
}
