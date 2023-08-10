using System.Collections;
using UnityEngine;

public class TrapFourRotate : MonoBehaviour
{
    [SerializeField] private int numberOfRotations = 3;
    [SerializeField] private float rotationSpeed = 180f;

    private bool isRotating = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isRotating)
        {
            //PlayerController.Instance.Falling();
            isRotating = true;
            StartCoroutine(RotateForNumberOfTimes());
        }
    }

    private IEnumerator RotateForNumberOfTimes()
    {
        float totalRotation = 360f * numberOfRotations;
        float currentRotation = 0f;

        while (currentRotation < totalRotation)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.back, rotationAmount);
            currentRotation += rotationAmount;

            yield return null;
        }

        isRotating = false;
    }
}
