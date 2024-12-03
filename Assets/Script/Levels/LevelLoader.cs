using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class LevelLoader : MonoBehaviour
{
    private Button button;
    public string levelName;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(onClick);

        LevelStatus levelStatus = LevelManager.Instance.GetLevelStatus(levelName);

        if (levelStatus == LevelStatus.Locked)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }

    }

    private void onClick()
    {
        LevelStatus levelStatus = LevelManager.Instance.GetLevelStatus(levelName);
        SoundManager.Instance.Play(Sounds.ButtonPlay);

        switch (levelStatus)
        {
            case LevelStatus.Locked:
                Debug.Log("Can't play this level till you unlock it");
                break;
            case LevelStatus.Unlocked:
                SceneManager.LoadScene(levelName);
                break;
            case LevelStatus.Completed:
                SceneManager.LoadScene(levelName);
                break;
            default:
                break;
        }

    }
}
