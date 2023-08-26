using System.Collections;
using UnityEngine;
using Photon.Pun;

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
    private GameObject parentTrampolineObject;
    private GameObject parentTrampolineObjectVTwo;
    private GameObject parentGroundBounceRight;
    private GameObject parentDeathObject;

    Vector3 bounceDirection = Vector3.zero;
    Vector3 surfaceNormal;
    float bounceForce = 0f;

    [Header("Check Point")]
    public GameObject player;
    private Vector3 lastCheckpointPosition;

    AudioSource[] playerAudioSources;

    int soundState;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        player = this.gameObject;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerAudioSources = GetComponents<AudioSource>();
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
        parentTrampolineObject = GameManager.Instance.parentTrampolineObject;
        parentTrampolineObjectVTwo = GameManager.Instance.parentTrampolineObjectVTwo;
        parentGroundBounceRight = GameManager.Instance.parentGroundBounceRight;
        parentDeathObject = GameManager.Instance.parentDeathObject;

        currentMana = maxMana;
        powerBar.SetMaxMana(maxMana);

        soundState = PlayerPrefs.GetInt("SoundState");

        if (AudioManager.Instance.playerAudioSources[1] != null && soundState == 1)
        {
            AudioManager.Instance.playerAudioSources[1].enabled = true;
            AudioManager.Instance.playerAudioSources[3].enabled = false;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            playerAudioSources[3].enabled = false;
        }
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
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D)) 
            && grounded && soundState == 1)
        {
            if (AudioManager.Instance.playerAudioSources[3] != null)
            {
                AudioManager.Instance.playerAudioSources[3].enabled = true;
            }
        }
        else
        {
            if (AudioManager.Instance.playerAudioSources[3] != null)
            {
                AudioManager.Instance.playerAudioSources[3].enabled = false;
            }
        }

        if (isCooldown)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && !mJumping && grounded)
        {
            AudioManager.Instance.PlaySFX("Jumping");
            mJumping = true;
            animator.SetBool("isJumping", true);

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
            if (AudioManager.Instance.playerAudioSources[2] != null && soundState == 1)
            {
                AudioManager.Instance.playerAudioSources[2].enabled = true;
                AudioManager.Instance.playerAudioSources[3].enabled = false;
            }

            mSprinting = true;

            currentMana -= manaReduce * Time.deltaTime;

            if (currentMana < 0)
            {
                currentMana = 0;
                PlayerAudioManager.Instance.EndingFlySound();
            }
        }
        else
        {
            if (AudioManager.Instance.playerAudioSources[2] != null)
            {
                AudioManager.Instance.playerAudioSources[2].enabled = false;
            }
            
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
        AudioManager.Instance.PlaySFX("CheckPoint");
        Debug.Log("Checkpoint saved for player: " + photonView.Owner.NickName);
    }

    public void RespawnAtLastCheckpoint()
    {
        if (transform.position.y < -20f)
        {
            AudioManager.Instance.playerAudioSources[1].enabled = false;
            player.transform.position = lastCheckpointPosition;
            Debug.Log("Player respawned at last checkpoint: " + photonView.Owner.NickName);

            if(AudioManager.Instance.playerAudioSources[1] != null && soundState == 1)
            {
                AudioManager.Instance.playerAudioSources[1].enabled = true;
            }
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
            AudioManager.Instance.PlaySFX("Collision");
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
                AudioManager.Instance.PlaySFX("Eggy3");
                bounceDirection = Quaternion.Euler(0f, 0f, 45f) * surfaceNormal;
                bounceForce = 10f;

            }

            if (IsChildOfParent(collision.gameObject, parentObjRight))
            {
                AudioManager.Instance.PlaySFX("Eggy3");
                bounceDirection = Quaternion.Euler(0f, 0f, -45f) * surfaceNormal;
                bounceForce = 10f;
            }

            if(IsChildOfParent(collision.gameObject, spindleTrap))
            {
                AudioManager.Instance.PlaySFX("Eggy3");
                bounceDirection = transform.position - collision.transform.position;
                bounceForce = 20f;
            }

            if(IsChildOfParent(collision.gameObject, parentTrampolineObject))
            {
                AudioManager.Instance.PlaySFX("Eggy2");
                bounceDirection = Quaternion.Euler(45f, 0f, 0f) * surfaceNormal;
                bounceForce = 10f;
            }

            if(IsChildOfParent(collision.gameObject, parentTrampolineObjectVTwo))
            {
                AudioManager.Instance.PlaySFX("Eggy2");
                bounceDirection = Quaternion.Euler(45f, 0f, 0f) * surfaceNormal;
                bounceForce = 10f;
            }

            if(IsChildOfParent(collision.gameObject, parentGroundBounceRight))
            {
                AudioManager.Instance.PlaySFX("Eggy1");
                bounceDirection = Quaternion.Euler(-45f, 0f, 0f) * surfaceNormal;
                bounceForce = 10f;
            }

            if (IsChildOfParent(collision.gameObject, parentDeathObject))
            {
                AudioManager.Instance.PlaySFX("Eggy3");
                //load checkpoint
                StartCoroutine(WaitToLoadCheckPoint(1f));
                bounceForce = 0f;
            }

            AddForceToPlayer();
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            AudioManager.Instance.PlaySFX("Eggy3");
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

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            AudioManager.Instance.playerAudioSources[3].enabled = false;
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

    public bool IsChildOfParent(GameObject childObject, GameObject parentObject)
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