using UnityEngine;

public class PlayerIdleState: PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }
    public override void Do()
    {
        base.Do();
        
        if (!isExitingState)
        {
            if (InputManager.Movement.x != 0f)
            {
                stateMachine.ChangeState(player.RunState);
            }
            else if (InputManager.Movement.y == -1f) 
            {
                stateMachine.ChangeState(player.CrouchState);
            }
        }
    }
    public override void FixedDo()
    {
        base.FixedDo();
        player.RB.linearVelocity = Vector2.zero;
    }
}
