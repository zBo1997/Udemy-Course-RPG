using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Field
    [Header("Move Info")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    [Header("Dash info")]
    [SerializeField] private float dashCooldown = 0.25f;
    private float dashTimer;
    public float dashSpeed = 10f;
    public float dashDiuration;
    public float dashDir { get; private set; }//为向左面朝方向的冲刺 跟随面朝方向冲刺

    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 0.2f;
    [SerializeField] private LayerMask whatisGround; // 射线检测的物体层级
    [SerializeField] private LayerMask whatisWall;

    public int facingDir { get; private set; } = 1;

    private bool facingRight = true;

    #endregion

    #region Components
    public Animator animator
    {
        get; private set;
    }
    public Rigidbody2D rb
    {
        get; private set;
    }
    #endregion

    #region States
    public PlayerStateMachine stateMachine
    {
        get; private set;
    }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerGroundedState groundedState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlippyState playerWallSlippyState { get; private set; }

    #endregion

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        playerWallSlippyState = new PlayerWallSlippyState(this, stateMachine, "WallSlippy");
        //groundedState = new PlayerGroundedState(this, stateMachine, "Grounded");
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Initializae(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
        CheckForDashInput();
    }

    private void CheckForDashInput()
    {
        dashTimer -= Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0)
        {
            dashTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");
            Debug.Log("dashDir: " + dashDir);
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(dashState);
        }
    }

    public void SetVelocity(float x_Velocity, float y_Velocity)
    {
        //在设置速度前，先选择是否反转角色的方向
        Debug.Log("x_Velocity: " + x_Velocity);
        rb.velocity = new Vector2(x_Velocity, y_Velocity);
        FlipController(x_Velocity);
    }

    //检测是否是 grounded 地面
    public bool isGroundedDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatisGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }

    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void FlipController(float _xVelocity)
    {
        if (_xVelocity > 0 && !facingRight)
        {
            Flip();
        }
        else if (_xVelocity < 0 && facingRight)
        {
            Flip();
        }
    }
}
