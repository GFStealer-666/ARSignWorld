using UnityEngine;

[CreateAssetMenu(fileName = "NewPageData", menuName = "Learning/Page Data")]
public class PageDataSO : ScriptableObject
{
    public string pageId;  // Unique identifier for this page
    public string audioPath;
}