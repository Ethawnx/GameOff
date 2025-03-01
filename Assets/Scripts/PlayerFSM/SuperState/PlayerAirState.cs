using UnityEngine;

public class PlayerAirState : PlayerState
{
    Vector2 gravity;
    float _fallSpeedDampingYThreshold;
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Do()
    {
        base.Do();

        player.Anim.SetFloat("YVelocity", player.RB.linearVelocityY);

        player._coyoteTimer -= Time.deltaTime;

        if (player.RB.linearVelocityY < _fallSpeedDampingYThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }
        if (player.RB.linearVelocityY >= 0f && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpedFromPlayerFalling = false;

            CameraManager.instance.LerpYDamping(false);
        }

        if (InputManager.JumpWasReleased && player.RB.linearVelocityY > 0f)
        {
            player.RB.linearVelocityY = player.RB.linearVelocityY * 0.5f;
        }

        if (InputManager.Movement.x != 0)
            player.CheckDirectionToFace(InputManager.Movement.x > 0);

        if (player.CheckIfGrounded() && player.RB.linearVelocityY < 0.1f && InputManager.Movement.x == 0f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (player.CheckIfGrounded() && player.RB.linearVelocityY < 0.1f && InputManager.Movement.x != 0f)
        {
            stateMachine.ChangeState(player.RunState);
        }
        else if (InputManager.TeleportWasPressed)
        {
            stateMachine.ChangeState(player.TeleportState);
        }
        else if (player.CheckIfTouchingWall() && InputManager.Movement.x == 1 * player.FacingDirection && player.RB.linearVelocityY < 1f && !player.CheckIfOnLedge())
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (player.CheckIfOnLedge() && InputManager.Movement.x == 1 * player.FacingDirection)
        {
            stateMachine.ChangeState(player.LedgeUpState);
        }
        else if (InputManager.JumpWasPressed && !player.CheckIfGrounded() && player._coyoteTimer > 0f ) 
        {
            stateMachine.ChangeState(player.JumpState);
        }
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        _fallSpeedDampingYThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
        gravity = new Vector2(0, 9.8f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedDo()
    {
        base.FixedDo();
        player.Run();
        player.RB.AddForceY(-9.8f, ForceMode2D.Force);
        if (player.RB.linearVelocityY < 0f)
        {
            player.RB.linearVelocity -= playerData.fallGravityMult * Time.fixedDeltaTime * gravity;
            player.RB.linearVelocityY = Mathf.Clamp(player.RB.linearVelocityY, -playerData.maxFallSpeed, 40f);
        }
        else if (player.RB.linearVelocityY > 0f)
        {
            player.RB.linearVelocityY -= gravity.y * Time.fixedDeltaTime;
        }
    }
}
