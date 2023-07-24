using System.Collections;
using UnityEngine;

public class TrapFourRotate : MonoBehaviour
{
    private PlayerController player;
    private GameObject playerObject;

    [SerializeField] private int numberOfRotations = 3;
    [SerializeField] private float rotationSpeed = 90f;



    private void Start()
    {
        playerObject = GameManager.Instance.playerObject;
        player = GameManager.Instance.player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == playerObject)
        {
            //PlayerController.Instance.Falling();
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
    }
}
