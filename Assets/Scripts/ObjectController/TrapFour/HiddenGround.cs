using UnityEngine;

public class HiddenGround : MonoBehaviour
{
    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            meshRenderer.enabled = false;

            // Set the mesh collider to be a trigger
            meshCollider.isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            meshRenderer.enabled = true;

            // Set the mesh collider to not be a trigger
            meshCollider.isTrigger = false;
        }
    }
}
