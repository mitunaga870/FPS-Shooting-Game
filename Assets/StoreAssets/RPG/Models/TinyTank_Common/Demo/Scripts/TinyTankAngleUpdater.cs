using UnityEngine;

namespace Pallab.TinyTank
{
    [RequireComponent(typeof(TinyTankAnimController))]
    public class TinyTankAngleUpdater : MonoBehaviour
    {
        [SerializeField] Transform root;
        [SerializeField] TinyTankAnimController animController;
        [SerializeField] float speed = 10f;

        void Update()
        {
            var targetRotate = TargetRotate(out var isHit);
            animController.SetIsFlying(!isHit);
            root.rotation = Quaternion.Lerp(root.rotation, targetRotate, Time.deltaTime * speed);
        }

        Quaternion TargetRotate(out bool isHit)
        {
            isHit = Physics.Linecast(transform.position + Vector3.up * 0.05f, transform.position + Vector3.down * 0.2f, out var hit);
            if (!isHit)
            {
                return transform.rotation;
            }

            var forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            var rotation = Quaternion.LookRotation(forward, hit.normal);
            return rotation;
        }
    }
}
