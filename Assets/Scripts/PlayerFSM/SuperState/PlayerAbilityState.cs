using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;
    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Do()
    {
        base.Do();
        if (isAbilityDone)
        {
            if (player.CheckIfGrounded() && player.RB.linearVelocityY <= 0.0f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.AirState);
            }
        }
    }
    public override void Enter()
    {
        base.Enter();
        isAbilityDone = false;
    }

}
