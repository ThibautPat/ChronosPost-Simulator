using UnityEngine;

public class PlayerPhysicsDriver : MonoBehaviour
{
    [Header("Setup")]
    [Range(0,8)]
    [SerializeField] float force = 3.81f;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;
        if (rigidbody != null && hit.collider.CompareTag("Pushable"))
        {
            Vector3 direction = hit.transform.position - transform.position;
            direction.y = 0; direction.Normalize();

            rigidbody.AddForceAtPosition(direction * force, transform.position, ForceMode.Impulse);
        }
    }
}