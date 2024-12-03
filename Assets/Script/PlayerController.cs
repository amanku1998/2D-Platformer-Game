using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public ScoreController scoreController;
    public GameOverController gameOverController;

    public ParticleController playerDeadParticleEffect;

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;

    [SerializeField] private Rigidbody2D rigidbodyPlayer;
    //[SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private CapsuleCollider2D boxCollider;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private bool isHurt = false; // Tracks if the player is currently hurt

    // New variable to store the original offset for the ground check
    private Vector2 originalGroundCheckOffset;

    private bool isPlayingFootstep = false;

    public Animator GetPlayerAnimator()
    {
        return playerAnimator; 
    }

    public void GetHurt()
    {
        if (HealthManager.health <= 0 || isHurt)
            return;

        isHurt = true; // Set hurt state

        HealthManager.health--;
        SoundManager.Instance.Play(Sounds.PlayerDeath);
        if (HealthManager.health <= 0)
        {
            KillPlayer();
            isHurt = false; // Reset hurt state in case of death
        }
        else
        {
            StartCoroutine(decreaseHealth());
        }
    }

    private void KillPlayer()
    {
        playerDeadParticleEffect.PlayEffect();
        StartCoroutine(hidePlayerCollisionWithEnemy());
        Debug.Log("Player killed by enemy");
        //Destroy(gameObject);
        playerAnimator.SetTrigger("isPlayerDead");
    }

    public void EnableGameoverPanel()
    {
        gameOverController.PlayerDead();
        this.enabled = false;
    }

    IEnumerator decreaseHealth()
    {
        Physics2D.IgnoreLayerCollision(7,8);
        playerAnimator.SetTrigger("isPlayerHurt");
        yield return new WaitForSeconds(3);
        Physics2D.IgnoreLayerCollision(7, 8, false);

        isHurt = false; // Reset hurt state
    }

    IEnumerator hidePlayerCollisionWithEnemy()
    {
        Physics2D.IgnoreLayerCollision(7, 8);
        yield return new WaitForSeconds(2f);
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    public Vector2 crouchSize = new Vector2(1.0f, 1.25f);  // Desired size when crouching (only height reduced)

    // New variables for jump collider size and offset
    public Vector2 jumpSize = new Vector2(1.0f, 1.25f);  // Desired size when jumping
    private Vector2 jumpOffset;                          // Offset for jump collider


    public void PickUpKey()
    {
        SoundManager.Instance.Play(Sounds.PlayerCollectable);
        scoreController.IncreaseScore(1);
    }

    private Vector2 crouchOffset;                          // New offset to keep the bottom in place
    private Vector2 originalSize;
    private Vector2 originalOffset;
    [SerializeField] private bool isGrounded;


    private void Awake()
    {
        rigidbodyPlayer = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Store the original size and offset of the collider
        originalSize = boxCollider.size;
        originalOffset = boxCollider.offset;

        // Store the original ground check offset relative to the player's position
        originalGroundCheckOffset = groundCheck.localPosition;

        // Calculate the crouch offset so the bottom of the collider stays in place
        crouchOffset = new Vector2(originalOffset.x, originalOffset.y - (originalSize.y - crouchSize.y) / 2);

        jumpOffset = new Vector2(originalOffset.x, originalOffset.y + (originalSize.y - jumpSize.y) / 2);
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float horizontal = Input.GetAxisRaw("Horizontal");

        HorizontalAnimation(horizontal);
        MoveCharacter(horizontal);

        float vertical = Input.GetAxisRaw("Vertical");
        MovePlayerVertically(vertical);

        //Check for crouch
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            Crouch(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            Crouch(false);
        }

        // Adjust collider for jumping or reset when grounded
        if (!isGrounded)
        {
            AdjustColliderForJump();
        }
        else
        {
            ResetCollider();
        }

        // Check for fall animation condition
        if (!isGrounded && rigidbodyPlayer.velocity.y < 0)
        {
            playerAnimator.SetBool("isFalling", true);
        }
        else if (isGrounded)
        {
            playerAnimator.SetBool("isFalling", false);
        }
    }

    public void Crouch(bool crouch)
    {
        if (crouch == true)
        {
            // Change collider size and offset for crouching
            boxCollider.size = crouchSize;
            boxCollider.offset = crouchOffset;
        }
        else
        {
            //Reset the box collider size back to default size
            boxCollider.size = originalSize;
            boxCollider.offset = originalOffset;
        }

        playerAnimator.SetBool("Crouch", crouch);
    }

    private void MoveCharacter(float horizontal)
    {
        // Horizontal character movement
        Vector3 newPosition = transform.position;
        newPosition.x += horizontal * moveSpeed * Time.deltaTime;
        transform.position = newPosition;

        //SoundManager.Instance.PlayMusic(Sounds.PlayerFootSteps);

        // Play footstep sound only if grounded and moving
        if (isGrounded && Mathf.Abs(horizontal) > 0)
        {
            if (!isPlayingFootstep)
            {
                isPlayingFootstep = true;
                SoundManager.Instance.Play(Sounds.PlayerFootSteps);
                StartCoroutine(ResetFootstepSound());
            }
        }
        else
        {
            isPlayingFootstep = false;
        }
    }

    private IEnumerator ResetFootstepSound()
    {
        yield return new WaitForSeconds(0.5f); // Adjust to match your footstep timing
        isPlayingFootstep = false;
    }

    private void HorizontalAnimation(float horizontal)
    {
        playerAnimator.SetBool("isGrounded", isGrounded);
        if (isGrounded)
        {
            //Horizontal animation
            playerAnimator.SetFloat("Speed", Mathf.Abs(horizontal));
            //SoundManager.Instance.Play(Sounds.PlayerFootSteps);
        }
        else{
            //Horizontal animation
            playerAnimator.SetFloat("Speed", 0);
        }

        //Flipping the player
        Vector2 scale = transform.localScale;
        if (horizontal < 0)
        {
            scale.x = -1f * Mathf.Abs(scale.x);
        }
        else if (horizontal > 0){
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
    }

    private void AdjustColliderForJump()
    {
        // Change collider size and offset for jumping
        boxCollider.size = jumpSize;
        boxCollider.offset = jumpOffset;
    }

    private void ResetCollider()
    {
        // Reset the collider size and offset to the original
        boxCollider.size = originalSize;
        boxCollider.offset = originalOffset;
    }

    public void MovePlayerVertically(float vertical)
    {
        if (vertical > 0 && isGrounded)
        {
            playerAnimator.SetTrigger("Jump");
            rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x, jumpPower);

            SoundManager.Instance.Play(Sounds.PlayerJump);
        }
    }
}
