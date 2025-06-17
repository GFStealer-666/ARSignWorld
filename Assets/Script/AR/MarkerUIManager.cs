using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class MarkerUIManager : MonoBehaviour
{
    [SerializeField] private SignPanelController panelController;
    [SerializeField] private MediaController mediaController;
    [SerializeField] private ModelViewController modelViewController;
    
    [Header("Content")]
    [SerializeField] private List<MarkerContent> markerContents;
    private Dictionary<string, MarkerContent> contentDict = new();
    private Dictionary<string, GameObject> trackedObjectsDict = new();
    [Header("Debug")]
    [SerializeField] private TrackedARObject[] trackedARObjects;
    private void Start()
    {
        InitializeTrackedObjects();
        InitializeContentDictionary();
    }

    private void InitializeTrackedObjects()
    {
        trackedARObjects = FindObjectsByType<TrackedARObject>(FindObjectsSortMode.None);
        foreach (var obj in trackedARObjects)
        {
            trackedObjectsDict[obj.name] = obj.gameObject;
        }
    }

    private void InitializeContentDictionary()
    {
        foreach (var content in markerContents)
        {
            contentDict[content.markerName] = content;
        }
    }

    public void ShowContent(string markerName)
    {
        // First validate the content exists
        if (!contentDict.ContainsKey(markerName)) 
        {
            Debug.LogWarning($"Content not found for marker: {markerName}");
            return;
        }
        if (!trackedObjectsDict.TryGetValue(markerName, out GameObject trackedObject))
        {
            Debug.LogWarning($"Tracked object not found for marker: {markerName}");
            return;
        }
        var content = contentDict[markerName];
        var trackedData = trackedObject.GetComponent<TrackedARObject>();
        // Reset previous state
        if (panelController != null)
        {
            panelController.HidePanel();
        }
        if (modelViewController != null)
        {
            modelViewController.SetInteractable(false);
        }
        if (mediaController != null)
        {
            mediaController.StopMedia();
        }

        // Setup new content
        
        
        if (trackedData != null)
        {
            modelViewController.Setup(trackedData.modelCanvas, trackedData.piviotPoint);
        }

        string fullPath = Path.Combine(Application.streamingAssetsPath, content.videoPath);
        mediaController.SetVideo(fullPath);

        // Enable interactions in correct order
        modelViewController.SetInteractable(true);
        panelController.ShowPanel();
    }

    public void HideContent()
    {
        try
        {
            if (mediaController != null)
            {
                mediaController.StopMedia();
            }

            if (modelViewController != null)
            {
                modelViewController.SetInteractable(false);
            }

            if (panelController != null)
            {
                panelController.HidePanel();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in HideContent: {e.Message}");
        }
    }
}