using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;
public class NoteUIController : MonoBehaviour
{
    VisualElement detail_Panel;
    VisualElement Container2;

    VisualElement[] ElementSheets;

    public VisualElement UI;
    public void Awake()
    {
        UI = GetComponent<UIDocument>().rootVisualElement;
    }
    public void Start()
    {
        detail_Panel = UI.Q<VisualElement>("Detail-Panel");
        Container2 = UI.Q<VisualElement>("Container2");
        

        CloseDetailPanel();

        ElementSheets = UI.Query<VisualElement>().ToList().Where(element => element.name.EndsWith("Sheet")).ToArray();

        foreach (var elementSheet in ElementSheets)
        {
            elementSheet.RegisterCallback<MouseUpEvent>(OnElementsClicked);
        }

        var ExitDetailPanelButton = UI.Q<Button>("ExitDetailPanelButton");
        ExitDetailPanelButton.clicked += OnExitButtonClicked;
    }
    public void Update()
    {
    
    }
    private void OnExitButtonClicked()
    {
        CloseDetailPanel();
    }
    private void OnElementsClicked(MouseUpEvent evt)
    {
        OpenDetailPanel();

        var clickedElement = evt.target as VisualElement;
        string clickedElementStart = GetStartString(clickedElement.name);

        var matchingElement = FindMatchingDetail(clickedElementStart);
        if (matchingElement != null)
        {
            HideAllDetailSheets();

            matchingElement.style.display = DisplayStyle.Flex;
        }
        else
            Debug.LogWarning($"No matching detail found for: {clickedElementStart}");
    }
    private string GetStartString(string clickedElementName)
    {
        // Extract the starting string (e.g., "Detail1" from "Button_Detail1")
        // Assumes the naming convention is "Prefix_StartString"
        int dashIndex = clickedElementName.IndexOf('-');
        if (dashIndex >= 0)
        {
            return clickedElementName.Substring(0, dashIndex);
        }
        return clickedElementName; // Fallback if no underscore is found
    }
    private VisualElement FindMatchingDetail(string startString)
    {
        var detailSheets = UI.Query<VisualElement>(className: "detail-Sheet").ToList();
        foreach (var detailSheet in detailSheets)
        {
            if (detailSheet.name.StartsWith(startString))
            {
                return detailSheet;
            }
        }
        return null;
    }
    private void HideAllDetailSheets()
    {
        // Hide all detail elements
        var detailElements = UI.Query<VisualElement>(className: "detail-Sheet").ToList();
        foreach (var detail in detailElements)
        {
            detail.style.display = DisplayStyle.None;
        }
    }
    private void OpenDetailPanel()
    {
        detail_Panel.SetEnabled(true);
        detail_Panel.pickingMode = PickingMode.Position;
        Container2.pickingMode = PickingMode.Position;
    }
    public void CloseDetailPanel()
    {
        detail_Panel.SetEnabled(false);
        detail_Panel.pickingMode = PickingMode.Ignore;
        Container2.pickingMode = PickingMode.Ignore;
    }
}
