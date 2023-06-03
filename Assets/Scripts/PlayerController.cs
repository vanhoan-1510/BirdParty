using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    public float moveSpeed = 2f;
    public float sprintSpeed = 5f;
    public float rotationSpeed = 15f;
    public float animationBlendSpeed = 2f;
    public float jumpSpeed = 5f;
    public float bounceForce = 5f;
    public float minBounceAngle = 45f;
    public Camera cam;
    Animator animator;

    float mDesiredRotation = 0f;
    float mDesiredAnimationSpeed = 0f;
    bool mSprinting = false;

    public float mSpeedY = 0f;
    public float mGravity = -9.81f;
    bool mJumping = false;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

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
            Debug.Log("Jump");         
            //animator.SetBool("isJump", true);

            mSpeedY += jumpSpeed  ; // Apply jump to the player
        }
        else
        {
            mJumping = false;
        }

        if (!controller.isGrounded)
        {
            mSpeedY += mGravity * Time.deltaTime; // Apply gravity to the player
        }
        else if(mSpeedY < 0f)
        {
            mSpeedY = 0f;
        }

        animator.SetFloat("SpeedY", mSpeedY / jumpSpeed); // Set the animation speed
        if(mJumping && mSpeedY < 0f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, .5f, LayerMask.GetMask("Default")))
            {
                mJumping = false;
                animator.SetTrigger("Land");
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Kiểm tra layer của đối tượng va chạm
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bounce"))
        {
            Vector3 characterBottom = controller.bounds.center - new Vector3(0f, controller.bounds.extents.y, 0f);
            Vector3 objectTop = hit.collider.bounds.center + new Vector3(0f, hit.collider.bounds.extents.y, 0f);

            // Kiểm tra điểm dưới của nhân vật chạm vào điểm top của đối tượng
            if (characterBottom.y >= objectTop.y)
            {
                // Áp dụng lực nảy lên
                mSpeedY = bounceForce;
            }
        }
    }
}
