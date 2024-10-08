using UnityEngine;

namespace Pallab.TinyTank
{
    public class TinyTankCameraConstraint : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float speed = 2f;

        Vector3 offset;

        void Start()
        {
            offset = transform.position - target.position;
        }

        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
        }
    }
}
