using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 holdPos;
    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        holdPos = player.transform.position;
    }
    public override void Do()
    {
        base.Do();
        HoldPosition();
    }
    public override void FixedDo()
    {
        base.FixedDo();
        player.RB.angularVelocity = 0f;
    }
    private void HoldPosition()
    {
        player.transform.position = holdPos;
    }
}
