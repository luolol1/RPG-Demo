using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerState _CurrentState)
    {
        this.CurrentState = _CurrentState;
        CurrentState.Enter();
    }
    public void ChangeState(PlayerState NextState)
    {
        CurrentState.Exit();
        CurrentState = NextState;
        CurrentState.Enter();
    }
}
