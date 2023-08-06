using System.Collections;
using UnityEngine;

public class TriggerAndWaitToMove : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private float moveSpeed = 5f;

    private bool isMoving = false;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
;    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            StartCoroutine(WaitToMove(1f));
        }
    }

    private IEnumerator WaitToMove(float timeToMove)
    {
        yield return new WaitForSeconds(timeToMove);
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetObject.position, moveSpeed * Time.deltaTime);

            if (transform.position == targetObject.position)
            {
                isMoving = false;
            }
        }
    }
}
