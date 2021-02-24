using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RuleHandler;

public class AIMgr : MonoBehaviour
{
    private Vector2Int finalPiecePlace;
    private Vector2Int finalMovePlace;
    private string finalPieceChange;
    private Stack<RuleHandler.MoveList> allMoveList;
    private bool whiteSide = true;
    private const double MAX_EVALUATION = 100000;
    private const double MIN_EVALUATION = -100000;
    private bool canMove = false;

    private GameObject boardController;

    public Vector2Int FinalPiecePlace { get => finalPiecePlace; set => finalPiecePlace = value; }
    public Vector2Int FinalMovePlace { get => finalMovePlace; set => finalMovePlace = value; }
    public string FinalPieceChange { get => finalPieceChange; set => finalPieceChange = value; }
    public Stack<MoveList> AllMoveList { get => allMoveList; set => allMoveList = value; }
    public bool WhiteSide { get => whiteSide; set => whiteSide = value; }
    public bool CanMove { get => canMove; set => canMove = value; }

    // Start is called before the first frame update
    void Awake()
    {
        finalPiecePlace = new Vector2Int(-1, -1);
        finalMovePlace = new Vector2Int(-1, -1);
        finalPieceChange = "0";
        boardController = GameObject.Find("PlayBoard");
    }

    void Start()
    {
        FindBestMove(boardController.GetComponent<PlayBoard>().BoardStack.Peek(),whiteSide);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindBestMove(Board currentBoard, bool whiteSide)
    {
        FindBestMoveGreedy(currentBoard, whiteSide);
    }

    void FindBestMoveGreedy(Board currentBoard, bool whiteSide)
    {
        double bestEvaluation = 0;
        Vector2Int kingPlace = new Vector2Int(-1, -1);
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (whiteSide)
                {
                    if (currentBoard.BoardCells[i, j] == "k")
                    {
                        kingPlace.x = i;
                        kingPlace.y = j;
                        break;
                    }
                }
                else
                {
                    if (currentBoard.BoardCells[i, j] == "K")
                    {
                        kingPlace.x = i;
                        kingPlace.y = j;
                        break;
                    }
                }
                
            }
        }

        if (whiteSide)
            bestEvaluation = MIN_EVALUATION;
        else
            bestEvaluation = MAX_EVALUATION;

        Stack<MoveList> allMoves = new Stack<MoveList>();
        allMoves = GetAllMoveInBoard(currentBoard, whiteSide);
        Debug.Log("moveable pieces = " + allMoves.Count);
        if (allMoves.Count == 0)
        {

            if (IsCheckedCell(kingPlace, currentBoard, whiteSide))
            {
                Debug.Log("Checkmate");
                Debug.Log("AI Lose");
                return;
            }
            else
            {
                Debug.Log("Can't move");
                Debug.Log("AI Draw");
                return;
            }
        }
        while(allMoves.Count != 0)
        {
            MoveList pieceMoveList = allMoves.Pop(); 
            while(pieceMoveList.movePlace.Count != 0)
            {
                Vector2Int tempPiecePlace = pieceMoveList.piecePlace;
                Vector2Int tempMovePlace = pieceMoveList.movePlace.Dequeue();
                string tempPieceChange = "0";
                if (pieceMoveList.pieceAfterMove.Count != 0)
                {
                    finalPieceChange = pieceMoveList.pieceAfterMove.Dequeue();
                }

                Board tempBoard = new Board(currentBoard);
                tempBoard = MovePiece(tempPiecePlace, tempMovePlace, tempPieceChange, tempBoard);
                if (whiteSide)
                {
                    if (tempBoard.Evaluation > bestEvaluation)
                    {
                        bestEvaluation = tempBoard.Evaluation;
                        finalPieceChange = tempPieceChange;
                        finalMovePlace = tempMovePlace;
                        finalPiecePlace = tempPiecePlace;
                        Debug.Log("white search move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + tempBoard.BoardCells[finalMovePlace.x, finalMovePlace.y] + " " + finalMovePlace.x + ", " + finalMovePlace.y);
                    }
                }
                else
                {
                    if (tempBoard.Evaluation < bestEvaluation)
                    {
                        bestEvaluation = tempBoard.Evaluation;
                        finalPieceChange = tempPieceChange;
                        finalMovePlace = tempMovePlace;
                        finalPiecePlace = tempPiecePlace;
                        Debug.Log("black search move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + tempBoard.BoardCells[finalMovePlace.x, finalMovePlace.y] + " " + finalMovePlace.x + ", " + finalMovePlace.y);
                    }
                }
            }
        }


        string logSide = "white";
        if (!whiteSide)
            logSide = "black";
        if (finalPieceChange == "0")
            Debug.Log(logSide + " use move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + finalMovePlace.x + ", " + finalMovePlace.y);
        else
            Debug.Log(logSide + " use move: " + currentBoard.BoardCells[finalPiecePlace.x, finalPiecePlace.y] + " from " + finalPiecePlace.x + ", " + finalPiecePlace.y + " to " + finalPieceChange + " " + finalMovePlace.x + ", " + finalMovePlace.y);
        canMove = true;
    }

    Stack<MoveList> GetAllMoveInBoard(Board currentBoard, bool whiteSide)
    {
        allMoveList = new Stack<MoveList>();
        for(int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (currentBoard.BoardCells[i, j] != "0")
                {
                    bool isWhitePiece = RuleHandler.isWhitePiece(new Vector2Int(i, j), currentBoard.BoardCells);
                    if ((whiteSide && isWhitePiece) || (!whiteSide && !isWhitePiece))
                    {
                        MoveList moveList = new MoveList();
                        moveList = RuleHandler.FindPieceMove(new Vector2Int(i, j), currentBoard, isWhitePiece);
                        if (moveList.movePlace.Count != 0)
                            allMoveList.Push(moveList);
                    }
                }
            }
        }

        return allMoveList;
    }    
}
