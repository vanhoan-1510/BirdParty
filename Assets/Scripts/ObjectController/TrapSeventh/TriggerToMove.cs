using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToMove : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private float moveSpeed = 5f;
    private GameObject playerObject;

    private bool isMoving = false;

    private void Start()
    {
        playerObject = GameManager.Instance.playerObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerObject)
        {
            isMoving = true;
        }
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
