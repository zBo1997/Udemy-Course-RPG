using UnityEngine;
public class PlayerState
{
    protected PlayerStateMachine stateMachine;

    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;

    private string animBoolName;

    protected float stateTimer;



    public PlayerState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName)
    {
        this.stateMachine = _playerStateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        Debug.Log("im Enter " + animBoolName);
        player.animator.SetBool(animBoolName, true);
        rb = player.GetComponent<Rigidbody2D>();
    }

    public virtual void Update()
    {
        Debug.Log("im Update " + animBoolName);
        stateTimer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        player.animator.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        Debug.Log("im Exit " + animBoolName);
        player.animator.SetBool(animBoolName, false);
    }
}
