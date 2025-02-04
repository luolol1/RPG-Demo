using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            stateMachine.ChangeState(player.wallJump);
            return;
        }
        if (yInput < 0)
            player.SetVelocity(0, rb.velocity.y);
        else
            player.SetVelocity(0, rb.velocity.y * 0.7f);
        if(xInput!=0 && xInput!=player.facingDirection)
            stateMachine.ChangeState(player.idleState);
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
