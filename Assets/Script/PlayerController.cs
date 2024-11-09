using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnimator;

    public float speed;

    public BoxCollider2D boxCollider;
    public Vector2 crouchSize = new Vector2(1.0f, 1.25f);  // Desired size when crouching (only height reduced)
    private Vector2 crouchOffset;                          // New offset to keep the bottom in place
    private Vector2 originalSize;
    private Vector2 originalOffset;

    private void Start()
    {
        // Store the original size and offset of the collider
        originalSize = boxCollider.size;
        originalOffset = boxCollider.offset;
        Debug.Log("originalOffset.y :" + originalOffset.y + "originalSize.y :" + originalSize.y + "crouchSize.y :" + crouchSize.y);
        // Calculate the crouch offset so the bottom of the collider stays in place
        crouchOffset = new Vector2(originalOffset.x, originalOffset.y - (originalSize.y - crouchSize.y) / 2);
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        MoveCharacter(horizontalInput);
        PlayMovementAnimation(horizontalInput);

        float verticalInput = Input.GetAxis("Vertical");
        PlayJumpAnimation(verticalInput);

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

    private void MoveCharacter(float horizontal)
    {
        Vector3 position = transform.position;
        position.x += horizontal * speed * Time.deltaTime;
        transform.position = position;
    }

    private void PlayMovementAnimation(float horizontal)
    {
        //Set the value of Speed in animator variable
        playerAnimator.SetFloat("Speed", Mathf.Abs(horizontal));

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

    public void PlayJumpAnimation(float vertical)
    {
        // Change the bool value of jump animation when vertical input is greater than zero
        if (vertical > 0)
        {
            playerAnimator.SetBool("Jump", true);
        }
    }

    //Create method to reset the jum variable which is called after the jump animation is completed
    public void TriggerJumpEvent()
    {
        playerAnimator.SetBool("Jump", false);
    }
}
