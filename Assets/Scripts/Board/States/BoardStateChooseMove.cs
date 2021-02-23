using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStateChooseMove : BoardState
{
    GameObject player;
    GameObject moveMark;
    GameObject captureMark;
    GameObject movePromoMark;
    GameObject capturePromoMark;
    Board currentBoard;
    float cellSize;
    Vector2Int clickPoint;
    RuleHandler.MoveList moveList;
    Vector3 startPosition;
    bool moved = false;

    public BoardStateChooseMove(StateMachine stateMachine, PlayBoard boardController) : base(stateMachine, boardController)
    {
        this.boardController = boardController;
        this.stateMachine = stateMachine;
    }

    public override void OnEnter()
    {
        Debug.Log("Entered Board State Choose Move!");

        boardController.GetComponent<PlayBoard>().StateName = "Board Choose Move";

        //Choose move side
        if (boardController.WhiteTurn)
        {
            player = boardController.PlayerWhiteMgr;
        }
        else
            player = boardController.PlayerBlackMgr;
        boardController.ChooseMovePhase = 1;
        clickPoint = boardController.GetComponent<PlayBoard>().ClickPoint;
        cellSize = boardController.GetComponent<PlayBoard>().CellSize;
        currentBoard = boardController.GetComponent<PlayBoard>().BoardStack.Peek();
        moveMark = boardController.GetComponent<PlayBoard>().MoveMarkObject;
        captureMark = boardController.GetComponent<PlayBoard>().CaptureMarkObject;
        movePromoMark = boardController.GetComponent<PlayBoard>().MovePromoMarkObject;
        capturePromoMark = boardController.GetComponent<PlayBoard>().CapturePromoMarkObject;
        startPosition = boardController.transform.position;
        moveList = RuleHandler.FindPieceMove(clickPoint, currentBoard, player.GetComponent<PlayerMgr>().IsWhite);
        if (moveList.pieceAfterMove.Count == 0)
        {
            foreach (Vector2Int movePlace in moveList.movePlace)
            {
                Vector3 createPos = startPosition + new Vector3(movePlace.x * cellSize, movePlace.y * cellSize, -20f);
                GameObject mark;
                if (currentBoard.BoardCells[movePlace.x, movePlace.y] == "0")
                {
                    mark = GameObject.Instantiate(moveMark, createPos, boardController.transform.rotation);
                    mark.GetComponent<Mark>().Coor = movePlace;
                }
                else
                {
                    mark = GameObject.Instantiate(captureMark, createPos, boardController.transform.rotation);
                    mark.GetComponent<Mark>().Coor = movePlace;
                }
            }
        }    
        else
        {
            Vector2Int currentMove = new Vector2Int(-1, -1);

            foreach (Vector2Int movePlace in moveList.movePlace)
            {
                if (movePlace != currentMove)
                {
                    currentMove = movePlace;
                    Vector3 createPos = startPosition + new Vector3(movePlace.x * cellSize, movePlace.y * cellSize, -20f);
                    GameObject mark;
                    if (currentBoard.BoardCells[movePlace.x, movePlace.y] == "0")
                    {
                        mark = GameObject.Instantiate(movePromoMark, createPos, boardController.transform.rotation);
                        mark.GetComponent<Mark>().Coor = movePlace;
                    }
                    else
                    {
                        mark = GameObject.Instantiate(capturePromoMark, createPos, boardController.transform.rotation);
                        mark.GetComponent<Mark>().Coor = movePlace;
                    }
                }
            }
        }
    }

    public override void OnExit()
    {
        GameObject[] moveMarkerList = GameObject.FindGameObjectsWithTag("Move Marker");
        foreach (GameObject marker in moveMarkerList)
        {
            GameObject.Destroy(marker);
        }

        GameObject[] captureMarkerList = GameObject.FindGameObjectsWithTag("Capture Marker");
        foreach (GameObject marker in captureMarkerList)
        {
            GameObject.Destroy(marker);
        }

        boardController.ClickPoint = new Vector2Int(-1, -1);

        boardController.ChooseMovePhase = 0;

        if (moved)
        {
            if (boardController.WhiteTurn)
            {
                boardController.WhiteTurn = false;
                return;
            }
            else
            {
                boardController.WhiteTurn = true;
                return;
            }
        }
    }

    public override void OnUpdate()
    {
        if (boardController.ChooseMovePhase == 0)
        {
            stateMachine.StateChange(new BoardStateIdle(stateMachine, boardController));
        }
        else if (boardController.ChooseMovePhase > 1)
        {
            if (GameObject.FindGameObjectWithTag("Promo Table") == null)
            {
                if (boardController.WhiteTurn)
                {
                    if (boardController.PlayerWhiteMgr.GetComponent<PlayerMgr>().FinalMovePlace != new Vector2Int(-1, -1))
                    {
                        Vector2Int movePlace = boardController.PlayerWhiteMgr.GetComponent<PlayerMgr>().FinalMovePlace;
                        string newPiece = boardController.PlayerWhiteMgr.GetComponent<PlayerMgr>().FinalPieceChange;
                        Board newBoard = RuleHandler.MovePiece(clickPoint, movePlace, newPiece, currentBoard);
                        boardController.GetComponent<PlayBoard>().UpdateBoard(newBoard);
                        boardController.PlayerWhiteMgr.GetComponent<PlayerMgr>().FinalMovePlace = new Vector2Int(-1, -1);
                        boardController.PlayerWhiteMgr.GetComponent<PlayerMgr>().FinalPieceChange = "0";
                        moved = true;
                        stateMachine.StateChange(new BoardStateIdle(stateMachine, boardController));
                    }
                }
                else
                {
                    if (boardController.PlayerBlackMgr.GetComponent<PlayerMgr>().FinalMovePlace != new Vector2Int(-1, -1))
                    {
                        Vector2Int movePlace = boardController.PlayerBlackMgr.GetComponent<PlayerMgr>().FinalMovePlace;
                        string newPiece = boardController.PlayerBlackMgr.GetComponent<PlayerMgr>().FinalPieceChange;
                        Board newBoard = RuleHandler.MovePiece(clickPoint, movePlace, newPiece, currentBoard);
                        boardController.GetComponent<PlayBoard>().UpdateBoard(newBoard);
                        boardController.PlayerBlackMgr.GetComponent<PlayerMgr>().FinalMovePlace = new Vector2Int(-1, -1);
                        boardController.PlayerBlackMgr.GetComponent<PlayerMgr>().FinalPieceChange = "0";
                        moved = true;
                        stateMachine.StateChange(new BoardStateIdle(stateMachine, boardController));
                    }
                }
            }
        }
    }

    public override bool OnChange()
    {
        return true;
    }
}
