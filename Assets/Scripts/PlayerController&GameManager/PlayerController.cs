using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public static PlayerController Instance;
    Rigidbody rb;
    private float moveSpeed = 1.5f;
    private float sprintSpeed = 2.22f;
    private float rotationSpeed = 15f;
    private float animationBlendSpeed = 2f;
    Camera cam;
    Animator animator;

    [Header("Movement")]
    float mDesiredRotation = 0f;
    float mDesiredAnimationSpeed = 0f;
    bool mSprinting = false;
    bool mJumping = false;

    float jumpForce = 2f;

    [Header("Ground Check")]
    float playerHeight = .1f;
    bool grounded;
    LayerMask whatIsGround;

    [Header("Player Mana")]
    private float maxMana = 100f;
    private float manaReduce = 50f;
    private float currentMana;
    private float manaRecovery = 10f;
    private PowerBar powerBar;

    bool isTouchingRotatingObject = false;
    bool isCooldown = false;
    float cooldownTime = 5f;
    float cooldownTimer = 0f;

    [Header("BounceObject")]
    private GameObject parentObjLeft;
    private GameObject parentObjRight;
    private GameObject parentBTwoObject;
    private GameObject spindleTrap;
    private GameObject parentTrampolineObjectVTwo;
    private GameObject parentGroundBounceRight;
    private GameObject parentDeathObject;

    Vector3 bounceDirection = Vector3.zero;
    Vector3 surfaceNormal;
    float bounceForce = 0f;

    [Header("Check Point")]
    public GameObject player;
    private Vector3 lastCheckpointPosition;


    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        player = this.gameObject;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        cam = GameManager.Instance.cam;
        powerBar = GameManager.Instance.powerBar;

        whatIsGround = LayerMask.GetMask("Ground");

        parentObjLeft = GameManager.Instance.parentObjLeft;
        parentObjRight = GameManager.Instance.parentObjRight;
        parentBTwoObject = GameManager.Instance.parentBTwoObject;
        spindleTrap = GameManager.Instance.spindleTrap;
        parentTrampolineObjectVTwo = GameManager.Instance.parentTrampolineObjectVTwo;
        parentGroundBounceRight = GameManager.Instance.parentGroundBounceRight;
        parentDeathObject = GameManager.Instance.parentDeathObject;

        currentMana = maxMana;
        powerBar.SetMaxMana(maxMana);
    }

    void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f, whatIsGround);

        if (photonView.IsMine)
        {
            MyInput();

            Falling();

            RespawnAtLastCheckpoint();
        }
    }

    private void MyInput()
    {
        if (isCooldown)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && !mJumping && grounded)
        {
            mJumping = true;
            animator.SetBool("isJumping", true);

            //rb.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

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
            currentMana += manaRecovery * Time.deltaTime;
            if(currentMana >= maxMana)
            {
                currentMana = maxMana;
            }

        }
        powerBar.SetMana(currentMana);

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
        
    public void Falling()
    {
        // Check if no longer touching the rotating object
        if (!isTouchingRotatingObject && isCooldown)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= cooldownTime)
            {
                animator.SetBool("isFalling", false);
                isCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }

    [PunRPC]
    public void SaveCheckpoint(Vector3 checkpointPosition)
    {
        // Save the checkpoint position for the player
        lastCheckpointPosition = checkpointPosition;
        Debug.Log("Checkpoint saved for player: " + photonView.Owner.NickName);
    }

    public void RespawnAtLastCheckpoint()
    {
        if (transform.position.y < -20f)
        {
            player.transform.position = lastCheckpointPosition;
            Debug.Log("Player respawned at last checkpoint: " + photonView.Owner.NickName);
        }
    }

    private IEnumerator WaitToLoadCheckPoint(float timeLoad)
    {
        yield return new WaitForSeconds(timeLoad);
        player.transform.position = lastCheckpointPosition;
        isTouchingRotatingObject = false;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("BounceTrap"))
        {
            // Get the surface normal of the contact point between the character and the object it collides with
            surfaceNormal = collision.contacts[0].normal;

            if (IsChildOfParent(collision.gameObject, parentBTwoObject))
            {
                // Get the direction from the rotating object to the player
                bounceDirection = transform.position - collision.transform.position;
                bounceDirection.y = 0f;
                bounceForce = 10f;
            }

            if (IsChildOfParent(collision.gameObject, parentObjLeft))
            {
                bounceDirection = Quaternion.Euler(0f, 0f, 45f) * surfaceNormal;
                bounceForce = 10f;
            }

            if (IsChildOfParent(collision.gameObject, parentObjRight))
            {
                bounceDirection = Quaternion.Euler(0f, 0f, -45f) * surfaceNormal;
                bounceForce = 10f;
            }

            if(IsChildOfParent(collision.gameObject, spindleTrap))
            {
                bounceDirection = transform.position - collision.transform.position;
                bounceForce = 20f;
            }

            if(IsChildOfParent(collision.gameObject, parentTrampolineObjectVTwo))
            {
                bounceDirection = Quaternion.Euler(45f, 0f, 0f) * surfaceNormal;
                bounceForce = 10f;
            }

            if(IsChildOfParent(collision.gameObject, parentGroundBounceRight))
            {
                bounceDirection = Quaternion.Euler(-45f, 0f, 0f) * surfaceNormal;
                bounceForce = 10f;
            }

            if (IsChildOfParent(collision.gameObject, parentDeathObject))
            {
                //load checkpoint
                StartCoroutine(WaitToLoadCheckPoint(1f));
                bounceForce = 0f;
            }

            AddForceToPlayer();
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            animator.SetBool("isFalling", true);
            isTouchingRotatingObject = true;
            isCooldown = true;
            StartCoroutine(WaitToLoadCheckPoint(2f));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BounceTrap"))
        {
            isTouchingRotatingObject = false;
        }
    }

    public void AddForceToPlayer()
    {

        // Apply a force to push the player away from the rotating object
        rb.AddForce(bounceDirection.normalized * bounceForce, ForceMode.Impulse);

        animator.SetBool("isFalling", true);
        isTouchingRotatingObject = true;
        isCooldown = true;
    }

    private bool IsChildOfParent(GameObject childObject, GameObject parentObject)
    {
        Transform parentTransform = childObject.transform.parent;
        while (parentTransform != null)
        {
            if (parentTransform.gameObject == parentObject)
            {
                return true;
            }
            parentTransform = parentTransform.parent;
        }
        return false;
    }
}