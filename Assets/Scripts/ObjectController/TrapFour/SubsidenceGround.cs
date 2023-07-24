using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsidenceGround : MonoBehaviour
{
    private MeshCollider meshCollider;
    private GameObject playerObject;
    public Transform targetPosition;
    private Vector3 objPos;

    private bool isMeshTrigger = false;
    private bool isMovingBack = false;

    private void Start()
    {
        playerObject = GameManager.Instance.playerObject;
        meshCollider = GetComponent<MeshCollider>();
        objPos = transform.position;
    }

    private void Update()
    {
        if (isMeshTrigger && !isMovingBack)
        {
            // Move the object towards targetPosition every frame
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, 5f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerObject)
        {
            // Activate the trigger and stop moving back to the original position if the object is currently moving back
            isMeshTrigger = true;
            StopMovingBack();

            // Set the mesh collider to be a trigger
            meshCollider.isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerObject)
        {
            // Deactivate the trigger and start moving the object back to the original position
            isMeshTrigger = false;
            StartMovingBack();

            // Set the mesh collider to not be a trigger
            meshCollider.isTrigger = false;
        }
    }

    private void StartMovingBack()
    {
        if (!isMovingBack)
        {
            isMovingBack = true;
            StartCoroutine(MoveBackToOriginalPosition());
        }
    }

    private void StopMovingBack()
    {
        if (isMovingBack)
        {
            isMovingBack = false;
            StopCoroutine(MoveBackToOriginalPosition());
        }
    }

    private IEnumerator MoveBackToOriginalPosition()
    {
        while (Vector3.Distance(transform.position, objPos) > 0.01f)
        {
            // Move the object back to the original position every frame
            transform.position = Vector3.MoveTowards(transform.position, objPos, 1f * Time.deltaTime);
            yield return null;
        }
    }
}
