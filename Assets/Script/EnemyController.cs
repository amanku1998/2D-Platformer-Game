using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public Rigidbody2D rb;
    public LayerMask groundLayers;

    public Transform groundCheck;
    public Transform frontCheck;

    bool isFacingRight = true;

    RaycastHit2D groundHit;
    RaycastHit2D frontHit;

    public Animator animator; // Reference to the Animator
    public float idleTime = 2f; // Time to wait in idle state

    //private bool isFacingRight = true;
    private bool isIdle = false; // Check if the enemy is in the idle state

    private void Update()
    {
        // Check for ground and obstacles
        groundHit = Physics2D.Raycast(groundCheck.position, -transform.up, 1f, groundLayers);
        frontHit = Physics2D.Raycast(frontCheck.position, isFacingRight ? transform.right : -transform.right, 1f, groundLayers);
        // Update animation parameters
        //animator.SetBool("isIdle", isIdle);
        animator.SetBool("isMoving", !isIdle && Mathf.Abs(rb.velocity.x) > 0.1f);
    }

    private void FixedUpdate()
    {
        if (isIdle) return; // Skip movement if idle

        if (groundHit.collider != null && frontHit.collider == null)
        {
            // Move enemy
            rb.velocity = new Vector2(isFacingRight ? speed : -speed, rb.velocity.y);
        }
        else
        {
            // Stop movement and trigger idle behavior
            rb.velocity = Vector2.zero;

            // Ensure the coroutine is only started once
            if (!isIdle)
            {
                StartCoroutine(HandleDirectionChange());
            }
        }
    }

    private void FlipDirection()
    {
        isFacingRight = !isFacingRight;
        // Flip the enemy's direction visually
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private IEnumerator HandleDirectionChange()
    {
        isIdle = true; // Enter idle state
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(idleTime);

        FlipDirection(); // Change direction
        isIdle = false; // Resume movement
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            //playerController.KillPlayer();
            playerController.GetHurt();
        }
    }
}
