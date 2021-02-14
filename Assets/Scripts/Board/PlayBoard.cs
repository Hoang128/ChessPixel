using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBoard : MonoBehaviour
{
    [SerializeField] GameObject lightCellObject;
    [SerializeField] GameObject darkCellObject;
    [SerializeField] float cellSize = 5f;

    struct Coordinate
    {
        public float x;
        public float y;
    }

    private StateMachine stateMachine;
    private Stack<Board> boardStack;
    private GameObject[,] cellUI; 

    public Stack<Board> BoardStack { get => boardStack; set => boardStack = value; }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.StateChange(new BoardStateIdle(stateMachine, this));
        InitPlayBoard();
        InitUIBoard();

        string[,] startCell = new string[8, 8]
        {
            { "r", "kn", "b", "k", "q", "b", "kn", "r" },
            { "p", "p" , "p", "p", "p", "p", "p" , "p" },
            { "0", "0" , "0", "0", "0", "0", "0" , "0" },
            { "0", "0" , "0", "0", "0", "0", "0" , "0" },
            { "0", "0" , "0", "0", "0", "0", "0" , "0" },
            { "0", "0" , "0", "0", "0", "0", "0" , "0" },
            { "P", "P" , "P", "P", "P", "P", "P" , "P" },
            { "R", "KN", "B", "K", "Q", "B", "KN", "R" }
        };

        Board startBoard = new Board(0, startCell);

        UpdatePlayBoard(startBoard);
        UpdateUIBoard();
        boardStack.Peek().Log();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Coordinate relativeCreatePos;
        relativeCreatePos.x = 0f;
        relativeCreatePos.y = 0f;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                relativeCreatePos.x = i * cellSize;
                relativeCreatePos.y = j * cellSize;
                Vector3 createPos = transform.position + new Vector3(relativeCreatePos.x, relativeCreatePos.y, 0f);
                if (i % 2 == 0)
                {
                    if (j % 2 == 0)
                        cellUI[j, i] = Instantiate(darkCellObject, createPos, transform.rotation);
                    else
                        cellUI[j, i] = Instantiate(lightCellObject, createPos, transform.rotation);
                }
                else
                {
                    if (j % 2 == 0)
                        cellUI[j, i] = Instantiate(lightCellObject, createPos, transform.rotation);
                    else
                        cellUI[j, i] = Instantiate(darkCellObject, createPos, transform.rotation);
                }

                cellUI[j, i].GetComponent<Cell>().SetCoor(j, i);
            }
        }
    }
}
