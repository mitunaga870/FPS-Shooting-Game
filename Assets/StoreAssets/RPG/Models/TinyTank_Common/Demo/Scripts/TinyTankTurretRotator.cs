using UnityEngine;

namespace Pallab.TinyTank
{
    public class TinyTankTurretRotator : MonoBehaviour
    {
        [SerializeField] Transform turretPivotBone;

        public void Rotate(float angle)
        {
            turretPivotBone.localRotation *= Quaternion.Euler(0.0f, angle, 0.0f);
        }
    }
}
