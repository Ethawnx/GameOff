using UnityEngine;

public class PlayerTeleportState : PlayerAbilityState
{
    public PlayerTeleportState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.RB.linearVelocity = Vector2.zero;
        player.transform.position = player.TeleportPosition;
        isAbilityDone = true;
    }
}
