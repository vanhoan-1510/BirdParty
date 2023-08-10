using UnityEngine;

public class isTriggerWithEmptyObject : MonoBehaviour
{
    public GameObject trap;

    private void Start()
    {
        trap.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            trap.SetActive(true);
        }
    }
}