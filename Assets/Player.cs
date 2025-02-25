using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attack details")]
    public Vector2[] AttackMovement;
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
    [Header("Collision info")]
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private float GroundCheckDistance;
    [SerializeField] private Transform WallCheck;
    [SerializeField] private float WallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public int facingDirection { get; private set; } = 1;//初始默认面向右
    private bool facingRight = true; 
    
    #region Components

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get;private set; }
    #endregion

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

    #endregion
    private void Awake()
    {
        stateMachine=new PlayerStateMachine();
        anim=GetComponentInChildren<Animator>();
        rb=GetComponent<Rigidbody2D>();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttack(this, stateMachine, "Attack");
    }
    private void Start()
    {
        stateMachine.Initialize(idleState);
    }
    private void Update()
    {
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
    public void SetVelocity(float _xVelocity,float _yVelocity)
    {
        rb.velocity=new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(GroundCheck.position, Vector2.down, GroundCheckDistance, whatIsGround);//地面检测

    public bool IsWallDetected() => Physics2D.Raycast(WallCheck.position, Vector2.right * facingDirection, WallCheckDistance, whatIsGround);
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(GroundCheck.position, new Vector3(GroundCheck.position.x, GroundCheck.position.y - GroundCheckDistance));
        Gizmos.DrawLine(WallCheck.position,new Vector3(WallCheck.position.x+WallCheckDistance,WallCheck.position.y));
    }
    #endregion
    #region Flip
    public void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if(_x < 0 && facingRight) 
            Flip();
    }
    #endregion
}
