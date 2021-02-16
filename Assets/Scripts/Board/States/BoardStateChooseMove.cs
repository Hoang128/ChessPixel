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
    Vector2 clickPoint;
    RuleHandler.MoveList moveList;
    Vector2 startPosition;

    public BoardStateChooseMove(StateMachine stateMachine, PlayBoard boardController) : base(stateMachine, boardController)
    {
        this.boardController = boardController;
        this.stateMachine = stateMachine;
    }

    public override void OnEnter()
    {
        Debug.Log("Entered Board State Choose Move!");

        //Choose move side
        if (boardController.WhiteTurn)
        {
            player = boardController.PlayerWhite;
        }
        else
            player = boardController.PlayerBlack;

        clickPoint = boardController.GetComponent<PlayBoard>().ClickPoint;
        cellSize = boardController.GetComponent<PlayBoard>().CellSize;
        currentBoard = boardController.GetComponent<PlayBoard>().BoardStack.Peek();
        moveMark = boardController.GetComponent<PlayBoard>().MoveMarkObject;
        captureMark = boardController.GetComponent<PlayBoard>().CaptureMarkObject;
        startPosition = boardController.transform.position;
        moveList = RuleHandler.FindMove(clickPoint, currentBoard, player.GetComponent<PlayerMgr>().IsWhite);

        Debug.Log("piece place = " + moveList.piecePlace.x + ", " + moveList.piecePlace.y);
        foreach (Vector2 movePlace in moveList.movePlace)
        {
            int movePlaceX = System.Convert.ToInt32(movePlace.x);
            int movePlaceY = System.Convert.ToInt32(movePlace.y);
            if (currentBoard.BoardCells[movePlaceY, movePlaceX] == "0")
                GameObject.Instantiate(moveMark, startPosition + new Vector2(movePlaceX * cellSize, movePlaceY * cellSize), boardController.transform.rotation);
            else
                GameObject.Instantiate(captureMark, startPosition + new Vector2(movePlaceX * cellSize, movePlaceY * cellSize), boardController.transform.rotation);
        }
    }

    public override void OnExit()
    {
        GameObject[] markerList = GameObject.FindGameObjectsWithTag("Marker");
        foreach (GameObject marker in markerList)
        {
            GameObject.Destroy(marker);
        }
    }

    public override void OnUpdate()
    {
        if (boardController.WhiteTurn)
        {
            if (boardController.PlayerWhite.GetComponent<PlayerMgr>().FinalMovePlace != new Vector2(-1, -1))
            {
                Vector2 movePlace = boardController.PlayerWhite.GetComponent<PlayerMgr>().FinalMovePlace;
                Board newBoard = RuleHandler.movePiece(clickPoint, movePlace, currentBoard); 
                boardController.GetComponent<PlayBoard>().BoardStack.Push(newBoard);
            }

            stateMachine.StateChange(new BoardStateIdle(stateMachine, boardController));
        }
        else
        {
            if (boardController.PlayerBlack.GetComponent<PlayerMgr>().FinalMovePlace != new Vector2(-1, -1))
            {
                Vector2 movePlace = boardController.PlayerBlack.GetComponent<PlayerMgr>().FinalMovePlace;
                Board newBoard = RuleHandler.movePiece(clickPoint, movePlace, currentBoard);
                boardController.GetComponent<PlayBoard>().BoardStack.Push(newBoard);
            }

            stateMachine.StateChange(new BoardStateIdle(stateMachine, boardController));
        }
    }

    public override bool OnChange()
    {
        return true;
    }
}
