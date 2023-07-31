using UnityEngine;

public class TriggerToRotareGroundRight : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 15f;

    private bool isRotated = false;
    private Quaternion targetQuaternion;
    private GameObject playerObject;

    private void Start()
    {
        playerObject = GameManager.Instance.playerObject;

        targetQuaternion = Quaternion.Euler(transform.eulerAngles + Vector3.forward * -80f);
    }
    private void Update()
    {
        if (isRotated)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, rotateSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerObject)
        {
            isRotated = true;;
        }
    }
}