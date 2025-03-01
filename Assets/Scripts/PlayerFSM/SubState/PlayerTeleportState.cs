using Unity.Cinemachine;
using UnityEngine;

public class PlayerTeleportState : PlayerAbilityState
{
    private float initialOrthographicSize;
    private float slowMotionDuration;
    private float timeElapsed;
    private LineRenderer lineHint;
    private LineRenderer circleHint;
    private SpriteRenderer spritHint;
    private GameObject cameraFollowObject;
    private int segment = 100;
    private Vector3 defaultYdamp;
    public PlayerTeleportState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        timeElapsed = 0f;
        Time.timeScale = 0.2f;

        slowMotionDuration = playerData.SlowMotionDuration;

        cameraFollowObject = GameObject.FindGameObjectWithTag("FollowObject");

        LineRenderer[] renderers = player.GetComponentsInChildren<LineRenderer>();

        SpriteRenderer[] spriteRenderers = player.GetComponentsInChildren<SpriteRenderer>();

        spritHint = spriteRenderers[1];

        circleHint = renderers[1];
        lineHint = renderers[0];
        lineHint.enabled = false;
        circleHint.enabled = false;
        spritHint.enabled = false;

        circleHint.positionCount = segment + 1;

        player.RB.linearVelocity = Vector2.zero;

        defaultYdamp = player.Cam.Damping;
    }
    public override void Do()
    {
        base.Do();

        if (!isExitingState)
        {

            if (timeElapsed > slowMotionDuration || !InputManager.TeleportIsHeld)
            {
                Camera.main.orthographicSize = initialOrthographicSize;
                player.Cam.TargetOffset.Set(0f, 0f, 0f);
                timeElapsed = 0f;
                isAbilityDone = true;
                if (player.CheckIfCanTP())
                {
                    player.transform.position = player.TeleportPosition;
                }
            }
            else 
            {
                Draw();
                MoveCamera();
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
        Time.timeScale = 1f;
        timeElapsed = 0f;
        lineHint.enabled = false;
        circleHint.enabled= false;
        spritHint.enabled = false;
        Camera.main.orthographicSize = initialOrthographicSize;
        player.Cam.TargetOffset.Set(0.5f, 0f, 0f);
        player.Cam.Damping = defaultYdamp;
        isAbilityDone = true;
    }
    private void MoveCamera() 
    {
        timeElapsed += Time.unscaledDeltaTime;
        Vector2 difference = player.Mouseposition - cameraFollowObject.transform.position;
        //difference.Normalize();
        player.Cam.Damping = new Vector3(1f, 1f, 1f);
        player.Cam.TargetOffset = difference * player.FacingDirection;
    }
    private void Draw() 
    {
        //RaycastHit2D hit = Physics2D.Raycast((Vector2)player.transform.position, (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position).normalized);
        lineHint.enabled = true;
        circleHint.enabled = true;
        CreateCircle();
        Vector2 offset = new(0.2f * player.FacingDirection, 0.3f);
        lineHint.SetPosition(0, (Vector2)player.transform.position + offset);
        if (player.CheckIfCanTP()) 
        {
            spritHint.enabled = true;
            spritHint.transform.position = player.TeleportPosition;
            lineHint.endColor = Color.green;
            circleHint.startColor = Color.green;
            circleHint.endColor = Color.green;
            lineHint.SetPosition(1, player.TeleportPosition);

        }
        else 
        {
            Vector2 direction = (player.Mouseposition - player.transform.position);
            float distance = direction.magnitude;
            if (distance > playerData.teleportRange)
            {
                direction = direction.normalized * playerData.teleportRange;
            }
            lineHint.endColor = Color.red;
            circleHint.startColor = Color.red;
            circleHint.endColor = Color.red;
            lineHint.SetPosition(1, (Vector2)player.transform.position + direction);
        }
    }
    void CreateCircle() 
    { 

        float angle = 2 * Mathf.PI / segment;
        for (int i = 0; i <= segment; i++) 
        { 
            float x = Mathf.Cos(i * angle) * playerData.teleportRange;
            float y = Mathf.Sin(i * angle) * playerData.teleportRange;
            circleHint.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}


