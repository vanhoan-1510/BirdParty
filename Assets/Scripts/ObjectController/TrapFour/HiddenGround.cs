using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenGround : MonoBehaviour
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
        if (other.gameObject == playerObject)
        {

            meshRenderer.enabled = false;

            // Set the mesh collider to be a trigger
            meshCollider.isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerObject)
        {

            meshRenderer.enabled = true;

            // Set the mesh collider to not be a trigger
            meshCollider.isTrigger = false;
        }
    }
}
