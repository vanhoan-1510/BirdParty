using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenTrampoline : MonoBehaviour
{
    private MeshCollider meshCollider;
    private GameObject playerObject;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        playerObject = GameManager.Instance.playerObject;
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        meshRenderer.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        meshRenderer.enabled = false;
    }
}
