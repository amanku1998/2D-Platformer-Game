using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float idleTime = 1f;

    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Collision Settings")]
    public Collider2D leftBoundaryCollider; // User-defined collider for left boundary
    public Collider2D rightBoundaryCollider; // User-defined collider for right boundary
    //public LayerMask enemyLayer; // Layer to detect other enemies

    private bool isFacingRight = true;
    private bool isIdle = false;//

    private void Update()
    {
        // Update animation parameters
        //animator.SetBool("isMoving", !isIdle && Mathf.Abs(rb.velocity.x) > 0.05f);
        animator.SetBool("isMoving", !isIdle);
    }

    private void FixedUpdate()
    {
        if (isIdle) return;

        // Move the enemy
        rb.velocity = new Vector2((isFacingRight ? speed : -speed), rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Mathf.Log(enemyLayer.value, 2) : " + Mathf.Log(enemyLayer.value, 8));

        if(collision == leftBoundaryCollider || collision == rightBoundaryCollider)
        {
            // Flip direction on hitting the boundaries
            if (!isIdle)
            {
                StartCoroutine(HandleDirectionChange());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision.gameObject.tag : " + collision.gameObject.tag);
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            // Damage the player
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.GetHurt();
        }
        else if (collision.gameObject.tag == "enemy")
        {
            // Detect another enemy and flip direction
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
}
