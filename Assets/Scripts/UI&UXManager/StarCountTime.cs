using UnityEngine;
using Photon.Pun;

public class StarCountTime : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Tinh thoi gian");
        }
    }
}
