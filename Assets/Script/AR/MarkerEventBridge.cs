using UnityEngine;
using Vuforia;

public class MarkerEventBridge : MonoBehaviour
{
    public string markerName;
    public MarkerUIManager uiManager;
    private ObserverBehaviour observer;

    private void Start()
    {
        observer = GetComponent<ObserverBehaviour>();
        if (observer != null)
        {
            observer.OnTargetStatusChanged += OnTargetChanged;
        }
    }

    private void OnTargetChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        Debug.Log($"[MarkerBridge] Marker: {markerName}, Status: {status.Status}, Info: {status.StatusInfo}");

        if (status.Status == Status.TRACKED)
        {
            uiManager.ShowContent(markerName);
        }
        else
        {
            uiManager.HideContent();
        }
    }
}
