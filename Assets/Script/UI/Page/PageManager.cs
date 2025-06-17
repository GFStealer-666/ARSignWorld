using UnityEngine;
using System.Collections.Generic;

public class PageManager : MonoBehaviour
{
    [SerializeField] private PageDataSO[] pages;
    [SerializeField] private GameObject[] pageObjects;
    private Dictionary<string, GameObject> pageDict = new Dictionary<string, GameObject>();
    [SerializeField] private PageAudioController pageAudioController;
    [SerializeField] private PageUIStateController pageStateController;
    [SerializeField] private int currentPageIndex;
    private void Awake()
    {
        // Initialize the dictionary
        for (int i = 0; i < pages.Length; i++)
        {
            if (i < pageObjects.Length)
            {
                pageDict[pages[i].pageId] = pageObjects[i];
            }
        }
    }
    public void OpenPageById(string id)
    {
        if (pageDict.TryGetValue(id, out GameObject pageObject))
        {
            DeactivateAllPages();
            // Find and update current index
            for (int i = 0; i < pages.Length; i++)
            {
                if (pages[i].pageId == id)
                {
                    currentPageIndex = i;
                    break;
                }
            }
            // Show this page
            pageObject.SetActive(true);
            pageStateController.SetPageViewState(true);
            
            // Setup audio
            SetUpCurrentPageAudio();
        }
    }
    public void ReturnToViewport()
    {
        DeactivateAllPages();
        currentPageIndex = 0; // Reset to the first page
        pageStateController.SetPageViewState(false);
        
        if (pageAudioController != null)
        {
            pageAudioController.StopAudio();
        }
    }

    public void NextPage()
    {
        currentPageIndex = (currentPageIndex + 1) % pages.Length;
        ShowPageByIndex();
    }

    public void PrevPage()
    {
        currentPageIndex = (currentPageIndex - 1 + pages.Length) % pages.Length;
        ShowPageByIndex();
    }

    private void ShowPageByIndex()
    {
        DeactivateAllPages();
        if (currentPageIndex < pageObjects.Length && pageObjects[currentPageIndex] != null)
            pageObjects[currentPageIndex].SetActive(true);
        SetUpCurrentPageAudio();
    }
    private void DeactivateAllPages()
    {
        foreach (var pageObject in pageObjects)
        {
            if (pageObject != null)
                pageObject.SetActive(false);
        }
    }

    private void SetUpCurrentPageAudio()
    {
        if (pageAudioController != null && currentPageIndex < pages.Length)
        {
            pageAudioController.SetupAudio(pages[currentPageIndex].audioPath, true); // true means start paused
        }
    }
}
