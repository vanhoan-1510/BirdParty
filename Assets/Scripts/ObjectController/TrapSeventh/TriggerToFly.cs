using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TriggerToFly : MonoBehaviourPunCallbacks
{
    public Transform objectA;
    public float moveSpeed = 5f;
    public float flightHeight = 2f;

    private bool isMoving = false;
    private Vector3 targetPosition;
    private int count = 0;
    public GameObject moveObject;
    public Transform secondMoveObjectTransform;
    public Transform thirdMoveObjectTransform;

    public GameObject parentTrampolineObject;

    private void Start()
    {
        parentTrampolineObject = GameManager.Instance.parentTrampolineObject;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isMoving)
        {
            AudioManager.Instance.PlaySFX("Collision");

            targetPosition = objectA.position;
            StartCoroutine(MoveToTargetPosition(other.transform));
            count++;
        }
    }

    private void Update()
    {
        if (count >= 4)
        {
            moveObject.transform.position = secondMoveObjectTransform.position;
            StartCoroutine(MoveObjectBack());
        }
        //Debug.Log(count);
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

    private IEnumerator MoveObjectBack()
    {
        yield return new WaitForSeconds(2f);
        moveObject.transform.position = thirdMoveObjectTransform.position;
        count = 0;
    }


}