using UnityEngine;

public class BounceOnCollision : MonoBehaviour
{
    public float bounceForce = 5f;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody otherRigidbody = hit.collider.attachedRigidbody;

        if (otherRigidbody != null)
        {
            // Áp dụng lực nảy lên vào đối tượng va chạm
            Vector3 bounceDirection = hit.normal;
            otherRigidbody.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
        }
    }
}
