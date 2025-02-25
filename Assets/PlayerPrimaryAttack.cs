using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    private int ComboCounter;//��������
    private float LastAttackTime;//���һ�ι�����ʱ��
    private float ComboWindow = 2;//�������ʱ��
    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (ComboCounter > 2 || Time.time - LastAttackTime >= ComboWindow)
            ComboCounter = 0;
        player.anim.SetInteger("ComboCounter", ComboCounter);
        player.SetVelocity(player.AttackMovement[ComboCounter].x * player.facingDirection, player.AttackMovement[ComboCounter].y);
        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();
        LastAttackTime=Time.time;
        ComboCounter++;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            player.SetVelocity(0,0);
        if (TriggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
