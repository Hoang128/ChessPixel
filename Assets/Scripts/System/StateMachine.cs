using System.Collections;
using System.Collections.Generic;

public class StateMachine
{
    private State currentState;

    public void StateHandle()
    {
        currentState.OnUpdate();
    }

    public void StateChange(State state)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = state;

        if (currentState != null)
        {
            currentState.OnEnter();
        }
    }
}
