using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }
    public override void FixedDo()
    {
        base.FixedDo();
        player.RB.linearVelocityY = -playerData.wallSlideVelocity;
    }
}
