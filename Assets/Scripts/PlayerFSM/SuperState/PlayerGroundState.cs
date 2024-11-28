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

        player._coyoteTimer = playerData.coyoteTime;

        if (player._jumpBufferTimer > 0f && player._coyoteTimer > 0f) 
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!player.CheckIfGrounded() && Mathf.Abs(player.RB.linearVelocityY) > 0f)
        {
            stateMachine.ChangeState(player.AirState);
        }
        else if (InputManager.TeleportWasPressed) 
        {
            stateMachine.ChangeState(player.TeleportState);
        }
        else if (InputManager.AttackWasPressed) 
        {
            stateMachine.ChangeState(player.DaggerAttackState);
        }
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        
    }
    public override void FixedDo()
    {
        base.FixedDo();
        
    }
}
