using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float idleTime = 2f;
    public LayerMask groundLayers;
    public LayerMask enemyLayers;

    [Header("Raycast Settings")]
    public Transform groundCheck;
    public Transform frontCheck;
    public float rayDistance = 1f;

    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;

    private bool isFacingRight = true;
    private bool isIdle = false;

    private void Update()
    {
        // Draw debug lines for raycasts
        Debug.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * rayDistance, Color.green);
        Debug.DrawLine(frontCheck.position, frontCheck.position + (isFacingRight ? Vector3.right : Vector3.left) * rayDistance, Color.red);

        // Update animation parameters
        animator.SetBool("isMoving", !isIdle && Mathf.Abs(rb.velocity.x) > 0.1f);
    }

    private void FixedUpdate()
    {
        if (isIdle) return;

        // Perform raycasts
        RaycastHit2D groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, rayDistance, groundLayers);
        RaycastHit2D frontHit = Physics2D.Raycast(frontCheck.position, isFacingRight ? Vector2.right : Vector2.left, rayDistance, enemyLayers);

        // Handle movement and obstacle detection
        if (groundHit.collider != null && frontHit.collider == null)
        {
            // Move the enemy
            rb.velocity = new Vector2((isFacingRight ? speed : -speed), rb.velocity.y);
        }
        else
        {
            // Enter idle state and stop movement
            rb.velocity = Vector2.zero;

            if (!isIdle)
            {
                StartCoroutine(HandleDirectionChange());
            }
        }
    }

    private IEnumerator HandleDirectionChange()
    {
        isIdle = true; // Enter idle state

        yield return new WaitForSeconds(idleTime);

        FlipDirection(); // Change direction
        isIdle = false; // Exit idle state
    }

    private void FlipDirection()
    {
        isFacingRight = !isFacingRight;

        // Flip the enemy's direction visually
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            // Damage the player
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.GetHurt();
        }
        else if (collision.gameObject.CompareTag("enemy"))
        {
            // Handle collision with another enemy
            if (!isIdle)
            {
                StartCoroutine(HandleDirectionChange());
            }
        }
    }

}
