using UnityEngine;

public class PageUIStateController : MonoBehaviour
{
    // This script manages the visibility of UI elements when a page is opened or closed.
    // It controls the visibility when switching between LearningViewport and LearningContent.
    [SerializeField] private GameObject LearningButtonScrollView;
    [SerializeField] private GameObject LearningPageContent;
    [SerializeField] private GameObject exitToMainmenuButton;

    public void SetPageViewState(bool isPageOpen)
    {
        LearningButtonScrollView.SetActive(!isPageOpen);
        exitToMainmenuButton.SetActive(!isPageOpen);
        if (LearningPageContent == null) return;
        LearningPageContent.SetActive(isPageOpen);
    }
}