using UnityEngine;

public class isTriggerWithEmptyObject : MonoBehaviour
{
    public GameObject trap;
    public GameObject player;

    private void Start()
    {
        trap.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            trap.SetActive(true);
        }
    }
}