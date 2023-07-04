using UnityEngine;

public class Bouncing : MonoBehaviour
{
    public LayerMask bouncingLayer;
    public float bounceForce = 10f;
    public float initialBounceSpeed = 5f;
    public float bounceSpeedReduction = 0.5f;

    private float currentBounceSpeed;

    private void Start()
    {
        currentBounceSpeed = initialBounceSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & bouncingLayer) != 0)
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 bounceDirection = (transform.position - collision.contacts[0].point).normalized;

                rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
                rb.velocity = rb.velocity.normalized * currentBounceSpeed;

                currentBounceSpeed -= bounceSpeedReduction;

                if (currentBounceSpeed < 0f)
                {
                    currentBounceSpeed = 0f;
                }
            }
        }
    }
}
