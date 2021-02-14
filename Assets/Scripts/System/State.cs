using System.Collections;
using System.Collections.Generic;

public abstract class State
{
    protected StateMachine stateMachine;

    public State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    // Start is called when start state
    public abstract void OnEnter();

    // Update is called once per frame
    public abstract void OnUpdate();

    //Exit is called when exit state
    public abstract void OnExit();

    //OnChange is called when need to change to another stage
    public abstract bool OnChange();
}
