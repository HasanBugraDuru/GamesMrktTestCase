using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Match3/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Levels")]
    public LevelData[] levels;
    public int currentLevelIndex = 1;
    public int selectedLevelIndex = 1;

    [Header("Audio")]
    [Range(0f, 1f)]
    public float musicVolume = 1f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;
}
