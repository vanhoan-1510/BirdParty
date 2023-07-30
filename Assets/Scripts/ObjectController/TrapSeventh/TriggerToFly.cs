using System.Collections;
using UnityEngine;

public class TriggerToFly : MonoBehaviour
{
    public Transform objectA;
    public float moveSpeed = 5f;
    public float flightHeight = 2f;

    private bool isMoving = false;
    private Vector3 targetPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMoving)
        {
            targetPosition = objectA.position;
            StartCoroutine(MoveToTargetPosition(other.transform));
            WaitToRespawn();
        }
    }

    private IEnumerator MoveToTargetPosition(Transform playerTransform)
    {
        isMoving = true;
        Vector3 startPosition = playerTransform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float journeyTime = distance / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < journeyTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / journeyTime);

            float heightOffset = Mathf.Sin(t * Mathf.PI) * flightHeight;

            playerTransform.position = Vector3.Lerp(startPosition, targetPosition, t) + Vector3.up * heightOffset;
            yield return null;
        }

        playerTransform.position = targetPosition;
        isMoving = false;
    }
    
    private IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(5f);
        PlayerController.Instance.LoadCheckPoint();
    }
}
