using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLevel : MonoBehaviour
{
    [SerializeField] private GameObject levelCompletedPopup;

    [SerializeField] private Animator animator;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Level finished by the player");
            LevelManager.Instance.MarkCurrentLevelCompleted();

            collision.gameObject.GetComponent<PlayerController>().GetPlayerAnimator().SetFloat("Speed", 0);
            collision.gameObject.GetComponent<PlayerController>().enabled = false;

            // Update animation parameters
            animator.SetTrigger("IsPlayer");
        }
    }

    public void ActivatelevelCompletePanel()
    {
        levelCompletedPopup.SetActive(true);
    }
}
