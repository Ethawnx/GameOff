using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Do()
    {
        base.Do();
        
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.RB.linearVelocity = new Vector2(player.RB.linearVelocity.x, playerData.jumpHeight);
        
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedDo()
    {
        base.FixedDo();
        player.Run();
    }
}
