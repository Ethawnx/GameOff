using System.Collections;
using UnityEngine;

public class PlayerRollState :PlayerGroundState
{
    bool isRolling;
    float rollTime;

    ParticleSystem slideVFX;
    public PlayerRollState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.SetVulnerability(true);
        //player.BodyCollider.isTrigger = true;
        player.RB.gravityScale = 0;
        rollTime = playerData.rollDuration;
        isRolling = true;

        slideVFX = player.GroundSlideVFX.GetComponentInChildren<ParticleSystem>();
        slideVFX.Play();
    }
    public override void Do()
    {
        base.Do();
        rollTime -= Time.deltaTime;
        if (!isExitingState)
        {
            if (rollTime < 0 || !player.CheckIfGrounded())
            {
                isRolling = false;
            }

            if (isRolling == false) 
            {
                stateMachine.ChangeState(player.CrouchState);
            }
        }
    }
    public override void FixedDo()
    {
        base.FixedDo();
        if (isRolling) 
        {
            Roll();
        }
    }
    public override void Exit() 
    {
        base.Exit();
        player.SetVulnerability(false);
        player.BodyCollider.isTrigger = false;
        isRolling = false;
        rollTime = playerData.rollDuration;
        player.RB.gravityScale = 1;

        slideVFX.Stop();
    }
    private void Roll() 
    {
        player.RB.linearVelocityX = playerData.rollVelocity * player.FacingDirection;
    }
    public bool CanRoll() 
    {
        if (!isRolling) 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
}
