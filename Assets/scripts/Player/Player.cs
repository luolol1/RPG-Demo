using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Player : Entity
{
    public bool isBusy { get;private set; }
    [Header("Attack details")]
    public Vector2[] AttackMovement;
    public float CounterAttackDuration;

    [Header("Move info")]
    public float Movespeed;

    [Header("Jump info")]
    public float Jumpforce;

    [Header("Dash info")]
    [SerializeField] private float DashCoolDown;//冲刺的cd
    private float DashTime;//还差多少时间才能使用冲刺技能（小于0即可使用）
    public float DashSpeed;
    public float DashDuration;
    public float DashDirection { get; private set; }





    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }//相当于FallState
    public PlayerDashState dashState { get; private set; }//冲刺
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerPrimaryAttack primaryAttack { get; private set; }
    public PlayerCounterAttack counterAttack { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();

        stateMachine=new PlayerStateMachine();
        
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttack(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttack(this, stateMachine, "CounterAttack");
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.CurrentState.Update();//持续更新玩家
        CheckForDashInput();//希望在任何时刻都能冲刺闪避

    }
    public void AnimationTrigger() => stateMachine.CurrentState.AnimationFinishTrigger();
    private void CheckForDashInput()
    {
        DashTime-=Time.deltaTime;
        if (IsWallDetected())
            return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && DashTime<0)
        {
            DashTime = DashCoolDown;//更新cd
            DashDirection = Input.GetAxisRaw("Horizontal");
            if (DashDirection == 0)
                DashDirection = facingDirection;
            stateMachine.ChangeState(dashState);
        }
    }
    public IEnumerator BusyFor()
    {
        isBusy = true;
        yield return new WaitForSeconds(0.15f);
        isBusy = false;
    }
    
    
}
