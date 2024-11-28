using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public Button buttonRestart;
    public Button buttonQuit;

    private void Awake()
    {
        buttonRestart.onClick.AddListener(ReloadLevel);
        buttonQuit.onClick.AddListener(QuitLevel);
    }

    public void PlayerDead()
    {
        gameObject.SetActive(true);
    }

    public void ReloadLevel()
    {
        Physics2D.IgnoreLayerCollision(7, 8, false);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    } 
    
    public void QuitLevel()
    {
        Physics2D.IgnoreLayerCollision(7, 8, false);
        SceneManager.LoadScene(0);
    }
}
