using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStateChooseMove : BoardState
{
    GameObject player;
    GameObject moveMark;
    GameObject captureMark;
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

        clickPoint = boardController.GetComponent<PlayBoard>().ClickPoint;
        cellSize = boardController.GetComponent<PlayBoard>().CellSize;
        currentBoard = boardController.GetComponent<PlayBoard>().BoardStack.Peek();
        moveMark = boardController.GetComponent<PlayBoard>().MoveMarkObject;
        captureMark = boardController.GetComponent<PlayBoard>().CaptureMarkObject;
        startPosition = boardController.transform.position;
        moveList = RuleHandler.FindPieceMove(clickPoint, currentBoard, player.GetComponent<PlayerMgr>().IsWhite);
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

    public override void OnExit()
    {
        GameObject[] markerList = GameObject.FindGameObjectsWithTag("Marker");
        foreach (GameObject marker in markerList)
        {
            GameObject.Destroy(marker);
        }

        boardController.ClickPoint = new Vector2Int(-1, -1);

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
        if (boardController.ClickPoint == new Vector2Int(-1, -1))
        {
            stateMachine.StateChange(new BoardStateIdle(stateMachine, boardController));
        }
        else
        {
            if (boardController.WhiteTurn)
            {
                if (boardController.PlayerWhiteMgr.GetComponent<PlayerMgr>().FinalMovePlace != new Vector2Int(-1, -1))
                {
                    Vector2Int movePlace = boardController.PlayerWhiteMgr.GetComponent<PlayerMgr>().FinalMovePlace;
                    Board newBoard = RuleHandler.movePiece(clickPoint, movePlace, currentBoard);
                    boardController.GetComponent<PlayBoard>().UpdateBoard(newBoard);
                    boardController.PlayerWhiteMgr.GetComponent<PlayerMgr>().FinalMovePlace = new Vector2Int(-1, -1);
                    moved = true;
                    stateMachine.StateChange(new BoardStateIdle(stateMachine, boardController));
                }
            }
            else
            {
                if (boardController.PlayerBlackMgr.GetComponent<PlayerMgr>().FinalMovePlace != new Vector2Int(-1, -1))
                {
                    Vector2Int movePlace = boardController.PlayerBlackMgr.GetComponent<PlayerMgr>().FinalMovePlace;
                    Board newBoard = RuleHandler.movePiece(clickPoint, movePlace, currentBoard);
                    boardController.GetComponent<PlayBoard>().UpdateBoard(newBoard);
                    boardController.PlayerBlackMgr.GetComponent<PlayerMgr>().FinalMovePlace = new Vector2Int(-1, -1);
                    moved = true;
                    stateMachine.StateChange(new BoardStateIdle(stateMachine, boardController));
                }
            }
        }
    }

    public override bool OnChange()
    {
        return true;
    }
}
