using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayBoard : MonoBehaviour
{
    [SerializeField] GameObject lightCellObject;
    [SerializeField] GameObject darkCellObject;
    [SerializeField] GameObject moveMarkObject;
    [SerializeField] GameObject captureMarkObject;

    [SerializeField] GameObject playerWhite;
    [SerializeField] GameObject playerBlack;
    [SerializeField] float cellSize = 5f;

    private StateMachine stateMachine;
    private Stack<Board> boardStack;
    private GameObject[,] cellUI;
    private Vector2Int clickPoint;
    private string stateName = null;
    private bool whiteTurn = true;
    private GameObject playerWhiteMgr;
    private GameObject playerBlackMgr;

    public Stack<Board> BoardStack { get => boardStack; set => boardStack = value; }
    public Vector2Int ClickPoint { get => clickPoint; set => clickPoint = value; }
    public float CellSize { get => cellSize; set => cellSize = value; }
    public bool WhiteTurn { get => whiteTurn; set => whiteTurn = value; }
    public GameObject PlayerWhite { get => playerWhite; set => playerWhite = value; }
    public GameObject PlayerBlack { get => playerBlack; set => playerBlack = value; }
    public GameObject MoveMarkObject { get => moveMarkObject; set => moveMarkObject = value; }
    public GameObject CaptureMarkObject { get => captureMarkObject; set => captureMarkObject = value; }
    public string StateName { get => stateName; set => stateName = value; }
    public GameObject PlayerWhiteMgr { get => playerWhiteMgr; set => playerWhiteMgr = value; }
    public GameObject PlayerBlackMgr { get => playerBlackMgr; set => playerBlackMgr = value; }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.StateChange(new BoardStateIdle(stateMachine, this));
        InitPlayBoard();
        InitUIBoard();

        playerWhiteMgr = GameObject.Instantiate(playerWhite);
        playerWhiteMgr.GetComponent<PlayerMgr>().IsWhite = true;
        playerBlackMgr = GameObject.Instantiate(playerBlack);
        playerBlackMgr.GetComponent<PlayerMgr>().IsWhite = false;

        string[,] startCell = new string[8, 8]
        {
            { "r" , "p", "0", "0", "0", "0", "P", "R" },
            { "kn", "p", "0", "0", "0", "0", "P", "KN" },
            { "b" , "p", "0", "0", "0", "0", "P", "B" },
            { "q" , "p", "0", "0", "0", "0", "P", "Q" },
            { "k" , "p", "0", "0", "0", "0", "P", "K" },
            { "b" , "p", "0", "0", "0", "0", "P", "B" },
            { "kn", "p", "0", "0", "0", "0", "P", "KN" },
            { "r" , "p", "0", "0", "0", "0", "P", "R" }
        };

        Board startBoard = new Board(0, startCell);

        UpdatePlayBoard(startBoard);
        UpdateUIBoard();

        clickPoint.x = -1;
        clickPoint.y = -1;
        boardStack.Peek().Log();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.StateHandle();
    }

    public void UpdateBoard(Board newBoard)
    {
        UpdatePlayBoard(newBoard);
        UpdateUIBoard();
    }

    void UpdatePlayBoard(Board newBoard)
    {
        boardStack.Push(newBoard);
    }

    void UpdateUIBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                cellUI[i, j].GetComponent<Cell>().Piece = boardStack.Peek().BoardCells[i, j];
                cellUI[i, j].GetComponentInChildren<Piece>().UpdatePiece(boardStack.Peek().BoardCells[i, j]);
            }
        }
    }

    void InitPlayBoard()
    {
        boardStack = new Stack<Board>();
        Board startBoard = new Board();
        boardStack.Push(startBoard);
    }

    public void InitUIBoard()
    {
        cellUI = new GameObject[8, 8];
        Vector2Int relativeCreatePos = new Vector2Int(0, 0);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                relativeCreatePos.x = i * (int)cellSize;
                relativeCreatePos.y = j * (int)cellSize;
                Vector3 createPos = transform.position + new Vector3(relativeCreatePos.x, relativeCreatePos.y, 0f);
                if (i % 2 == 0)
                {
                    if (j % 2 == 0)
                        cellUI[i, j] = Instantiate(darkCellObject, createPos, transform.rotation);
                    else
                        cellUI[i, j] = Instantiate(lightCellObject, createPos, transform.rotation);
                }
                else
                {
                    if (j % 2 == 0)
                        cellUI[i, j] = Instantiate(lightCellObject, createPos, transform.rotation);
                    else
                        cellUI[i, j] = Instantiate(darkCellObject, createPos, transform.rotation);
                }

                cellUI[i, j].GetComponent<Cell>().Coor = new Vector2Int(i, j);
                cellUI[i, j].GetComponent<Cell>().Piece = boardStack.Peek().BoardCells[i, j];
                cellUI[i, j].GetComponentInChildren<Piece>().UpdatePiece(boardStack.Peek().BoardCells[i, j]);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position + new Vector3(0f, -8f, 0f), "white turn = " + WhiteTurn);
        Handles.Label(transform.position + new Vector3(0f, -10f, 0f), "evaluation = " + BoardStack.Peek().Evaluation);
        Handles.Label(transform.position + new Vector3(0f, -12f, 0f), "click point = " + clickPoint.x + ", " + clickPoint.y);
    }
}
