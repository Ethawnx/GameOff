using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Do()
    {
        base.Do();
        
        if (Time.time >= startTime + playerData.wallJumpTime)
        {
            isAbilityDone = true;
        }
    }
    public override void Enter()
    {
        base.Enter();
        player.RB.linearVelocity = new Vector2(playerData.wallJumpAngle.x * playerData.wallJumpVelocity * -player.FacingDirection, playerData.wallJumpAngle.y * playerData.wallJumpVelocity);
        player.CheckIfShouldFlip(-player.FacingDirection);
    }
}
