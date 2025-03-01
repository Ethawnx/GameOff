using UnityEngine;

public class NotesBoard : MonoBehaviour, IInteractable 
{
    public GameObject UI;
    public float AnimationDuration;
    public void Start()
    {
        UI.transform.localScale = new Vector3(0f, 1f, 1f);
        if (UI != null)
        {
            UI.SetActive(false);
        }
    }
    public void OnInteract()
    {
        UIManager.Instance.OpenNoteUI();
    }
    public void PopUI() 
    {
        UI.SetActive(true);
        LeanTween.scaleX(UI, 1f, AnimationDuration).setEaseInOutCubic();
    }
    public void CloseUI()
    {
        if (UI != null)
        {
            LeanTween.scaleX(UI, 0f, AnimationDuration).setEaseInOutCubic();
        }
    }
}
