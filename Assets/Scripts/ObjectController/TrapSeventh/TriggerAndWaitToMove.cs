using System.Collections;
using UnityEngine;
using Photon.Pun;

public class TriggerAndWaitToMove : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private float moveSpeed = 5f;

    private bool isMoving = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Move();
            photonView.RPC("Move", RpcTarget.All);
        }
    }

    [PunRPC]
    public void Move()
    {
        StartCoroutine(WaitToMove(1f));
    }

    private IEnumerator WaitToMove(float timeToMove)
    {
        yield return new WaitForSeconds(timeToMove);
        isMoving = true;
        AudioManager.Instance.PlaySFX("Eggy2");
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
