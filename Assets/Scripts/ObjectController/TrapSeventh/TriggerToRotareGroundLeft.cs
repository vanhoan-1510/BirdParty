using UnityEngine;
using Photon.Pun;

public class TriggerToRotareGroundLeft : MonoBehaviourPunCallbacks
{
    [SerializeField] private float rotateSpeed = 15f;

    private bool isRotated = false;
    private Quaternion targetQuaternion;


    private void Start()
    {
        targetQuaternion = Quaternion.Euler(transform.eulerAngles + Vector3.forward * 80f);
    }
    private void Update()
    {
        if (isRotated)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, rotateSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isRotated = true;
            photonView.RPC("Rotate", RpcTarget.All);
        }
    }

    [PunRPC]
    public void Rotate()
    {
        isRotated = true;
    }
}