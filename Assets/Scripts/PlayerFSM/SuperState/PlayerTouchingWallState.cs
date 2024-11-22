using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Do()
    {
        base.Do();

        if (InputManager.Movement.x != player.FacingDirection || !player.CheckIfTouchingWall())
        {
            stateMachine.ChangeState(player.AirState);
        }
        else if (player.CheckIfGrounded())
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (player._jumpBufferTimer > 0f)
        {
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (player.CheckIfOnLedge())
        {
            stateMachine.ChangeState(player.LedgeUpState);
        }
    }
}
