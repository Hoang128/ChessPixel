﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStateAIMove : BoardState
{
    GameObject player;
    bool isWhiteTurn;
    bool moved = false;
    Board currentBoard;
    public BoardStateAIMove(StateMachine stateMachine, PlayBoard boardController) : base(stateMachine, boardController)
    {
        this.boardController = boardController;
        this.stateMachine = stateMachine;
    }

    public override void OnEnter()
    {
        currentBoard = boardController.GetComponent<PlayBoard>().BoardStack.Peek();
        isWhiteTurn = boardController.GetComponent<PlayBoard>().WhiteTurn;
        if (isWhiteTurn)
            player = boardController.GetComponent<PlayBoard>().PlayerWhiteMgr;
        else
            player = boardController.GetComponent<PlayBoard>().PlayerBlackMgr;
        player.GetComponent<AIMgr>().CanMove = false;

        player.GetComponent<AIMgr>().FindBestMove(currentBoard, isWhiteTurn);
    }

    public override void OnExit()
    {
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
        if (player.GetComponent<AIMgr>().CanMove)
        {
            Vector2Int piecePlace = player.GetComponent<AIMgr>().FinalPiecePlace;
            Vector2Int movePlace = player.GetComponent<AIMgr>().FinalMovePlace;
            string pieceAfterChange = player.GetComponent<AIMgr>().FinalPieceChange;

            Board newBoard = RuleHandler.MovePiece(piecePlace, movePlace, pieceAfterChange, currentBoard);
            boardController.GetComponent<PlayBoard>().UpdateBoard(newBoard);

            moved = true;
            stateMachine.StateChange(new BoardStateIdle(stateMachine, boardController));
        }
    }

    public override bool OnChange()
    {
        return true;
    }
}
