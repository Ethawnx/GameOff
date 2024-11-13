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
