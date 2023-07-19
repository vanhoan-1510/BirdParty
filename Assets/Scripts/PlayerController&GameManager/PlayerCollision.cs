using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;

    bool isTouchingRotatingObject = false;
    bool isCooldown = false;
    float cooldownTime = 2f;
    float cooldownTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("RotateTrap"))
        {
            // Get the direction from the rotating object to the player
            Vector3 pushDirection = transform.position - collision.transform.position;

            // Set the push direction to be parallel to the ground
            pushDirection.y = 0f;

            // Apply a force to push the player away from the rotating object
            rb.AddForce(pushDirection.normalized * 10f, ForceMode.Impulse);

            animator.SetBool("isDying", true);
            isTouchingRotatingObject = true;
            isCooldown = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("BounceTrap"))
        {
            Vector3 surfaceNormal = collision.contacts[0].normal;

            // Calculate the bounce direction by rotating the surface normal by 45 degrees around the world up axis
            Vector3 bounceDirection = Quaternion.Euler(0f, 0f, 45f) * surfaceNormal;

            // Apply a force to push the player away from the rotating object
            rb.AddForce(bounceDirection.normalized * 10f, ForceMode.Impulse);

            animator.SetBool("isDying", true);
            isTouchingRotatingObject = true;
            isCooldown = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("RotateTrap"))
        {
            isTouchingRotatingObject = false;
        }
    }
}
