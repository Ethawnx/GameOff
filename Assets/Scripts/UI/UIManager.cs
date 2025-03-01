using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class UIManager : MonoBehaviour
{
    // Static instance of UIManager
    private static UIManager _instance;

    // UI Documents References
    public UIDocument PauseUI;
    public UIDocument NoteUI;

    private VisualElement _pauseUI;
    private VisualElement _noteUI;

    private NoteUIController _noteUIController;
    // GameStates
    public bool IsPaused { get; private set; }

    // Public property to get the instance of UIManager
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // If no instance exists, find one in the scene
                _instance = FindFirstObjectByType<UIManager>();

                if (_instance == null)
                {
                    // If still no instance, create a new GameObject and add UIManager component
                    GameObject uiManager = new GameObject("UIManager");
                    _instance = uiManager.AddComponent<UIManager>();
                }
            }

            return _instance;
        }
    }

    // Awake method to enforce the singleton pattern
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy the new one
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance to this one
        _instance = this;

        // Optionally, make this instance persistent across scenes
        DontDestroyOnLoad(gameObject);

        _pauseUI = PauseUI.rootVisualElement;
        _noteUI = NoteUI.rootVisualElement;
    }

    private void Start()
    {
        _noteUIController = GetComponentInChildren<NoteUIController>();

        _pauseUI.style.display = DisplayStyle.None;
        _noteUI.style.display = DisplayStyle.None;
    }

    public void Update()
    {
        if (InputManager.MenuOpenWasPressed)
        {
            if (!IsPaused)
            {
                _pauseUI.style.display = DisplayStyle.Flex;
                Pause();
            }
        }
        else if (InputManager.MenuCloseWasPressed) 
        {
            if (IsPaused)
            {
                _pauseUI.style.display = DisplayStyle.None;
                _noteUI.style.display = DisplayStyle.None;
                _noteUIController.CloseDetailPanel();
                Unpause();
            }
        }
    }
    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        InputManager.playerInput.SwitchCurrentActionMap("UI");
    }
    public void Unpause()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        InputManager.playerInput.SwitchCurrentActionMap("Player");
    }
    public void OpenNoteUI()
    {
        Pause();
        _noteUI.style.display = DisplayStyle.Flex;
    }
}

