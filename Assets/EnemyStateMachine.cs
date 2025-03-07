using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine 
{
    public EnemyState CurrentState { get; private set; }
    public void Initialize(EnemyState _CurrentState)
    {
        CurrentState = _CurrentState;
        CurrentState.Enter();
    }
    public void ChangeState(EnemyState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
