using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOverController : MonoBehaviour
{
    [SerializeField] private GameObject levelCompletedPopup;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            SoundManager.Instance.Play(Sounds.PlayerVictory);
            Debug.Log("Level finished by the player");
            LevelManager.Instance.MarkCurrentLevelCompleted();

            levelCompletedPopup.SetActive(true);
        }
    }
}
