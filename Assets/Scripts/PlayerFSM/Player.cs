using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player: MonoBehaviour 
{
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerAbilityState AbilityState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerTeleportState TeleportState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeUpState LedgeUpState { get; private set; }
    public PlayerRollState RollState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerDaggerAttackState DaggerAttackState { get; private set; }

    #region Components
    public Animator Anim { get; private set; }
    public InputManager InputManager { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public BoxCollider2D BodyCollider { get; private set; }
    
    #endregion

    public float TargetSpeed { get; private set; }
    public float LastOnGroundTime { get; private set; }
    public bool IsFacingRight { get; private set; }
    public int FacingDirection { get; private set; }
    public Vector2 TeleportPosition { get; private set; }
    public Vector2 HitPosition { get; private set; }
    public Vector2 CornerPosition { get; private set; }
    public bool IsInvulnerable {  get; private set; }
    public bool IsRunning { get; private set; }
    public Vector3 Mouseposition { get; private set; }
    [Header("VFX References")]    
    public GameObject SlideVFX;
    public GameObject GroundSlideVFX;

    [SerializeField]
    private Transform _groudncheck;
    [SerializeField]
    private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField]
    private Transform _bumpedHead;
    [SerializeField]
    private Vector2 _bumpedHeadSize = new Vector2(0.49f, 0.03f);
    [SerializeField]
    private Transform _wallCheck;
    [SerializeField]
    private float _wallCheckDistance;
    [SerializeField]
    private Transform _ledgeCheck;
    [SerializeField]
    private float _ledgeCheckDistance;
    [SerializeField]
    private Transform _attackPoint;
    
    [SerializeField]
    private PlayerData playerData;

    private Vector2 defaultColliderSize;
    private Vector2 defaultColliderOffset;
    private float defaultgroundCheckSize;

    //JumpBuffer Vars
    [HideInInspector]
    public float _jumpBufferTimer;
    //Coyote Timer Vars
    [HideInInspector]
    public float _coyoteTimer;

    //DaggerVariables
    public int numberOfElectricalCharges;

    public CameraFollowObject camfollowobj;
    public CinemachinePositionComposer Cam;

    private IInteractable currentInteractable;
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "Idle");
        RunState = new PlayerRunState(this, StateMachine, playerData, "Run");
        WalkState = new PlayerWalkState(this, StateMachine, playerData, "Walk");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "Jump");
        AirState = new PlayerAirState(this, StateMachine, playerData, "Air");
        AbilityState = new PlayerAbilityState(this, StateMachine, playerData, "Ability");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "WallGrab");
        TeleportState = new PlayerTeleportState(this, StateMachine, playerData, "Teleport");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "Slide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "Jump");
        LedgeUpState = new PlayerLedgeUpState(this, StateMachine, playerData, "LedgeUp");
        RollState = new PlayerRollState(this, StateMachine, playerData, "Roll");
        CrouchState = new PlayerCrouchState(this, StateMachine, playerData, "Crouch");
        DaggerAttackState = new PlayerDaggerAttackState(this, StateMachine, playerData, "Attack");

        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        BodyCollider = GetComponent<BoxCollider2D>();
        InputManager = GetComponentInChildren<InputManager>();
    }
    private void Start()
    {
        Anim.SetBool("IsDead", false);
        StateMachine.Initialize(IdleState);
        IsFacingRight = true;
        FacingDirection = 1;
        defaultColliderOffset = BodyCollider.offset;
        defaultColliderSize = BodyCollider.size;
        defaultgroundCheckSize = _groundCheckSize.y;
        numberOfElectricalCharges = playerData.daggerCharges;
        IsInvulnerable = false;
        IsRunning = false;
    }
    private void Update()
    {
        LastOnGroundTime -= Time.deltaTime;

        Vector3 mousePos = Input.mousePosition;
        mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
        mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height);
        Mouseposition = Camera.main.ScreenToWorldPoint(mousePos);


        if (InputManager.JumpWasPressed)
        {
            _jumpBufferTimer = playerData.jumpBufferTime;
        }
        else 
        {
            _jumpBufferTimer -= Time.deltaTime;
        }

        Interact();
        
        StateMachine.CurrentState.Do();
    }
    private void FixedUpdate()
    {
        StateMachine.CurrentState.FixedDo();
    }
    public void Run()
    {
        TargetSpeed = IsRunning? InputManager.Movement.x * playerData.runMaxSpeed : InputManager.Movement.x * playerData.walkMaxSpeed; 
        TargetSpeed = Mathf.Lerp(RB.linearVelocity.x, TargetSpeed, 1f);

        float accelRate;

        if (CheckIfGrounded() && IsRunning)
            accelRate = (Mathf.Abs(TargetSpeed) > 0.01f) ? playerData.runAccelAmount : playerData.runDeccelAmount;
        else if (CheckIfGrounded() && !IsRunning)
            accelRate = (Mathf.Abs(TargetSpeed) > 0.01f) ? playerData.walkAccelAmount : playerData.walkDeccelAmount;
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
    public void Interact() 
    {
        if (InputManager.InteractKeyWasPressed && currentInteractable != null) 
        {
            currentInteractable.OnInteract();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out var interactable))
        {
            currentInteractable = interactable;
            currentInteractable.PopUI();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable) 
        {
            currentInteractable.CloseUI();
            currentInteractable = null;
        }
    }
    public void ChargeDagger() 
    {
        numberOfElectricalCharges = playerData.daggerCharges;
    }
    public void Attack() 
    {
        Collider2D enemy = Physics2D.OverlapCircle(_attackPoint.transform.position, playerData.attackRadius, playerData.enemyLayerMask);
        if (enemy != null) 
        {
            Enemy foundEnemy = enemy.GetComponent<Enemy>();
            foundEnemy.TakeDamage(playerData.attackDamage, CheckIfDaggerHasCharges());
            numberOfElectricalCharges--;
            Debug.Log("attacked");
        }

    }
    public void DetermineCornerPosition() 
    {
        RaycastHit2D hitX = Physics2D.Raycast((Vector2)_wallCheck.position, Vector2.right * FacingDirection, _wallCheckDistance, playerData.groundMask);
        float x = hitX.point.x;
        RaycastHit2D hitY = Physics2D.Raycast((Vector2)_ledgeCheck.transform.position + new Vector2(0.1f * FacingDirection, 0f), Vector2.down, playerData.groundMask);
        float y = hitY.point.y - 0.05f;
        CornerPosition = new Vector2(x, y);
    }
    #region CheckFunctions
    public bool CheckIfDaggerHasCharges()
    {
        if (numberOfElectricalCharges > 0) 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
    public bool CheckIfBumpedHead()
    {
        return Physics2D.OverlapBox(_bumpedHead.position, _bumpedHeadSize, 0f, playerData.groundMask);
    }
    public bool CheckIfCanTP()
    {
        Vector2 offset = new Vector2(0.4f * FacingDirection, 0.3f);
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + offset, (Mouseposition - transform.position).normalized, playerData.teleportRange,playerData.enemyLayerMask);
        Debug.DrawRay((Vector2)transform.position + offset, (Mouseposition - transform.position).normalized * playerData.teleportRange);
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
        return Physics2D.Raycast((Vector2)_wallCheck.position, Vector2.right * FacingDirection, _wallCheckDistance, playerData.groundMask);
    }
    public bool CheckIfOnLedge()
    {
        bool ledgeCheck = !Physics2D.Raycast((Vector2)_ledgeCheck.transform.position, Vector2.right * FacingDirection, _wallCheckDistance, playerData.groundMask);
        if (CheckIfTouchingWall() && ledgeCheck)
        {
            return true;
        }
        else
        {
            return false;
        }
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
    #endregion
    public void SetColliderHeight(float offset, float size) 
    {
        BodyCollider.offset = new Vector2 (BodyCollider.offset.x, offset);
        BodyCollider.size = new Vector2 (BodyCollider.size.x, size);
        _groundCheckSize.y = 0.30f;
    }
    
    public void ResetColliderHeight() 
    {
        BodyCollider.offset = defaultColliderOffset;
        BodyCollider.size = defaultColliderSize;
        _groundCheckSize.y = defaultgroundCheckSize;
    }
    private void Turn()
	{
		//stores scale and flips the player along the x axis, 
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;

		IsFacingRight = !IsFacingRight;
        FacingDirection *= -1;
        camfollowobj.CallTurn();
    }
    public void SetVulnerability(bool isVulnerable) 
    {
        IsInvulnerable = isVulnerable;
    }
    public bool IsOnRun() 
    {
        if (InputManager.RunIsPressed) 
        {
            IsRunning = !IsRunning;
        }
        return IsRunning;
    }
    public void Die() 
    {
        Anim.SetTrigger("IsDEAD");
        RB.linearVelocity = Vector3.zero;
    }
    public void ResetGame()
    {
        Anim.SetBool("IsDead", false);
        SceneManager.LoadScene("Corp");
    }
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groudncheck.position, _groundCheckSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerData.teleportRange);
        Gizmos.color = Color.black;
        Gizmos.DrawRay((Vector2)_wallCheck.position, _wallCheckDistance * FacingDirection * Vector2.right);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay((Vector2)_ledgeCheck.position, _ledgeCheckDistance * FacingDirection * Vector2.right);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_bumpedHead.position, _bumpedHeadSize);
    }
    private void AnimationFinishedTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
}
