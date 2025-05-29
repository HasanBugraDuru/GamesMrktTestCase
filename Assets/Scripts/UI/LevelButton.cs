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
        gameSettings.selectedLevelIndex = levelIndex;
        SceneManager.LoadScene("Game");
    }
    public void PlayNextLevel()
    {
        gameSettings.selectedLevelIndex++;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene("Game");
    }
}
