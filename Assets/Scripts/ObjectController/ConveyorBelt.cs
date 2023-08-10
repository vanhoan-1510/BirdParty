using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float conveyorSpeed = 2.0f;
    public Vector3 direction;
    public List<GameObject> playersOnBelt;

    private float timeOnBelt = 0.0f;
    private float timeToApplyForce = 1.0f;


    //private void Update()
    //{
    //    if (playersOnBelt.Contains(player))
    //    {
    //        timeOnBelt += Time.deltaTime;

    //        if (timeOnBelt <= timeToApplyForce)
    //        {
    //            player.transform.position += direction * conveyorSpeed * Time.deltaTime;
    //        }
    //    }
    //    else
    //    {
    //        timeOnBelt = 0.0f;
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playersOnBelt.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playersOnBelt.Remove(collision.gameObject);
            timeOnBelt = 0.0f;
        }
    }
}
