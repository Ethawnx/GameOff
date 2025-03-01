using UnityEngine;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Do()
    {
        base.Do();
        if (InputManager.Movement.x != 0)
            player.CheckDirectionToFace(InputManager.Movement.x > 0);
        if (!isExitingState)
        {
            if (InputManager.Movement.x == 0f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (InputManager.Movement.x != 0f && player.IsRunning)
            {
                stateMachine.ChangeState(player.RunState);
            }
        }
    }
    public override void FixedDo()
    {
        base.FixedDo();
        player.Run();
    }
}
