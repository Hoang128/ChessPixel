using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardState : State
{
    protected PlayBoard boardController;

    public BoardState(StateMachine stateMachine, PlayBoard boardController) : base(stateMachine)
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
