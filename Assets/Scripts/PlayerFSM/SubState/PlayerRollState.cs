using System.Collections;
using UnityEngine;

public class PlayerRollState :PlayerGroundState
{
    bool isRolling;
    float rollTime;
    //int numberOfAlowedRolls;
    public PlayerRollState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.BodyCollider.isTrigger = true;
        player.RB.gravityScale = 0;
        rollTime = playerData.rollDuration;
        isRolling = true;
    }
    public override void Do()
    {
        base.Do();
        //Debug.Log(rollTime);
        //Debug.Log("CanRoll: " + CanRoll());
        //Debug.Log("isRolling: " + isRolling);
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
            //else if (InputManager.Movement.y )
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
        player.BodyCollider.isTrigger = false;
        isRolling = false;
        rollTime = playerData.rollDuration;
        player.RB.gravityScale = 1;
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
