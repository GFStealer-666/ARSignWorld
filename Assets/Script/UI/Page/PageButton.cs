using UnityEngine;

public class PageButton : MonoBehaviour
{
    [SerializeField] private PageDataSO pageData;  // Direct reference to SO
    [SerializeField] private PageManager pageManager;

    public void SendPageData()
    {
        if (pageData != null)
        {
            pageManager.OpenPageById(pageData.pageId);
        }
        else
        {
            Debug.LogError($"[PageButton] No PageDataSO assigned to button {gameObject.name}");
        }
    }
}
