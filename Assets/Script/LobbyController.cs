using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public Button buttonPlay;
    public Button buttonQuit;
    public Button buttonBack;
    public GameObject levelSelection;

    private void Awake()
    {
        buttonPlay.onClick.AddListener(PlayGame);
    }

    private void PlayGame()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        //SceneManager.LoadScene(1);
        levelSelection.SetActive(true);
    }

    private void QuitGame()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        //SceneManager.LoadScene(1);
        levelSelection.SetActive(true);
    }
}
