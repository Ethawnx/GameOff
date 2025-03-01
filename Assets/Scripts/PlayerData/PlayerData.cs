using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Data")] //Create a new playerData object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the player
public class PlayerData : ScriptableObject
{
    public float Gravity { get; private set; }
    public float InitialJumpVelocity { get; private set; }
	public float AdjustedJumpHeight { get; private set; }

	[Header("VFX References")]
	public GameObject SlideVFX;

    [Header("Attack")]
	public float attackRadius = 1f;
    public int daggerCharges = 3;
	public float attackCooldown = 0.3f;
	public float attackDamage = 50f;
	public Vector2 attackVelocity = new Vector2(2f, 0f);

    [Header("Roll")]
	public float rollVelocity = 10f;
	public float rollDuration = 2f;
	public int numberOfRolls = 1;
	public float crouchColliderOffset = -0.13f;
	public float crouchColliderSize = 0.75f;

	[Header("LedgeUp")]
	public Vector2 StartPositionOffset = new Vector2(0.9f, 0f);

	[Header("Teleport")]
	public GameObject SpriteHint;
	public float OrthoGraphicSize = 4f;
	public float SlowMotionDuration = 2f;
	public float teleportRange = 50f;
	public Vector2 tpOffset = new Vector2(1, 0f);

	[Header("JumpState")]
	public float jumpHeight = 6.5f; //Height of the player's jump
	[Range(1f, 1.1f)] public float jumpHeightCompensationFactor = 1.054f;
	public float timeTillJumpApex = 0.35f;
	[Range(0.01f, 5f)] public float gravityOnReleaseMultiplyer = 2f;
	public float maxFallSpeed = 26f;
	[Range(0f, 1f)] public float jumpBufferTime = 0.125f;
	[Range(0f, 1f)] public float coyoteTime = 0.4f;
	public float fallGravityMult; //Multiplier to the player's gravityScale when falling.

	[Header("Wall Slide State")]
	public float wallSlideVelocity = 3f;

	[Header("Wall Jump State")]
	public float wallJumpVelocity = 5f;
	public float wallJumpTime = 0.4f;
	public Vector2 wallJumpAngle = new Vector2(1, 2);

	[Header("Check Variables")]
	public float groundCheckRadius;
	public LayerMask groundMask;
	public LayerMask enemyLayerMask;
	public LayerMask obstacleMask;
	public LayerMask playerMask;

	[Space(20)]
	[Header("Gravity")]

	[HideInInspector] public float gravityStrength; //Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.
	[HideInInspector] public float gravityScale; //Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D).
												 //Also the value the player's rigidbody2D.gravityScale is set to.
	[Space(20)]

	[Header("Run")]
	public float runMaxSpeed; //Target speed we want the player to reach.
	public float runAcceleration; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
    [HideInInspector] public float walkAccelAmount;
    [HideInInspector] public float walkDeccelAmount;
    [HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
	public float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
	public float walkMaxSpeed;
	public float walkAcceleration;
	public float walkDecceleration;
	[HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
	[Space(5)]
	[Range(0f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
	[Range(0f, 1)] public float deccelInAir;
	[Space(5)]
	public bool doConserveMomentum = true;

    private void OnEnable()
    {
        CalculateValues();
    }
    //Unity Callback, called when the inspector updates
    private void OnValidate()
	{
		CalculateValues();
        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

		//Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
		runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
		runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;
        walkAccelAmount = (50 * walkAcceleration) / walkMaxSpeed;
        walkDeccelAmount = (50 * walkDecceleration) / walkMaxSpeed;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
		runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
		#endregion
	}

	private void CalculateValues() 
	{
		AdjustedJumpHeight = jumpHeight * jumpHeightCompensationFactor;
		Gravity = -(2f * AdjustedJumpHeight) / Mathf.Pow(timeTillJumpApex, 2f);
		InitialJumpVelocity = Mathf.Abs(Gravity) * timeTillJumpApex;
	}
}
