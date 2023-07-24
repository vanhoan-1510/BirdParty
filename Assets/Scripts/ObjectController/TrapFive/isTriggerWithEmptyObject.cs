using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isTriggerWithEmptyObject : MonoBehaviour
{
    private GameObject playerObject;
    public GameObject trap;

    private void Start()
    {
        playerObject = GameManager.Instance.playerObject;
        trap.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerObject)
        {
            trap.SetActive(true);
        }
    }
}
