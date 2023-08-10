using UnityEngine;

public class TriggerToMove : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private float moveSpeed = 5f;
    private GameObject player;

    private bool isMoving = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
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
