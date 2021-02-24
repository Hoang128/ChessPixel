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
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        PlayerMgr aiComp = player.GetComponent<PlayerMgr>();
        if (aiComp != null)
        {
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
