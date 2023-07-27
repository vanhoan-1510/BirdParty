using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearWhenCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
