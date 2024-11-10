using UnityEngine;

public class PlayerRunState :PlayerGroundState
{
    public PlayerRunState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedDo()
    {
        base.FixedDo();
    }
}