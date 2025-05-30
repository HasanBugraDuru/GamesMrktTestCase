using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData 
{
    public int width;
    public int height;
    public int moveLimit;
    public float timeLimit;

    [System.Serializable]
    public struct FruitGoal
    {
        public Fruit fruitGoal;
        public int requiredAmount;
    }

    public FruitGoal[] goals;
}
