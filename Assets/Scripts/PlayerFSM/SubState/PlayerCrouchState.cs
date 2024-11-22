using UnityEngine;

public class PlayerCrouchState : PlayerGroundState
{
    public PlayerCrouchState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }
    public override void Enter() 
    {
        base.Enter();
        player.SetColliderHeight(playerData.crouchColliderOffset, playerData.crouchColliderSize);
    }
    public override void Do()
    {
        base.Do();
        if (!isExitingState) 
        {
            if (InputManager.Movement.y >= 0) 
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
    public override void FixedDo()
    {
        base.FixedDo();
        player.RB.linearVelocityX = 0f;
        player.RB.AddForceY(-9.8f);
    }
    public override void Exit() 
    {
        base.Exit();
        player.ResetColliderHeight();
    }
}
