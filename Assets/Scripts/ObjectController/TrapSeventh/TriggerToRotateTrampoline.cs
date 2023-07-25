using System.Collections;
using UnityEngine;

public class TriggerToRotateTrampoline : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 15f;

    private bool isRotating = false;
    private Quaternion targetQuaternion;
    private Quaternion defaultQuaternion;
    [SerializeField] private GameObject trap;


    private void Start()
    {
        targetQuaternion = Quaternion.Euler(transform.eulerAngles + Vector3.right * 15f);
        defaultQuaternion = Quaternion.Euler(transform.eulerAngles);
        trap.SetActive(false);
    }
    private void Update()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, rotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultQuaternion, rotateSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.playerObject)
        {
            isRotating = true;
            trap.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.playerObject)
        {
            StartCoroutine(WaitToRotateBack());
        }
    }

    IEnumerator WaitToRotateBack()
    {
        yield return new WaitForSeconds(2f);
        isRotating = false;
    }
}
