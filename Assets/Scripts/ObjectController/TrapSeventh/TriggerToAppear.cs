using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToAppear : MonoBehaviour
{
    public GameObject objectIsInActive;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        objectIsInActive.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            objectIsInActive.SetActive(true);
        }
    }
}
