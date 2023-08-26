using UnityEngine;
using Photon.Pun;

public class TriggerToMove : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private float moveSpeed = 5f;
    private GameObject player;

    private bool isMoving = false;


    private void OnTriggerEnter(Collider other)
    {
        isMoving = true;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            photonView.RPC("Move", RpcTarget.All);
        }
    }

    [PunRPC]
    public void Move()
    {
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetObject.position, moveSpeed * Time.deltaTime);

            if (transform.position == targetObject.position)
            {
                isMoving = false;
            }
        }
    }
}
