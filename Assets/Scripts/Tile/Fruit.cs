using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Fruit : MonoBehaviour
{
   [Header("SFX")]
   [SerializeField] public AudioClip audioSlide;
   [SerializeField] public float audioSlideVolume;

   [Header("SFX")]
   public Color particleColor;

   [HideInInspector] public Vector2Int posIndex;
   [HideInInspector] public Board board;

   [HideInInspector] public Vector2 firstClickPoint;
   [HideInInspector] public Vector2 lastClickPoint;
   [HideInInspector] public Vector2 fruitPosition;
   [HideInInspector]  public bool isMatch;
 
   private bool isMousePressed;
   private float dragAngle;
   private Vector2Int FirstIndex;
   private Vector2 FirstPos;
   private Fruit touchedFruit;

    public enum FruitType
    {
        Apple,
        Banana,
        Orange,
        BlueBerry,
        Grapes
    }
    public FruitType type;

    private void Update()
    {
        if (Vector2.Distance(transform.position,fruitPosition) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, fruitPosition, board.MoveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position= new Vector3 (fruitPosition.x, fruitPosition.y, 0);
        }

        if(isMousePressed && Input.GetMouseButtonUp(0))
        {
            isMousePressed = false;
            if (board.validState == Board.BoardState.canMove)
            {
                lastClickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateAngle();
            } 
        }
    }
    public void ArrangeTheFruit(Vector2Int pos , Board _board)
    {
        posIndex = pos;
        board = _board;
    }

    private void OnMouseDown()
    {
        if(board.validState == Board.BoardState.canMove)
        {
            firstClickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isMousePressed = true;
        }
    }

    //Calculating The Angle Between Two Fruits
    private void CalculateAngle() 
    {
        float dx = lastClickPoint.x - firstClickPoint.x;
        float dy = lastClickPoint.y - firstClickPoint.y;

        dragAngle = Mathf.Atan2(dy, dx);
        dragAngle = dragAngle * 180 / Mathf.PI;

        if(Vector3.Distance(firstClickPoint,lastClickPoint)> 0.5f)
        {
            MoveFruit();
        }
    }

    //Moving Fruit 
    private void MoveFruit()
    {
        FirstIndex = posIndex;
        FirstPos = fruitPosition;
        Vector2 tempPosition;
        if ( dragAngle < 45 &&  dragAngle > -45 && posIndex.x < board.width -1) 
        {
            touchedFruit = board.allFruits[posIndex.x + 1,posIndex.y];
            touchedFruit.posIndex.x--;
            posIndex.x++;
            tempPosition = touchedFruit.fruitPosition;
            touchedFruit.fruitPosition = fruitPosition;
            fruitPosition = tempPosition;
        }
        else if (dragAngle > 45 && dragAngle <= 135 && posIndex.y < board.height - 1)
        {
            touchedFruit = board.allFruits[posIndex.x, posIndex.y + 1];
            touchedFruit.posIndex.y--;
            posIndex.y++;
            tempPosition = touchedFruit.fruitPosition;
            touchedFruit.fruitPosition = fruitPosition;
            fruitPosition = tempPosition;
        }
        else if (dragAngle < -45 && dragAngle >= -135 && posIndex.y > 0)
        {
            touchedFruit = board.allFruits[posIndex.x , posIndex.y - 1];
            touchedFruit.posIndex.y++;
            posIndex.y--;
            tempPosition = touchedFruit.fruitPosition;
            touchedFruit.fruitPosition = fruitPosition;
            fruitPosition = tempPosition;
        }
        else if (dragAngle > 135 || dragAngle > -135 && posIndex.x >0)
        {
            touchedFruit = board.allFruits[posIndex.x -1, posIndex.y];
            touchedFruit.posIndex.x++;
            posIndex.x--;
            tempPosition = touchedFruit.fruitPosition;
            touchedFruit.fruitPosition = fruitPosition;
            fruitPosition = tempPosition;
        }

        board.allFruits[posIndex.x, posIndex.y] = this;
        board.allFruits[touchedFruit.posIndex.x, touchedFruit.posIndex.y] = touchedFruit;
        SoundFXManager.instance.PlaySoundFXClip(audioSlide, transform, audioSlideVolume);
        StartCoroutine(ControlMoveRouitne());
    }

    // If There is No Match After Moving Fruits
    public IEnumerator ControlMoveRouitne()
    {
        board.validState = Board.BoardState.waiting;

        yield return new WaitForSeconds(0.3f);
        board.matchManager.FindMatches();

        if(touchedFruit != null)
        {
            if (!isMatch && !touchedFruit.isMatch)
            {
                touchedFruit.posIndex = posIndex;
                posIndex = FirstIndex;
                touchedFruit.fruitPosition = fruitPosition;
                fruitPosition = FirstPos;


                board.allFruits[posIndex.x, posIndex.y] = this;
                board.allFruits[touchedFruit.posIndex.x, touchedFruit.posIndex.y] = touchedFruit;

                yield return new WaitForSeconds(0.2f);

                board.validState = Board.BoardState.canMove;
            }
            else
            {
                board.DeleteAllMatcheds();
            }
        }
    }
    private SlideFruit()
    {

    }
}
