using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public ScoreController scoreController;
    [SerializeField] private Animator playerAnimator;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;

    [SerializeField] private Rigidbody2D rigidbodyPlayer;
    [SerializeField] private BoxCollider2D boxCollider;

    //public void KillPlayer()
    //{
    //    HealthManager.health--;

    //    if (HealthManager.health <= 0)
    //    {
    //        Debug.Log("Player killed by enemy");
    //        //Destroy(gameObject);
    //        playerAnimator.SetTrigger("isPlayerDead");
    //    }
    //    else
    //    {
    //        playerAnimator.SetTrigger("isPlayerHurt");
    //    }
    //}

    public void GetHurt()
    {
        HealthManager.health--;

        if (HealthManager.health <= 0)
        {
            Debug.Log("Player killed by enemy");
            //Destroy(gameObject);
            playerAnimator.SetTrigger("isPlayerDead");
        }
        else
        {
            StartCoroutine(decreaseHealth());
        }
    }

    IEnumerator decreaseHealth()
    {
        Physics2D.IgnoreLayerCollision(7,8);
        playerAnimator.SetTrigger("isPlayerHurt");
        yield return new WaitForSeconds(2);
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(0);
    }

    public Vector2 crouchSize = new Vector2(1.0f, 1.25f);  // Desired size when crouching (only height reduced)

    public void PickUpKey()
    {
        scoreController.IncreaseScore(10);
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
        // Calculate the crouch offset so the bottom of the collider stays in place
        crouchOffset = new Vector2(originalOffset.x, originalOffset.y - (originalSize.y - crouchSize.y) / 2);
    }

    private void Update()
    {
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
    }

    private void HorizontalAnimation(float horizontal)
    {
        playerAnimator.SetBool("isGrounded", isGrounded);
        if (isGrounded)
        {
            //Horizontal animation
            playerAnimator.SetFloat("Speed", Mathf.Abs(horizontal));
        }
        else
        {
            //Horizontal animation
            playerAnimator.SetFloat("Speed", 0);
        }

        //Flipping the player
        Vector2 scale = transform.localScale;
        if (horizontal < 0)
        {
            scale.x = -1f * Mathf.Abs(scale.x);
        }
        else if (horizontal > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
    }


    public void MovePlayerVertically(float vertical)
    {
        if (vertical > 0 && isGrounded)
        {
            playerAnimator.SetTrigger("Jump");
            rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x, jumpPower);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.tag == "platform")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "platform")
        {
            isGrounded = false;
        }
    }

}
