using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToAppear : MonoBehaviour
{
    public GameObject objectIsInActive;

    private void Start()
    {
        objectIsInActive.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            objectIsInActive.SetActive(true);
        }
    }
}
