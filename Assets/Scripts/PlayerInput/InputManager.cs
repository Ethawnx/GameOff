using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;

    public static Vector2 Movement;
    public static Vector2 MousePos;
    public static bool JumpWasPressed;
    public static bool JumpIsHeld;
    public static bool JumpWasReleased;
    public static bool AttackWasPressed;
    public static bool AttackIsHeld;
    public static bool AttackWasReleased;
    public static bool TeleportWasPressed;
    public static bool TeleportIsHeld;
    public static bool TeleportWasReleased;
    public static bool RunIsPressed;
    public static bool InteractKeyWasPressed;
    //UI
    public static bool MenuOpenWasPressed;
    public static bool MenuCloseWasPressed;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _runAction;
    private InputAction _attackAction;
    private InputAction _MousePosAction;
    private InputAction _TeleportAction;
    private InputAction _InteractAction;
    //UI
    private InputAction _menuOpenAction;
    private InputAction _menuCloseAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        _moveAction = playerInput.actions["Move"];
        _jumpAction = playerInput.actions["Jump"];
        _runAction = playerInput.actions["Sprint"];
        _attackAction = playerInput.actions["Attack"];
        _MousePosAction = playerInput.actions["Look"];
        _TeleportAction = playerInput.actions["Teleport"];
        _InteractAction = playerInput.actions["Interact"];

        _menuOpenAction = playerInput.actions["MenuOpen"];
        _menuCloseAction = playerInput.actions["MenuClose"];
    }

    void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();
        MousePos = _MousePosAction.ReadValue<Vector2>();

        JumpWasPressed = _jumpAction.WasPressedThisFrame();
        JumpIsHeld = _jumpAction.IsPressed();
        JumpWasReleased = _jumpAction.WasReleasedThisFrame();
        
        AttackWasPressed = _attackAction.WasPressedThisFrame();
        AttackIsHeld = _attackAction.IsPressed();
        AttackWasReleased = _jumpAction.WasReleasedThisFrame();

        TeleportWasPressed = _TeleportAction.WasPressedThisFrame();
        TeleportIsHeld = _TeleportAction.IsPressed();
        TeleportWasReleased = _TeleportAction.WasReleasedThisFrame();

        RunIsPressed = _runAction.WasPressedThisFrame();

        InteractKeyWasPressed = _InteractAction.WasPressedThisFrame();

        MenuOpenWasPressed = _menuOpenAction.WasPressedThisFrame();
        MenuCloseWasPressed = _menuCloseAction.WasPressedThisFrame();
    }
}
