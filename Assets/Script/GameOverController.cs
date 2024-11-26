using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public Button buttonRestart;

    private void Awake()
    {
        buttonRestart.onClick.AddListener(ReloadLevel);
    }

    public void PlayerDead()
    {
        gameObject.SetActive(true);
    }

    public void ReloadLevel()
    {
        Physics2D.IgnoreLayerCollision(7, 8, false);
        SceneManager.LoadScene(1);
    }
}
