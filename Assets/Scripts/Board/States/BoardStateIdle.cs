using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStateIdle : BoardState
{
    public BoardStateIdle(StateMachine stateMachine, PlayBoard boardController) : base(stateMachine, boardController)
    {
        this.boardController = boardController;
        this.stateMachine = stateMachine;
    }

    public override void OnEnter()
    {
        Debug.Log("Entered Board State Idle!");

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
        if (boardController.ClickPoint != new Vector2Int(-1, -1))
        {
            stateMachine.StateChange(new BoardStateChooseMove(stateMachine, boardController));
        }
    }

    public override bool OnChange()
    {
        return true;
    }
}
