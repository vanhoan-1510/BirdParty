using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    public float moveSpeed = 2f;
    public float sprintSpeed = 5f;
    public float rotationSpeed = 15f;
    public Camera cam;
    Animator animator;

    float mDesiredRotation = 0f;
    float mDesiredAnimationSpeed = 0f;
    public float animationBlendSpeed = 2f;
    bool mSprinting = false;

    public float mSpeedY = 0f;
    public float mGravity = -9.81f;
    public float jumpHeight = 3f;
    bool mJumping = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MyInpput();
    }

    private void MyInpput()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Get the input
        float verticalInput = Input.GetAxis("Vertical"); // Get the input

        if(Input.GetButtonDown("Jump") && !mJumping)
        {
            mJumping = true;
            animator.SetTrigger("Jump");

            mSpeedY += jumpHeight; // Apply jump to the player
        }

        if (!controller.isGrounded)
        {
            mSpeedY += mGravity * Time.deltaTime; // Apply gravity to the player
        }
        else if(mSpeedY < 0f)
        {
            mSpeedY = 0f;
        }

        animator.SetFloat("SpeedY", mSpeedY / jumpHeight); // Set the animation speed
        if(mJumping && mSpeedY < 0f)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
            {
                mJumping = false;
            }
        }

        mSpeedY += mGravity * Time.deltaTime; // Apply gravity to the player
        mSprinting = Input.GetKey(KeyCode.LeftShift); // Get the input


        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized; // Get the movement vector


        Vector3 rotatedMovement = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0) * movement; // Rotate the movement vector to the direction of the camera
        Vector3 verticalMovement = Vector3.up * mSpeedY; // Get the vertical movement vector
        controller.Move((verticalMovement + (rotatedMovement * (mSprinting ? sprintSpeed : moveSpeed))) * Time.deltaTime); // Move the player

        if(rotatedMovement.magnitude > 0) // If the player is moving (magnitude is the length of the vector 
        {
            mDesiredRotation = Mathf.Atan2(rotatedMovement.x, rotatedMovement.z) * Mathf.Rad2Deg; // Get the angle of movement
            mDesiredAnimationSpeed = mSprinting ? 1f : 0.5f; // Set the animation speed
        }
        else
        {
            mDesiredAnimationSpeed = 0f;
        }

        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), mDesiredAnimationSpeed, animationBlendSpeed * Time.deltaTime)); // Set the animation speed

        Quaternion currentRotation = transform.rotation; 
        Quaternion targetRotation = Quaternion.Euler(0, mDesiredRotation, 0);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
}