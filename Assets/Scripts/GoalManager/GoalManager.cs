using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private GameSettings gameSettings;

    [Header("SFX")]
    [SerializeField] public AudioClip audioWin;
    [SerializeField] public float audioWinVolume;

    [Header("WinPanel")]
    [SerializeField] private GameObject winPanel;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI goal1Text;
    [SerializeField] private TextMeshProUGUI goal2Text;
    [SerializeField] private TextMeshProUGUI goal3Text;

    [Header("Images")]
    [SerializeField] private Image goal1Image;
    [SerializeField] private Image goal2Image;
    [SerializeField] private Image goal3Image;
    [SerializeField] private GameObject plus2Image;

    private Fruit goal1,goal2,goal3;
    private int  count1=1,count2,count3;
    private bool isWin=false;

    private void Awake()
    {
        InitilizeGoals();
    }
    private void Update()
    {
        if (!isWin)
        {
            ControlWin();
        }
        
    }

    private void ControlWin()
    {
        if(count1 + count2 + count3 == 0)
        {
            isWin = true;
            SoundFXManager.instance.PlaySoundFXClip(audioWin, transform, audioWinVolume);
            
            if (gameSettings.currentLevelIndex != 2)
            {
                gameSettings.currentLevelIndex++;
            }

            winPanel.SetActive(true);
        }
    }
    public void ControlFruit(Fruit fruit)
    {
        if(fruit.type == goal1.type)
        {
            if (count1 > 0)
            {
                count1--;
                goal1Text.text = count1.ToString();
            }
;
        }else if(fruit.type == goal2.type)
        {
            if (count2 > 0)
            {
                count2--;
                goal2Text.text = count2.ToString();
            }
        }
        else if(goal3 != null)
        {
            if (fruit.type == goal3.type)
            {
                if (count3 > 0)
                {
                    count3--;
                    goal3Text.text = count3.ToString();
                }
            }
        }    
    }
    private void InitilizeGoals()
    {
        if (gameSettings.levels[gameSettings.selectedLevelIndex].goals.Length ==2)
        {
            goal1 = gameSettings.levels[gameSettings.selectedLevelIndex].goals[0].fruitGoal;
            count1 = gameSettings.levels[gameSettings.selectedLevelIndex].goals[0].requiredAmount;
            goal2 = gameSettings.levels[gameSettings.selectedLevelIndex].goals[1].fruitGoal;
            count2 = gameSettings.levels[gameSettings.selectedLevelIndex].goals[1].requiredAmount;

            plus2Image.gameObject.SetActive(false);
            goal3Image.gameObject.SetActive(false);
            goal3Text.gameObject.SetActive(false);
            goal1Image.sprite = goal1.GetComponent<SpriteRenderer>().sprite;
            goal1Text.text = count1.ToString();
            goal2Image.sprite = goal2.GetComponent<SpriteRenderer>().sprite;
            goal2Text.text = count2.ToString();

        }
        else if (gameSettings.levels[gameSettings.selectedLevelIndex].goals.Length == 3)
        {
            goal1 = gameSettings.levels[gameSettings.selectedLevelIndex].goals[0].fruitGoal;
            count1 = gameSettings.levels[gameSettings.selectedLevelIndex].goals[0].requiredAmount;
            goal2 = gameSettings.levels[gameSettings.selectedLevelIndex].goals[1].fruitGoal;
            count2 = gameSettings.levels[gameSettings.selectedLevelIndex].goals[1].requiredAmount;
            goal3 = gameSettings.levels[gameSettings.selectedLevelIndex].goals[2].fruitGoal;
            count3 = gameSettings.levels[gameSettings.selectedLevelIndex].goals[2].requiredAmount;
            goal1Image.sprite = goal1.GetComponent<SpriteRenderer>().sprite;
            goal1Text.text = count1.ToString();
            goal2Image.sprite = goal2.GetComponent<SpriteRenderer>().sprite;
            goal2Text.text = count2.ToString();
            goal3Image.sprite = goal3.GetComponent<SpriteRenderer>().sprite;
            goal3Text.text = count3.ToString();
        }
    }
}


