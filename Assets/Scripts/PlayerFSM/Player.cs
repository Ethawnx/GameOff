using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player: MonoBehaviour 
{
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerAbilityState AbilityState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerTeleportState TeleportState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }

    #region Components
    public Animator Anim { get; private set; }
    public InputManager InputManager { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public BoxCollider2D BodyCollider { get; private set; }
    
    #endregion

    public float LastOnGroundTime { get; private set; }
    public bool IsFacingRight { get; private set; }
    public int FacingDirection { get; private set; }
    public Vector2 TeleportPosition { get; private set; }
    public Vector2 HitPosition { get; private set; }

    [SerializeField]
    private Transform _groudncheck;
    [SerializeField]
    private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField]
    private Transform _wallCheck;
    [SerializeField]
    private float _wallCheckDistance;

    public float TargetSpeed { get; private set; }
    
    [SerializeField]
    private PlayerData playerData;
    
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "Idle");
        RunState = new PlayerRunState(this, StateMachine, playerData, "Run");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "Jump");
        AirState = new PlayerAirState(this, StateMachine, playerData, "Air");
        AbilityState = new PlayerAbilityState(this, StateMachine, playerData, "Ability");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "WallGrab");
        TeleportState = new PlayerTeleportState(this, StateMachine, playerData, "Teleport");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "Slide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "Jump");

        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        BodyCollider = GetComponent<BoxCollider2D>();
        InputManager = GetComponentInChildren<InputManager>();
    }
    private void Start()
    {
        StateMachine.Initialize(IdleState);
        IsFacingRight = true;
        FacingDirection = 1;
    }
    private void Update()
    {
        Debug.Log(CheckIfGrounded());

        LastOnGroundTime -= Time.deltaTime;

        StateMachine.CurrentState.Do();
    }
    private void FixedUpdate()
    {
        StateMachine.CurrentState.FixedDo();
    }
    public void Run()
    {
        TargetSpeed = InputManager.Movement.x * playerData.runMaxSpeed; 
        TargetSpeed = Mathf.Lerp(RB.linearVelocity.x, TargetSpeed, 1f);

        float accelRate;

        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(TargetSpeed) > 0.01f) ? playerData.runAccelAmount : playerData.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(TargetSpeed) > 0.01f) ? playerData.runAccelAmount * playerData.accelInAir : playerData.runDeccelAmount * playerData.deccelInAir;

        if (playerData.doConserveMomentum && Mathf.Abs(RB.linearVelocity.x) > Mathf.Abs(TargetSpeed) && Mathf.Sign(RB.linearVelocity.x) == Mathf.Sign(TargetSpeed) && Mathf.Abs(TargetSpeed) > 0.01f && CheckIfGrounded())
		{
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 0;
		}

        float speedDif = TargetSpeed - RB.linearVelocity.x;
        float movement = speedDif * accelRate;

        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
        
    }
    public bool CheckIfCanTP() 
    {
        Vector2 offset = new Vector2(0.4f * FacingDirection, 0.3f);
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + offset, (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized, playerData.teleportRange);
        Debug.DrawRay((Vector2)transform.position + offset, (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * playerData.teleportRange);
        if (hit.collider != null && hit.collider.CompareTag("Enemy")) 
        {
            Vector2 detectedEnemyPos = hit.collider.transform.position;
            int enemyDir = hit.collider.GetComponent<Enemy>().GetFacingDirection();
            Vector2 tpPos = detectedEnemyPos + playerData.tpOffset * (-enemyDir);
            TeleportPosition = new Vector2(tpPos.x, detectedEnemyPos.y);
            return true;
        }
        else 
        {
            HitPosition = hit.point;
            return false;
        }
    }
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapBox(_groudncheck.position, _groundCheckSize, 0f, playerData.groundMask);
    }
    public bool CheckIfTouchingWall()
    {
        Vector2 offset = new Vector2(0.3f * FacingDirection, 0f);
        return Physics2D.Raycast((Vector2)_wallCheck.position + offset, Vector2.right * FacingDirection, _wallCheckDistance, playerData.groundMask);
    }
    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Turn();
        }
    }
    public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
			Turn();
	}
    private void Turn()
	{
		//stores scale and flips the player along the x axis, 
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;

		IsFacingRight = !IsFacingRight;
        FacingDirection *= -1;
	}
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groudncheck.position, _groundCheckSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerData.teleportRange);
        Gizmos.color = Color.black;
        Vector2 offset = new Vector2(0.3f * FacingDirection, 0f);
        Gizmos.DrawRay((Vector2)_wallCheck.position + offset, _wallCheckDistance * FacingDirection * Vector2.right);
    }

    public void SetLastOnGroundTime(float amount) 
    {
        LastOnGroundTime = amount;
    }
}
