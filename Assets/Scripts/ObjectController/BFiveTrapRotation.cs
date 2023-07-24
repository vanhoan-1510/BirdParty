using UnityEngine;

public class BFiveTrapRotation : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float maxRotation = 80f;

    [SerializeField] private float currentRotation;
    private bool rotateClockwise = true;

    void Update()
    {
        float rotationDirection = rotateClockwise ? 1f : -1f;

        float newRotation = currentRotation + rotationSpeed * Time.deltaTime * rotationDirection;

        newRotation = Mathf.Clamp(newRotation, -maxRotation, maxRotation);

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newRotation);

        currentRotation = newRotation;

        
        if (currentRotation >= maxRotation || currentRotation <= -maxRotation)
        {
            rotateClockwise = !rotateClockwise;
        }
    }
}
