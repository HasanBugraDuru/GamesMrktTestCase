using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int levelIndex = 1;
    [SerializeField] private GameSettings gameSettings;

    public void PlayLevel()
    {
        gameSettings.currentLevelIndex = levelIndex;
        SceneManager.LoadScene("Game");
    }

}
