using UnityEngine;

public class PlayerLedgeUpState : PlayerState
{
    Vector2 endPosition;
    Vector2 ledgeUpdir;
    public PlayerLedgeUpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        player.DetermineCornerPosition();
        Debug.Log(player.CornerPosition);
        player.transform.position = player.CornerPosition + new Vector2((playerData.StartPositionOffset.x * -player.FacingDirection), playerData.StartPositionOffset.y);
        endPosition = player.CornerPosition + new Vector2(0.02f, 0.5f);
    }
    public override void Do()
    {
        base.Do();
        if (isAnimationFinished) 
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
    public override void FixedDo()
    {
        base.FixedDo();
        ledgeUpdir = new Vector2(1f, 2f);
        ledgeUpdir = ledgeUpdir.normalized;
        ledgeUpdir.x = ledgeUpdir.x * player.FacingDirection;
        player.RB.linearVelocity = ledgeUpdir * 1.3f;
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.Anim.SetBool("LedgeUp", false);
    }
    public override void Exit()
    {
        base.Exit();
        player.transform.position = endPosition;
    }
    
}
