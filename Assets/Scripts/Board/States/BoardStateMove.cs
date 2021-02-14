using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStateMove : BoardState
{
    public BoardStateMove(StateMachine stateMachine, PlayBoard boardController) : base(stateMachine, boardController)
    {
        this.boardController = boardController;
        this.stateMachine = stateMachine;
    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }

    public override bool OnChange()
    {
        return true;
    }
}
