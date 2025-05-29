using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MatchManager : MonoBehaviour
{
    Board board;

    public List<Fruit> MatchedFruitsList = new List<Fruit>();

    private void Awake()
    {
        board = Object.FindObjectOfType<Board>();
    }

    public void FindMatches()
    {
        MatchedFruitsList.Clear();

        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y <board.height; y++)
            {
                Fruit validFruit = board.allFruits[x,y];

                if(validFruit != null)
                {
                    if(x >0  && x < board.width - 1) 
                    {
                        Fruit leftFruit = board.allFruits[x-1,y];
                        Fruit rightFruit = board.allFruits[x+1,y];
                        
                        if (leftFruit != null && rightFruit  != null)
                        {
                            if(leftFruit.type == validFruit.type && rightFruit.type == validFruit.type)
                            {
                                validFruit.isMatch = true;
                                leftFruit.isMatch = true;
                                rightFruit.isMatch = true;
                                MatchedFruitsList.Add(validFruit);
                                MatchedFruitsList.Add(leftFruit);
                                MatchedFruitsList.Add(rightFruit);
                            }
                        }
                    }

                    if (y > 0 && y < board.height - 1)
                    {
                        Fruit downFruit = board.allFruits[x , y - 1];
                        Fruit upFruit = board.allFruits[x , y + 1];

                        if (downFruit != null && upFruit != null)
                        {
                            if (downFruit.type == validFruit.type && upFruit.type == validFruit.type)
                            {
                                validFruit.isMatch = true;
                                downFruit.isMatch = true;
                                upFruit.isMatch = true;
                                MatchedFruitsList.Add(validFruit);
                                MatchedFruitsList.Add(upFruit);
                                MatchedFruitsList.Add(downFruit);
                            }
                        }
                    }
                }
            }
        }
        if(MatchedFruitsList.Count > 0)
        {
            MatchedFruitsList = MatchedFruitsList.Distinct().ToList();
        }

    }
}
