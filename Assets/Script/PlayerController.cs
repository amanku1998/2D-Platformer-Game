using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnimator;

    public float speed;
    public float jumpPower;

    private Rigidbody2D rb2d;
    public BoxCollider2D boxCollider;
    public LayerMask groundLayer; // LayerMask to specify ground layer

    public Vector2 crouchSize = new Vector2(1.0f, 1.25f);  // Desired size when crouching (only height reduced)
    private Vector2 crouchOffset;                          // New offset to keep the bottom in place
    private Vector2 originalSize;
    private Vector2 originalOffset;

    private bool isGrounded;

    private void Awake()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Store the original size and offset of the collider
        originalSize = boxCollider.size;
        originalOffset = boxCollider.offset;
        //Debug.Log("originalOffset.y :" + originalOffset.y + "originalSize.y :" + originalSize.y + "crouchSize.y :" + crouchSize.y);
        // Calculate the crouch offset so the bottom of the collider stays in place
        crouchOffset = new Vector2(originalOffset.x, originalOffset.y - (originalSize.y - crouchSize.y) / 2);
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Jump");

        MoveCharacter(horizontalInput, verticalInput);
        PlayMovementAnimation(horizontalInput, verticalInput);

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

    // Method to check if the player is on the ground
    private void CheckGrounded()
    {
        // Check if the boxCollider is colliding with the ground layer using Physics2D.OverlapBox
        isGrounded = Physics2D.OverlapBox(boxCollider.bounds.center, boxCollider.bounds.size, 0f, groundLayer);
    }

    private void MoveCharacter(float horizontal , float vertical)
    {
        //Move player horizontally
        Vector3 position = transform.position;
        position.x += horizontal * speed * Time.deltaTime;
        transform.position = position;

        CheckGrounded();

        Debug.Log("vertical :"+ vertical  + "CheckGrounded :"+ isGrounded);
        //Move player Vertically
        if (vertical > 0 && isGrounded)
        {
            rb2d.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Force);
            isGrounded = false; // Set isGrounded to false until the player touches the ground again
        }
    }

    private void PlayMovementAnimation(float horizontal, float vertical)
    {
        if (isGrounded)
        {
            //Set the value of Speed in animator variable
            playerAnimator.SetFloat("Speed", Mathf.Abs(horizontal));
        }

        Vector3 scale = transform.localScale;
        if (horizontal < 0)
        {
            //Flip the player into left side
            scale.x = -1f * Mathf.Abs(scale.x);
        }
        else if (horizontal > 0)
        {
            //Flip the player into right side
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;

        // Change the bool value of jump animation when vertical input is greater than zero
        if (vertical > 0 /*&& isGrounded*/)
        {
            playerAnimator.SetBool("Jump", true);
        }
        else
        {
            playerAnimator.SetBool("Jump", false);
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

    private void OnDrawGizmos()
    {
        // Set Gizmo color to visualize the overlap box
        Gizmos.color = Color.red;

        // Draw the overlap box at the collider's center with the same size as the boxCollider
        if (boxCollider != null)
        {
            // You can adjust the color or transparency for better visibility
            Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
        }
    }

}
