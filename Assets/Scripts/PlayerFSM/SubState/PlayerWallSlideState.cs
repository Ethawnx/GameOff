using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    ParticleSystem slideVFX;
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        slideVFX = player.SlideVFX.GetComponentInChildren<ParticleSystem>();
        slideVFX.Play();
    }
    public override void FixedDo()
    {
        base.FixedDo();
        player.RB.linearVelocityY = -playerData.wallSlideVelocity;
    }
    public override void Exit()
    {
        base.Exit();
        slideVFX.Stop();
    }
}
