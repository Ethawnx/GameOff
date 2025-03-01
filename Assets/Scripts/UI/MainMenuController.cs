using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public VisualElement UI;

    public Button Play;
    public Button Options;
    public Button Exit;

    private void Awake()
    {
        UI = GetComponent<UIDocument>().rootVisualElement;
    }
    private void Start()
    {
        Play = UI.Q<Button>("Play");
        Play.clicked += OnPlayButtonClicked;

        Options = UI.Q<Button>("Options");
        Options.clicked += OnOptionsButtonClicked;

        Exit = UI.Q<Button>("Exit");
        Exit.clicked += OnExitButtonClicked;
    }
    private void OnPlayButtonClicked()
    {
        UIManager.Instance.Unpause();
        UI.style.display = DisplayStyle.None;
    }
    private void OnOptionsButtonClicked()
    {
        Debug.Log("Options");
    }
    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
