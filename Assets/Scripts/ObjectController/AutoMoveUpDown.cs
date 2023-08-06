using UnityEngine;

public class AutoMoveUpDown : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxHeight = 5f;
    [SerializeField] private float minHeight = 1f;
    private bool isColliding = false;

    private Vector3 initialPosition;
    private bool movingUp = true;

    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (!isColliding)
        {
            AutoMove();
        }
    }

    private void AutoMove()
    {
        if (movingUp)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }

        if (transform.position.y >= initialPosition.y + maxHeight)
        {
            movingUp = false;
        }
        else if (transform.position.y <= initialPosition.y + minHeight)
        {
            movingUp = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            isColliding = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player)
        {
            movingUp = true;
        }
    }
}
