using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompleteController : MonoBehaviour
{
    public Button buttonNextLevel;
    public Button buttonMainMenu;
    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex != 5)
        {
            buttonNextLevel.onClick.AddListener(NextLevel);
        }else {
            buttonNextLevel.gameObject.SetActive(false);
        }
        buttonMainMenu.onClick.AddListener(MainMenu);

    }

    public void NextLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex + 1);
    }

    public void MainMenu()
    {
        //Physics2D.IgnoreLayerCollision(7, 8, false);
        SceneManager.LoadScene(0);
    }
}
