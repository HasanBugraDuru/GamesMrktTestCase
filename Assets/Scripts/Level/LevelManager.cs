using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private Button level1;
    [SerializeField] private Button level2;
    [SerializeField] private Button level3;


    private void Awake()
    {
        if(gameSettings.currentLevelIndex == 0)
        {
            level1.interactable = true;
        }
        else if (gameSettings.currentLevelIndex == 1)
        {
            level1.interactable = true;
            level2.interactable = true;
        }
        else if (gameSettings.currentLevelIndex == 2)
        {
            level1.interactable = true;
            level2.interactable = true;
            level3.interactable = true;
        }
    }
}
