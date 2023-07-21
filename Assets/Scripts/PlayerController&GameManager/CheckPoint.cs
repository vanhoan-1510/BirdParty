using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] List<GameObject> checkPointList;

    [SerializeField] Vector3 checkPont;
    [SerializeField] float deadPosY;

    private void Update()
    {
        if(player.transform.position.y < deadPosY)
        {
            player.transform.position = checkPont;
        }
    }




}
