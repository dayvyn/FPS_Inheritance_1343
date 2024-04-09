using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    State state;

    public StateManager(State initialState)
    {
        Debug.Log("Hello");
    }

    // Update is called once per frame
    public void Update()
    {
        state.ExitState();
        state.SwitchState();
        state.EnterState();
    }
}

public abstract class State : MonoBehaviour
{
    public virtual void ExitState()
    {

    }

    public virtual void SwitchState()
    {

    }

    public virtual void EnterState()
    {

    }
    private void Update()
    {
        
    }
}
