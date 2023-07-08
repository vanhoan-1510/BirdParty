using UnityEngine;

public class BounceOnCollision : MonoBehaviour
{
    [SerializeField] float bounceForce = 5f; // Lực nảy
    [SerializeField] LayerMask bounceLayer; // LayerMask của vật thể nảy
    [SerializeField] float bounceAngle = 45f; // Góc nảy

    private void OnCollisionEnter(Collision collision)
    {
        if (bounceLayer == (bounceLayer | (1 << collision.gameObject.layer))) // Kiểm tra xem vật thể va chạm có layer phù hợp hay không
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            // Tính toán hướng nảy dựa trên góc nảy và hướng của nhân vật
            Vector3 bounceDirection = Quaternion.Euler(0f, -bounceAngle, 0f) * transform.forward;

            rb.AddForce(bounceDirection * bounceForce, ForceMode.VelocityChange);
        }
    }
}
