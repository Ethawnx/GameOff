using UnityEngine;

public class PlayerAirState : PlayerState
{
    Vector2 gravity;
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Do()
    {
        base.Do();
        if (InputManager.Movement.x != 0)
            player.CheckDirectionToFace(InputManager.Movement.x > 0);

        if (player.CheckIfGrounded() && Mathf.Abs(player.RB.linearVelocityY) < 0.1f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (InputManager.TeleportWasPressed && player.CheckIfCanTP())
        {
            stateMachine.ChangeState(player.TeleportState);
        }
        else if (player.CheckIfTouchingWall() && InputManager.Movement.x == 1 * player.FacingDirection && !player.CheckIfGrounded()) 
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        gravity = new Vector2(0, 9.8f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedDo()
    {
        player.RB.AddForce(Vector2.down * 9.8f, ForceMode2D.Force);
        if (player.RB.linearVelocityY < 0f) 
        {
            player.RB.linearVelocity -= gravity * playerData.fallGravityMult * Time.deltaTime;
        }
        base.FixedDo();
    }
}
