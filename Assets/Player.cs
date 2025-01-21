using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine;
    
    public PlayerIdleState idleState;
    public PlayerMoveState moveState;
    private void Awake()
    {
        stateMachine=new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
    }
    private void Start()
    {
        stateMachine.Initialize(idleState);
    }
    private void Update()
    {
        stateMachine.CurrentState.Update();//持续更新玩家
    }
}
