using NUnit.Framework.Interfaces;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Do()
    {
        base.Do();

        if (InputManager.JumpWasPressed && player.CheckIfGrounded())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!player.CheckIfGrounded() && Mathf.Abs(player.RB.linearVelocityY) > 0f)
        {
            stateMachine.ChangeState(player.AirState);
        }
        else if (InputManager.TeleportWasPressed && player.CheckIfCanTP()) 
        {
            stateMachine.ChangeState(player.TeleportState);
        }
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetLastOnGroundTime(playerData.coyoteTime);
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
