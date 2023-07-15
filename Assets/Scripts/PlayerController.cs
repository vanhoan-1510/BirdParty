using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float sprintSpeed = 5f;
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float animationBlendSpeed = 2f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] Camera cam;
    Animator animator;

    float mDesiredRotation = 0f;
    float mDesiredAnimationSpeed = 0f;
    bool mSprinting = false;
    bool mJumping = false;

    [SerializeField] float jumpForce;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask whatIsGround;
    bool grounded;

    [SerializeField] float maxMana = 100f;
    [SerializeField] float manaReduce = 10f;
    float currentMana;

    bool isTouchingRotatingObject = false;
    bool isCooldown = false;
    float cooldownTime = 2f;
    float cooldownTimer = 0f;

    [Header("BouncObject")]
    [SerializeField] float bounceForce = 5f;
    [SerializeField] LayerMask bounceLayer;
    [SerializeField] float bounceAngle = 45f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        currentMana = maxMana;
    }

    void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();

        Debug.Log("Mana: " + currentMana);

        Die();
    }

    private void MyInput()
    {
        if (isCooldown)
        {
            return; // Ngăn chặn di chuyển khi đang trong thời gian cooldown
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && !mJumping && grounded)
        {
            mJumping = true;
            animator.SetBool("isJumping", true);

            //rb.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            Debug.Log("Jump"); 

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else
        {
            mJumping = false;
            animator.SetBool("isJumping", false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && (horizontalInput != 0 || verticalInput != 0) && currentMana > 0)
        {
            mSprinting = true;

            currentMana -= manaReduce * Time.deltaTime;

            if (currentMana < 0)
            {
                currentMana = 0;
            }
        }
        else
        {
            mSprinting = false;
        }

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
        Vector3 rotatedMovement = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0) * movement;

        Vector3 newVelocity = rb.velocity;
        newVelocity.x = rotatedMovement.x * (mSprinting ? sprintSpeed : moveSpeed);
        newVelocity.z = rotatedMovement.z * (mSprinting ? sprintSpeed : moveSpeed);
        rb.velocity = newVelocity;

        if (rotatedMovement.magnitude > 0)
        {
            mDesiredRotation = Mathf.Atan2(rotatedMovement.x, rotatedMovement.z) * Mathf.Rad2Deg;
            mDesiredAnimationSpeed = mSprinting ? 1f : 0.5f;
        }
        else
        {
            mDesiredAnimationSpeed = 0f;
        }

        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), mDesiredAnimationSpeed, animationBlendSpeed * Time.deltaTime));

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, mDesiredRotation, 0);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Die()
    {
        // Check if no longer touching the rotating object
        if (!isTouchingRotatingObject && isCooldown)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= cooldownTime)
            {
                animator.SetBool("isDying", false);
                isCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        float raycastDistance = 0.1f;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("RotatingObject"))
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

        if (collision.gameObject.CompareTag("BounceGround")) 
        {

            Vector3 bounceDirection = collision.contacts[0].normal;

            rb.AddForce(Vector3.ProjectOnPlane(bounceDirection, Vector3.up).normalized * bounceForce, ForceMode.Impulse);


        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("RotatingObject"))
        {
            isTouchingRotatingObject = false;
        }
    }
}