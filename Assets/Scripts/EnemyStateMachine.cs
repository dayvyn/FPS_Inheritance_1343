using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine 
{
    public State currentState { get; set; }

    public void Initialize(State startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }
    public void ChangeState(State changingState)
    {
        currentState.Exit();
        currentState = changingState;
        changingState.Enter();
    }

}
