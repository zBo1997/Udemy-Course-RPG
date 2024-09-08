using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDiuration;
    }

    public override void Exit()
    {
        base.Exit();
        //
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("Dash");
        player.SetVelocity(player.dashSpeed * player.facingDir, 0);
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
