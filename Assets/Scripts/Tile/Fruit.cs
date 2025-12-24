using System.Collections;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] public AudioClip audioSlide;
    [SerializeField] public float audioSlideVolume;

    [Header("Particle")]
    public Color particleColor;

    [HideInInspector] public Vector2Int posIndex;
    [HideInInspector] public Board board;

    [HideInInspector] public Vector2 firstClickPoint;
    [HideInInspector] public Vector2 lastClickPoint;
    [HideInInspector] public Vector2 fruitPosition;
    [HideInInspector] public bool isMatch;

    private bool isMousePressed;
    private Vector2Int lastTargetIndex;
    private bool hasSlid = false;

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
        if (Vector2.Distance(transform.position, fruitPosition) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, fruitPosition, board.MoveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(fruitPosition.x, fruitPosition.y, 0);
        }

        if (isMousePressed && Input.GetMouseButtonUp(0))
        {
            isMousePressed = false;

            if (hasSlid)
            {
                StartCoroutine(ControlMoveRoutine());
            }
        }
    }

    public void ArrangeTheFruit(Vector2Int pos, Board _board)
    {
        posIndex = pos;
        board = _board;
    }

    private void OnMouseDown()
    {
        if (board.validState == Board.BoardState.canMove)
        {
            firstClickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isMousePressed = true;
            lastTargetIndex = posIndex;
            hasSlid = false;
        }
    }

    private void OnMouseDrag()
    {
        if (!isMousePressed || board.validState != Board.BoardState.canMove) return;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int targetX = Mathf.Clamp(Mathf.RoundToInt(worldPos.x / board.tileSize), 0, board.width - 1);
        int targetY = Mathf.Clamp(Mathf.RoundToInt(worldPos.y / board.tileSize), 0, board.height - 1);

        Vector2Int targetIndex = new Vector2Int(targetX, targetY);

        if (targetIndex == lastTargetIndex) return;

        if (targetIndex.x == posIndex.x || targetIndex.y == posIndex.y)
        {
            SlideTo(targetIndex);
            lastTargetIndex = targetIndex;
            hasSlid = true;
        }
    }

    private void SlideTo(Vector2Int targetIndex)
    {
        board.validState = Board.BoardState.waiting;

        if (targetIndex.y == posIndex.y)
        {
            int delta = targetIndex.x - posIndex.x;
            ShiftRow(delta);
        }
        else if (targetIndex.x == posIndex.x)
        {
            int delta = targetIndex.y - posIndex.y;
            ShiftColumn(delta);
        }

        SoundFXManager.instance.PlaySoundFXClip(audioSlide, transform, audioSlideVolume);
    }

    private void ShiftRow(int delta)
    {
        int y = posIndex.y;
        Fruit[] row = new Fruit[board.width];
        for (int x = 0; x < board.width; x++)
            row[x] = board.allFruits[x, y];

        Fruit[] newRow = new Fruit[board.width];
        for (int x = 0; x < board.width; x++)
        {
            int newX = (x + delta + board.width) % board.width;
            newRow[newX] = row[x];
            newRow[newX].posIndex.x = newX;
            newRow[newX].fruitPosition = board.fruitPositions[newX, y];
        }

        for (int x = 0; x < board.width; x++)
            board.allFruits[x, y] = newRow[x];
    }

    private void ShiftColumn(int delta)
    {
        int x = posIndex.x;
        Fruit[] column = new Fruit[board.height];
        for (int y = 0; y < board.height; y++)
            column[y] = board.allFruits[x, y];

        Fruit[] newColumn = new Fruit[board.height];
        for (int y = 0; y < board.height; y++)
        {
            int newY = (y + delta + board.height) % board.height;
            newColumn[newY] = column[y];
            newColumn[newY].posIndex.y = newY;
            newColumn[newY].fruitPosition = board.fruitPositions[x, newY];
        }

        for (int y = 0; y < board.height; y++)
            board.allFruits[x, y] = newColumn[y];
    }

    public IEnumerator ControlMoveRoutine()
    {
        yield return new WaitForSeconds(0.3f);

        board.matchManager.FindMatches();

        if (!isMatch)
        {
            board.validState = Board.BoardState.canMove;
        }
        else
        {
            board.DeleteAllMatcheds();
        }
    }
}
