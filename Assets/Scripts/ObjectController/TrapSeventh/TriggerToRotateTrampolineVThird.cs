using System.Collections;
using UnityEngine;

public class TriggerToRotateTrampolineVThird : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 15f;

    private bool isRotated = false;
    private Quaternion targetQuaternion;
    private Quaternion defaultQuaternion;
    [SerializeField] private GameObject trap;



    private void Start()
    {
        targetQuaternion = Quaternion.Euler(transform.eulerAngles + Vector3.left * 15f);
        defaultQuaternion = Quaternion.Euler(transform.eulerAngles);
        trap.SetActive(false);
    }
    private void Update()
    {
        if (isRotated)
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isRotated = true;
            trap.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(WaitToRotateBack());
        }
    }

    IEnumerator WaitToRotateBack()
    {
        yield return new WaitForSeconds(2f);
        isRotated = false;
    }
}