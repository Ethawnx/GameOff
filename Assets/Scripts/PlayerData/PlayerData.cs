using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Data")] //Create a new playerData object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the player
public class PlayerData : ScriptableObject
{
    [Header("Teleport")]
    public float teleportRange = 50f;
	public Vector2 tpOffset = new Vector2(1, 0f);
	
	[Header("JumpState")]
    public float jumpHeight; //Height of the player's jump
	public float coyoteTime = 0.4f;

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
	[Space(5)]
	public float fallGravityMult; //Multiplier to the player's gravityScale when falling.

	[Space(20)]

	[Header("Run")]
	public float runMaxSpeed; //Target speed we want the player to reach.
	public float runAcceleration; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
	[HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
	public float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
	[HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
	[Space(5)]
	[Range(0f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
	[Range(0f, 1)] public float deccelInAir;
	[Space(5)]
	public bool doConserveMomentum = true;


	//Unity Callback, called when the inspector updates
	private void OnValidate()
	{
		//Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
		gravityScale = gravityStrength / Physics2D.gravity.y;

		//Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
		runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
		runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

		#region Variable Ranges
		runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
		runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
		#endregion
	}
}
