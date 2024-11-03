using DG.Tweening;
using UnityEngine;

namespace Pallab.TinyTank
{
    public class TinyTankTurretRotator : MonoBehaviour
    {
        [SerializeField] Transform turretPivotBone;

        public void Rotate(float angle)
        {
            turretPivotBone.DOLocalRotate(new Vector3(0, angle, 0), 0.5f)
                .SetEase(Ease.InQuart)
                .SetRelative(false);
        }
    }
}
