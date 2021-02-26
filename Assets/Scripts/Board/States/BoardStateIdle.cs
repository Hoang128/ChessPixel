using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStateIdle : BoardState
{
    GameObject player;
    bool isWhiteTurn;
    public BoardStateIdle(StateMachine stateMachine, PlayBoard boardController) : base(stateMachine, boardController)
    {
        this.boardController = boardController;
        this.stateMachine = stateMachine;
    }

    public override void OnEnter()
    {
        Debug.Log("Entered Board State Idle!");

        if (boardController.MovingAnimObj != null)
            GameObject.Destroy(boardController.MovingAnimObj);
        isWhiteTurn = boardController.GetComponent<PlayBoard>().WhiteTurn;
        if (isWhiteTurn)
            player = boardController.GetComponent<PlayBoard>().PlayerWhiteMgr;
        else
            player = boardController.GetComponent<PlayBoard>().PlayerBlackMgr;
        boardController.GetComponent<PlayBoard>().ClickPoint = new Vector2Int(-1, -1);
        boardController.GetComponent<PlayBoard>().StateName = "Board Idle";

        boardController.GetComponent<PlayBoard>().CanRematch = true;
        if (boardController.GetComponent<PlayBoard>().BoardStack.Count < 2)
        {
            boardController.GetComponent<PlayBoard>().CanUndoMove = false;
        }
        else
            boardController.GetComponent<PlayBoard>().CanUndoMove = true;

        GameObject[] lastMoveMarks = GameObject.FindGameObjectsWithTag("Last Move Marker");
        if ((lastMoveMarks.Length != 0) && (lastMoveMarks != null))
        {
            foreach (GameObject mark in lastMoveMarks)
            {
                GameObject.Destroy(mark);
            }
        }

        Vector2 startPosition = boardController.transform.position;
        float cellSize = boardController.CellSize;
        if (boardController.GetComponent<PlayBoard>().LastPieceMove.Count > 0)
        {
            Vector2Int cellPos = boardController.GetComponent<PlayBoard>().LastPieceMove.Peek();
            GameObject.Instantiate(boardController.GetComponent<PlayBoard>().LastMoveMarkObject, startPosition + new Vector2(cellPos.x * cellSize, cellPos.y * cellSize), boardController.transform.rotation);
        }

        if (boardController.GetComponent<PlayBoard>().LastPiecePlace.Count > 0)
        {
            Vector2Int cellPos = boardController.GetComponent<PlayBoard>().LastPiecePlace.Peek();
            GameObject.Instantiate(boardController.GetComponent<PlayBoard>().LastMoveMarkObject, startPosition + new Vector2(cellPos.x * cellSize, cellPos.y * cellSize), boardController.transform.rotation);
        }
    }

    public override void OnExit()
    {
        boardController.CanRematch = false;
        boardController.CanUndoMove = false;

        if (boardController.MovingAnimObj == null)
            boardController.MovingAnimObj = GameObject.Instantiate(boardController.MovingAnimEffect, new Vector3(0f, 28f, 0f), boardController.transform.rotation);
    }

    public override void OnUpdate()
    {
        PlayerMgr aiComp = player.GetComponent<PlayerMgr>();
        if (aiComp != null)
        {
            Board currentBoard = boardController.BoardStack.Peek();
            int gameState = AIMgr.GetGameState(currentBoard);

            if (gameState != 0)
            {
                string result = "Draw!";
                if (gameState == 1)
                    result = "White win!";
                if (gameState == -1)
                    result = "Black win!";

                if (boardController.GetComponent<PlayBoard>().GameCanvas.GetComponent<Canvas>().Table == null)
                {
                    boardController.GetComponent<PlayBoard>().GameCanvas.GetComponent<Canvas>().CreateResultTable();
                    GameObject table = GameObject.FindGameObjectWithTag("UI Table");
                    table.GetComponent<UnityEngine.UI.Text>().text = result;
                }
            }

            if (boardController.ClickPoint != new Vector2Int(-1, -1))
            {
                stateMachine.StateChange(new BoardStateChooseMove(stateMachine, boardController));
            }
        }
        else
        {
                stateMachine.StateChange(new BoardStateAIMove(stateMachine, boardController));
        }
    }

    public override bool OnChange()
    {
        return true;
    }
}
