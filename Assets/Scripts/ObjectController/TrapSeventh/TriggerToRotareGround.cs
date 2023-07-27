using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerToRotareGround : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 15f;

    private bool isRotated = false;
    private Quaternion targetQuaternion;
    private Quaternion defaultQuaternion;
    private GameObject playerObject;

    Rigidbody rb;


    private void Start()
    {
        playerObject = GameManager.Instance.playerObject;
        rb = playerObject.GetComponent<Rigidbody>();

        targetQuaternion = Quaternion.Euler(transform.eulerAngles + Vector3.forward * -80f);
        defaultQuaternion = Quaternion.Euler(transform.eulerAngles);
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
    IEnumerator WaitToRotateBack()
    {
        yield return new WaitForSeconds(2f);
        isRotated = false;
    }
}