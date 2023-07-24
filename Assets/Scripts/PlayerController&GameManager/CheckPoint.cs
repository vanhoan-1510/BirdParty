using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] GameObject player;
    [SerializeField] List<GameObject> checkPointList;
    [SerializeField] Vector3 checkPoint;
    [SerializeField] float deadPosY;

    private void Update()
    {
        if(player.transform.position.y < deadPosY)
        {
            player.transform.position = checkPoint;

            rb.velocity = Vector3.zero;
        }
    }
}
