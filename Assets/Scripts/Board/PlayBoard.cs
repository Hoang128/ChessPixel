﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayBoard : MonoBehaviour
{
    [SerializeField] GameObject lightCellObject;
    [SerializeField] GameObject darkCellObject;
    [SerializeField] GameObject moveMarkObject;
    [SerializeField] GameObject captureMarkObject;
    [SerializeField] GameObject movePromoMarkObject;
    [SerializeField] GameObject capturePromoMarkObject;
    [SerializeField] GameObject lastMoveMarkObject;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject turnText;
    [SerializeField] GameObject movingAnimEffect;

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
    private int chooseMovePhase = 0;
    private bool canRematch = true;
    private bool canUndoMove = false;
    private GameObject movingAnimObj;
    private Stack<Vector2Int> lastPiecePlace;
    private Stack<Vector2Int> lastPieceMove;
    private string[,] startBoardCell = new string[8, 8]
    {
        { "r" , "p", "0", "0", "0", "0", "P", "R" },
        { "kn", "p", "0", "0", "0", "0", "P", "KN"},
        { "b" , "p", "0", "0", "0", "0", "P", "B" },
        { "q" , "p", "0", "0", "0", "0", "P", "Q" },
        { "k" , "p", "0", "0", "0", "0", "P", "K" },
        { "b" , "p", "0", "0", "0", "0", "P", "B" },
        { "kn", "p", "0", "0", "0", "0", "P", "KN"},
        { "r" , "p", "0", "0", "0", "0", "P", "R" }
    };
    private string[,] testBoardCell = new string[8, 8]
    {
        { "0", "P", "0", "0", "0", "0", "p", "0" },
        { "0", "P", "0", "0", "0", "0", "p", "0" },
        { "0", "P", "0", "0", "0", "0", "p", "0" },
        { "0", "P", "k", "0", "0", "0", "p", "0" },
        { "0", "P", "0", "0", "0", "K", "p", "0" },
        { "0", "P", "0", "0", "0", "0", "p", "0" },
        { "0", "P", "0", "0", "0", "0", "p", "0" },
        { "0", "P", "0", "0", "0", "0", "p", "0" }
    };

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
    public GameObject MovePromoMarkObject { get => movePromoMarkObject; set => movePromoMarkObject = value; }
    public GameObject CapturePromoMarkObject { get => capturePromoMarkObject; set => capturePromoMarkObject = value; }
    public GameObject GameCanvas { get => gameCanvas; set => gameCanvas = value; }
    public int ChooseMovePhase { get => chooseMovePhase; set => chooseMovePhase = value; }
    public GameObject TurnText { get => turnText; set => turnText = value; }
    public bool CanRematch { get => canRematch; set => canRematch = value; }
    public bool CanUndoMove { get => canUndoMove; set => canUndoMove = value; }
    public GameObject MovingAnimEffect { get => movingAnimEffect; set => movingAnimEffect = value; }
    public GameObject MovingAnimObj { get => movingAnimObj; set => movingAnimObj = value; }
    public Stack<Vector2Int> LastPiecePlace { get => lastPiecePlace; set => lastPiecePlace = value; }
    public Stack<Vector2Int> LastPieceMove { get => lastPieceMove; set => lastPieceMove = value; }
    public GameObject LastMoveMarkObject { get => lastMoveMarkObject; set => lastMoveMarkObject = value; }

    // Start is called before the first frame update
    void Start()
    {
        movingAnimObj = null;
        PlayerMgr playerComp;
        playerWhiteMgr = GameObject.Instantiate(playerWhite);
        playerComp = playerWhiteMgr.GetComponent<PlayerMgr>();
        if (playerComp != null)
            playerWhiteMgr.GetComponent<PlayerMgr>().IsWhite = true;
        else
            playerWhiteMgr.GetComponent<AIMgr>().WhiteSide = true;
        playerBlackMgr = GameObject.Instantiate(playerBlack);
        playerComp = playerBlackMgr.GetComponent<PlayerMgr>();
        if (playerComp != null)
            playerBlackMgr.GetComponent<PlayerMgr>().IsWhite = false;
        else
            playerBlackMgr.GetComponent<AIMgr>().WhiteSide = false;

        InitPlayBoard();
        InitUIBoard();
        UpdateUIBoard();

        canRematch = true;
        canUndoMove = false;

        stateMachine = new StateMachine();
        stateMachine.StateChange(new BoardStateIdle(stateMachine, this));

        clickPoint = new Vector2Int(-1, -1);
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
        boardStack.Peek().UpdateCells(startBoardCell);

        lastPieceMove = new Stack<Vector2Int>();
        lastPiecePlace = new Stack<Vector2Int>();
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

    public void UndoMove()
    {
        if (boardStack.Count > 2)
        {
            boardStack.Pop();
            boardStack.Pop();

            lastPieceMove.Pop();
            lastPieceMove.Pop();

            lastPiecePlace.Pop();
            lastPiecePlace.Pop();

            UpdateUIBoard();

            GameObject[] lastMoveMarks = GameObject.FindGameObjectsWithTag("Last Move Marker");
            if ((lastMoveMarks.Length != 0) && (lastMoveMarks != null))
            {
                foreach (GameObject mark in lastMoveMarks)
                {
                    GameObject.Destroy(mark);
                }
            }

            Vector2 startPosition = transform.position;
            if (lastPieceMove.Count > 0)
            {
                Vector2Int cellPos = lastPieceMove.Peek();
                GameObject.Instantiate(lastMoveMarkObject, startPosition + new Vector2(cellPos.x * cellSize, cellPos.y * cellSize), transform.rotation);
            }

            if (lastPiecePlace.Count > 0)
            {
                Vector2Int cellPos = lastPiecePlace.Peek();
                GameObject.Instantiate(lastMoveMarkObject, startPosition + new Vector2(cellPos.x * cellSize, cellPos.y * cellSize), transform.rotation);
            }
        }
    }

    public void ResetBoard()
    {
        boardStack.Clear();
        lastPieceMove.Clear();
        lastPiecePlace.Clear();

        InitPlayBoard();
        UpdateUIBoard();

        Board startBoard = new Board();
        startBoard.BoardCells = startBoardCell;

        UpdatePlayBoard(startBoard);
        UpdateUIBoard();

        stateMachine.StateChange(new BoardStateIdle(stateMachine, this));
    }

    private void OnDrawGizmos()
    {
        //Handles.Label(transform.position + new Vector3(0f, -8f, 0f), "white turn = " + WhiteTurn);
        //Handles.Label(transform.position + new Vector3(0f, -10f, 0f), "evaluation = " + BoardStack.Peek().Evaluation);
        //Handles.Label(transform.position + new Vector3(0f, -12f, 0f), "click point = " + clickPoint.x + ", " + clickPoint.y);
    }
}
