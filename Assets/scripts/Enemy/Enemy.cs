using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;
    [Header("Stunned info")]
    public float StunnedDuration;
    public Vector2 StunnedDir;
    protected bool canbeStunned;
    [SerializeField] protected GameObject CounterImage;
    [Header("Move info")]
    public float MoveSpeed;
    public float IdleTime;
    public float BattleTime;

    [Header("Attack info")]
    public float attackDistance;
    public float attackCoolDown;
    [HideInInspector] public float LastAttackTime;
    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine=new EnemyStateMachine();
    }
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.CurrentState.Update();
    }
    public virtual void OpenCounterAttackWindow()
    {
        canbeStunned=true;
        CounterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()
    {
        canbeStunned = false;
        CounterImage.SetActive(false);
    }
    public virtual bool CanbeStunned()
    {
        if(canbeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;

    }
    public virtual RaycastHit2D isPlayerDetected() => Physics2D.Raycast(WallCheck.position, Vector2.right * facingDirection, 50, whatIsPlayer);
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }
    public void AnimationTrigger() =>stateMachine.CurrentState.AnimationFinishTrigger();
}
