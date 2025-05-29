using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 8;
    public int height = 8;
    public float tileSize = 1.0f;

    [Header("Game Settings")]
    [SerializeField] private GameSettings gameSettings;

    [Header("SFX")]
    [SerializeField] public AudioClip audioBreak;
    [SerializeField] public float audioBreakVolume;

    [Header("Tile Prefabs")]
    public GameObject tileLightPrefab;
    public GameObject tileDarkPrefab;

    public MatchManager matchManager;

    [Header("Fruit Prefabs")]
    public Fruit[] fruitPrefabs;

   [HideInInspector] public Fruit[,] allFruits;
    private Vector2[,] fruitPositions;
    public float MoveSpeed;

    public enum BoardState 
    {
        waiting,
        canMove 
    };
    public BoardState validState = BoardState.canMove;
     

    private void Awake()
    {
        width = gameSettings.levels[gameSettings.selectedLevelIndex].width;
        height = gameSettings.levels[gameSettings.selectedLevelIndex].height;
        matchManager = Object.FindObjectOfType<MatchManager>();
    }
    void Start()
    {
        fruitPositions = new Vector2[width, height];
        allFruits = new Fruit[width, height];
        
        GenerateGrid();
    }


    #region Create Region
    public void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 spawnPos = new Vector2(x * tileSize, y * tileSize);

                
                GameObject prefabToUse = (x + y) % 2 == 0 ? tileLightPrefab : tileDarkPrefab;
                GameObject tile = Instantiate(prefabToUse, spawnPos, Quaternion.identity, transform);
                tile.name = $"Tile_{x}_{y}";

                int randomFruit = Random.Range(0,fruitPrefabs.Length);

                int controlFlag = 0;
                while (IsThereAnyMatch(new Vector2Int(x, y), fruitPrefabs[randomFruit]) && controlFlag < 100)
                {
                    randomFruit = Random.Range(0, fruitPrefabs.Length);
                    controlFlag++;
                }

                CreateFruit(new Vector2Int(x,y), fruitPrefabs[randomFruit]);
            }
        }

       CenterGrid();
       CorretFruitPosition();
       validState = BoardState.canMove;    
    }

    private void CenterGrid()
    {
        float offsetX = (width - 1) * tileSize / 2f;
        float offsetY = (height - 1) * tileSize;
        transform.localPosition = new Vector3(-offsetX, -offsetY, 0f);
    }

    private void CreateFruit(Vector2Int pos,Fruit fruit)
    {
        Fruit fruit1 = Instantiate (fruit, new Vector3(pos.x, pos.y ,0f),Quaternion.identity, transform);
        fruit1.name = "Fruit" +pos.x +", "+ pos.y;

        allFruits[pos.x,pos.y] = fruit1;
        fruit1.ArrangeTheFruit(pos, this);
    }

    private void CorretFruitPosition()
    {
        Fruit fruit;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                fruit = allFruits[x, y];
                fruit.fruitPosition = fruit.transform.position;
                fruitPositions[x,y] = fruit.transform.position;
            }
        }
    }
    #endregion

    #region Control Region
    private bool IsThereAnyMatch(Vector2Int controlPos, Fruit ctrlFruit)
    {
        if (controlPos.x > 1)
        {
            if (allFruits[controlPos.x - 1, controlPos.y].type == ctrlFruit.type &&
                allFruits[controlPos.x - 2, controlPos.y].type == ctrlFruit.type)
            {
                return true;
            }
        }
        if (controlPos.y > 1)
        {
            if (allFruits[controlPos.x, controlPos.y - 1].type == ctrlFruit.type &&
                allFruits[controlPos.x, controlPos.y - 2].type == ctrlFruit.type)
            {
                return true;
            }
        }
        return false;
    }
    private void ControlMisplacement()
    {
        List<Fruit> FoundedFruitsList = new List<Fruit>();

        FoundedFruitsList.AddRange(FindObjectsOfType<Fruit>());
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (FoundedFruitsList.Contains(allFruits[x, y]))
                {
                    FoundedFruitsList.Remove(allFruits[x, y]);
                }
            }
        }

        foreach (Fruit fruit in FoundedFruitsList)
        {
            Destroy(fruit.gameObject);
        }
    }
    #endregion

    #region Delete Region
    private void DeleteMatchedFruit(Vector2Int pos)
    {
        if (allFruits[pos.x,pos.y] != null) 
        {
            if (allFruits[pos.x, pos.y].isMatch)
            {
                SoundFXManager.instance.PlaySoundFXClip(audioBreak, transform, audioBreakVolume);
                ParticleEffectsManager.instance.PlayBreakingEffect(allFruits[pos.x, pos.y].transform.position, allFruits[pos.x, pos.y].particleColor);
                Destroy(allFruits[pos.x, pos.y].gameObject);
                allFruits[pos.x, pos.y] = null;
            }
        }
    }
    public void DeleteAllMatcheds()
    {
        for (int i = 0; i < matchManager.MatchedFruitsList.Count; i++)
        {
            if (matchManager.MatchedFruitsList[i] != null)
            {
                DeleteMatchedFruit(matchManager.MatchedFruitsList[i].posIndex);
            }
        }
        StartCoroutine(MoveDownFruitsRouitine());
    }
    #endregion

    #region Move Region
    IEnumerator MoveDownFruitsRouitine()
    {
        yield return new WaitForSeconds(0.2f);

        int emptyCount = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allFruits[x, y] == null)
                {
                    emptyCount++;
                }else if(emptyCount > 0)
                {
                    allFruits[x, y].posIndex.y -= emptyCount;
                    allFruits[x, y- emptyCount] = allFruits[x,y];
                    allFruits[x, y- emptyCount].fruitPosition = fruitPositions[x, y - emptyCount];
                    allFruits[x,y] = null;
                }
            }
            emptyCount = 0;
        }
        StartCoroutine(FillBoardRoutine());
    }
    IEnumerator FillBoardRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        FillUpperGaps();

        yield return new WaitForSeconds(0.25f);

        matchManager.FindMatches();
        if(matchManager.MatchedFruitsList.Count > 0)
        {
            yield return new WaitForSeconds(0.75f);
            DeleteAllMatcheds();
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            validState = BoardState.canMove;
        }
    } 
    
    private void FillUpperGaps()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allFruits[x,y] == null)
                {
                    int randomFruit = Random.Range(0, fruitPrefabs.Length);
                    CreateFruit(new Vector2Int(x, y), fruitPrefabs[randomFruit]);
                    allFruits[x,y].fruitPosition = fruitPositions[x,y]; 
                }
            }
        }
        ControlMisplacement();
    }
    #endregion
}