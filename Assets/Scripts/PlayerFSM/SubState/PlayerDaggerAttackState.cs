using UnityEngine;

public class PlayerDaggerAttackState : PlayerAbilityState
{
    public PlayerDaggerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //isAbilityDone = true;
    }
    public override void Do()
    {
        base.Do();
        if (Time.time >= startTime + playerData.attackCooldown) 
        {
            isAbilityDone = true;
        }
    }
    public override void FixedDo()
    {
        base.FixedDo();
        player.RB.linearVelocityX += playerData.attackVelocity.x * player.FacingDirection;
    }
}
