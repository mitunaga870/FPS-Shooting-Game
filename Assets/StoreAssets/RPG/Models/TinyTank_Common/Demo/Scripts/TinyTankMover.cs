using UnityEngine;

namespace Pallab.TinyTank
{
    [RequireComponent(typeof(Rigidbody))]
    public class TinyTankMover : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;
        [SerializeField] float moveSpeed = 5.0f;
        [SerializeField] float rotateSpeed = 90.0f;

        Vector3 movement;

        public void SetMovement(float forward, float turn)
        {
            movement.Set(turn, 0.0f, forward);
        }

        void FixedUpdate()
        {
            var velocity = rb.velocity;
            var currentMovement = movement.z * transform.forward * moveSpeed;
            velocity.x = currentMovement.x;
            velocity.z = currentMovement.z;

            rb.velocity = velocity;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0.0f, movement.x * rotateSpeed * Time.fixedDeltaTime, 0.0f));
        }
    }
}
