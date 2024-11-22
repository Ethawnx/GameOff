using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    protected float startTime;

    protected bool isExitingState;
    protected bool isAnimationFinished;

    private string animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        DoChecks();
        player.Anim.SetBool(animBoolName,true);
        startTime = Time.time;
        Debug.Log(animBoolName);
        isAnimationFinished = false;
        isExitingState = false;
    }
    public virtual void Do()
    {

    }
    public virtual void FixedDo()
    {
        DoChecks();
    }
    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }
    public virtual void DoChecks()
    {

    }
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
