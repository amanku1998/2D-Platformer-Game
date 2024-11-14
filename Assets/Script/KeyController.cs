using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] private Animator keyAnimator;
    private bool isCollected = false;   // Flag to ensure item is collected only once

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCollected && collision.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.PickUpKey();

            keyAnimator.SetTrigger("isPlayerTrigger");      
        }
    }

    public void TriggerFadeOut()
    {
        Destroy(gameObject);
    }
}
