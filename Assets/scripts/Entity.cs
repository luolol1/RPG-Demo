using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Knockback info")]
    public Vector2 KnockbackDirection;
    private bool isKnocked;
    public float KnockbackDuration;
    [Header("Collision info")]
    public Transform AttackCheck;
    public float AttackRadius;
    [SerializeField] protected Transform GroundCheck;
    [SerializeField] protected float GroundCheckDistance;
    [SerializeField] protected Transform WallCheck;
    [SerializeField] protected float WallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    public int facingDirection { get; private set; } = 1;//��ʼĬ��������
    protected bool facingRight = true;

    #region Components

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX FX { get; private set; }
    #endregion

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        FX = GetComponent<EntityFX>();
    }

    protected virtual void Start()
    {
        
    }
    protected virtual void Update()
    {

    }
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    public virtual void Damage()
    {
        Debug.Log(gameObject.name + " was damaged");
        FX.StartCoroutine("FlashFX");
        StartCoroutine("KnockBack");
    }
    public IEnumerator KnockBack()
    {
        isKnocked = true;
        rb.velocity = new Vector2(KnockbackDirection.x * -facingDirection, KnockbackDirection.y);

        yield return new WaitForSeconds(KnockbackDuration);

        isKnocked = false;
    }
    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(GroundCheck.position, Vector2.down, GroundCheckDistance, whatIsGround);//������

    public virtual bool IsWallDetected() => Physics2D.Raycast(WallCheck.position, Vector2.right * facingDirection, WallCheckDistance, whatIsGround);
    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(GroundCheck.position, new Vector3(GroundCheck.position.x, GroundCheck.position.y - GroundCheckDistance));//���ߣ���һ��������ȥǰһ��������
        Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + WallCheckDistance, WallCheck.position.y));//����
        Gizmos.DrawWireSphere(AttackCheck.position, AttackRadius);
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
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion
}
