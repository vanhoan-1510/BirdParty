using UnityEngine;

public class TriggerToPlay : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float maxRotation = 80f;

    [SerializeField] private float currentRotation;
    private bool rotateClockwise = true;
    private GameObject playerObject;

    private void Start()
    {
        playerObject = GameManager.Instance.playerObject;
    }

    private void Update()
    {
         ObjectRotation();
    }
    private void ObjectRotation()
    {
        float rotationDirection = rotateClockwise ? 1f : -1f;

        float newRotation = currentRotation + rotationSpeed * Time.deltaTime * rotationDirection;

        newRotation = Mathf.Clamp(newRotation, -maxRotation, maxRotation);

        transform.rotation = Quaternion.Euler(newRotation, transform.rotation.y, transform.rotation.z);

        currentRotation = newRotation;


        if (currentRotation >= maxRotation || currentRotation <= -maxRotation)
        {
            rotateClockwise = !rotateClockwise;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerObject)
        {
            Vector3 direction = GetComponent<Rigidbody>().velocity.normalized;

            float bounceForce = 5f;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(direction * bounceForce, ForceMode.Impulse);
        }
    }
}
