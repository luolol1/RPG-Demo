using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected string animBoolName;
    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;

    protected float stateTimer;
    protected bool TriggerCalled;
    public PlayerState(Player _player,PlayerStateMachine _stateMachine,string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;  
        this.animBoolName = _animBoolName;
    }
    public virtual void Enter()
    {
        rb = player.rb;
        player.anim.SetBool(animBoolName, true);
        TriggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer-=Time.deltaTime;

        player.anim.SetFloat("yVelocity", rb.velocity.y);//更新yVelocity的值用来切换Jump/Fall的动画
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
    public virtual void AnimationFinishTrigger()
    {
        TriggerCalled = true;
    }
}
