using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallMovementL : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float torqueForce = 2f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        Vector3 torque = new Vector3(0f, 0f, torqueForce);
        rb.AddTorque(-torque);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            torqueForce = torqueForce * -1;
        }
    }
}