using UnityEngine;

public class HiddenGround : MonoBehaviour
{
    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;
    private GameObject player;

    private void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {

            meshRenderer.enabled = false;

            // Set the mesh collider to be a trigger
            meshCollider.isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {

            meshRenderer.enabled = true;

            // Set the mesh collider to not be a trigger
            meshCollider.isTrigger = false;
        }
    }
}
