public class PlayerJumpState :PlayerAbilityState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //player._coyoteTimer = 0f;
        player.RB.linearVelocityY = playerData.InitialJumpVelocity;
        isAbilityDone = true;
    }
    public override void FixedDo()
    {
        base.FixedDo();
    }
}
