using System.Collections;
using Unity.AppUI.UI;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class PlayerTeleportState : PlayerAbilityState
{
    private float targetOrthographicSize;
    private float initialOrthographicSize;
    private float slowMotionDuration;
    private float timeElapsed;
    private LineRenderer lineHint;
    private LineRenderer circleHint;
    private SpriteRenderer spritHint;
    private Vector3 Mouseposition;
    private int segment = 100;
    
    CinemachinePositionComposer cinemachineCam;
    public PlayerTeleportState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        timeElapsed = 0f;
        Time.timeScale = 0.1f;
        slowMotionDuration = playerData.SlowMotionDuration;
        targetOrthographicSize = playerData.OrthoGraphicSize;
        initialOrthographicSize = Camera.main.orthographicSize;
        cinemachineCam = Camera.main.GetComponent<CinemachinePositionComposer>();
        LineRenderer[] renderers = player.GetComponentsInChildren<LineRenderer>();
        SpriteRenderer[] spriteRenderers = player.GetComponentsInChildren<SpriteRenderer>();
        spritHint = spriteRenderers[1];
        lineHint = renderers[0];
        circleHint = renderers[1];
        lineHint.enabled = false;
        circleHint.enabled = false;
        spritHint.enabled = false;
        circleHint.positionCount = segment + 1;
        player.RB.linearVelocity = Vector2.zero;
        //Debug.Log(Time.timeScale);
        //
    }
    public override void Do()
    {
        base.Do();
        Mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!isExitingState)
        {

            if (timeElapsed > slowMotionDuration || !InputManager.TeleportIsHeld)
            {
                Camera.main.orthographicSize = initialOrthographicSize;
                cinemachineCam.TargetOffset.Set(0f, 0f, 0f);
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
                ZoomCamera();
            }

            /* if (timeElapsed > slowMotionDuration || !InputManager.TeleportIsHeld) 
            {
                timeElapsed = 0f;
                Camera.main.orthographicSize = initialOrthographicSize;
                cinemachineCam.TargetOffset.Set(0f, 0f, 0f);
                isAbilityDone = true;
            } 
            else if (timeElapsed > slowMotionDuration && InputManager.TeleportIsHeld)
            {
                timeElapsed = 0f;
                Camera.main.orthographicSize = initialOrthographicSize;
                cinemachineCam.TargetOffset.Set(0f, 0f, 0f);
                isAbilityDone = true;
            }
            else 
            {
                timeElapsed += Time.unscaledDeltaTime;


            }*/
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
        cinemachineCam.TargetOffset.Set(0f, 0f, 0f);
        isAbilityDone = true;
    }
    private void ZoomCamera() 
    {
        timeElapsed += Time.unscaledDeltaTime;
        Vector2 difference = Mouseposition - player.transform.position;
        cinemachineCam.TargetOffset = difference;
        Camera.main.orthographicSize = Mathf.Lerp(initialOrthographicSize, targetOrthographicSize, timeElapsed/slowMotionDuration);
    }
    private void Draw() 
    {
        //RaycastHit2D hit = Physics2D.Raycast((Vector2)player.transform.position, (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position).normalized);
        lineHint.enabled = true;
        circleHint.enabled = true;
        CreateCircle();
        Vector2 offset = new Vector2(0.2f * player.FacingDirection, 0.3f);
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
            Vector2 direction = (Mouseposition - player.transform.position);
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


