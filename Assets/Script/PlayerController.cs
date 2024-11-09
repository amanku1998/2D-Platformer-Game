using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

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
        Debug.Log("originalOffset.y :"+ originalOffset.y + "originalSize.y :"+ originalSize.y + "crouchSize.y :"+ crouchSize.y);
        // Calculate the crouch offset so the bottom of the collider stays in place
        crouchOffset = new Vector2(originalOffset.x, originalOffset.y - (originalSize.y - crouchSize.y) / 2);
    }

    private void Update()
    {
        float speed = Input.GetAxisRaw("Horizontal");
        float verticalSpeed = Input.GetAxisRaw("Vertical");
        //Set the value of Speed in animator variable
        animator.SetFloat("Speed", Mathf.Abs(speed));

        Vector3 scale = transform.localScale;
        if (speed < 0){
            //Flip the player into left side
            scale.x = -1f * Mathf.Abs(scale.x);
        }
        else if (speed > 0){
            //Flip the player into right side
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;

        //Check for crouch
        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)){
            animator.SetBool("Crouch", true);
            // Change collider size and offset for crouching
            boxCollider.size = crouchSize;
            boxCollider.offset = crouchOffset;
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl)){
            animator.SetBool("Crouch", false);
            //Reset the box collider size back to default size
            boxCollider.size = originalSize;
            boxCollider.offset = originalOffset;
        }

        // Change the bool value of jump animation when vertical input is greater than zero
        if (verticalSpeed > 0){
            animator.SetBool("Jump", true);
        }
    }

    //Create method to reset the jum variable which is called after the jump animation is completed
    public void TriggerJumpEvent(){
        animator.SetBool("Jump", false);
    }
}
