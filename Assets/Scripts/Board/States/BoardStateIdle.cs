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

        isWhiteTurn = boardController.GetComponent<PlayBoard>().WhiteTurn;
        if (isWhiteTurn)
            player = boardController.GetComponent<PlayBoard>().PlayerWhiteMgr;
        else
            player = boardController.GetComponent<PlayBoard>().PlayerBlackMgr;
        boardController.GetComponent<PlayBoard>().ClickPoint = new Vector2Int(-1, -1);
        boardController.GetComponent<PlayBoard>().StateName = "Board Idle";
        boardController.PlayerBlack.GetComponent<PlayerMgr>().FinalMovePlace = new Vector2Int(-1, -1);
        boardController.PlayerWhite.GetComponent<PlayerMgr>().FinalMovePlace = new Vector2Int(-1, -1);
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (player.GetComponent<AIMgr>() == null)
        {
            if (boardController.ClickPoint != new Vector2Int(-1, -1))
            {
                stateMachine.StateChange(new BoardStateChooseMove(stateMachine, boardController));
            }
        }
        else
        {
            if (player.GetComponent<AIMgr>().FinalPiecePlace != new Vector2Int(-1, -1))
            {
                stateMachine.StateChange(new BoardStateMove(stateMachine, boardController));
            }
        }
    }

    public override bool OnChange()
    {
        return true;
    }
}
