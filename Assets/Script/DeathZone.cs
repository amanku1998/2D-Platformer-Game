using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public GameOverController gameOverController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            //RestartGame();
            other.gameObject.GetComponent<PlayerController>().enabled = false;
            gameOverController.PlayerDead();
        }
    }

    private void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
