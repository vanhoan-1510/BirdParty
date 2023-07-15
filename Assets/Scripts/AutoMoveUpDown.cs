using UnityEngine;

public class AutoMoveUpDown : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f; // T?c ?? di chuy?n
    [SerializeField] private float maxHeight = 5f; // ?? cao t?i ?a
    [SerializeField] private float minHeight = 1f; // ?? cao t?i thi?u

    private Vector3 initialPosition;
    private bool movingUp = true;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Di chuy?n lên xu?ng
        if (movingUp)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            if (transform.position.y >= initialPosition.y + maxHeight)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            if (transform.position.y <= initialPosition.y - minHeight)
            {
                movingUp = true;
            }
        }
    }
}
