using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class MultiImageTracker : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager _trackedImageManager;
    [SerializeField] private List<GameObject> prefabsToSpawn = new List<GameObject>();
    [SerializeField] private GameObject maincanvas;
    private Dictionary<string, GameObject> _arObjects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (_trackedImageManager == null)
            _trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
#if AR_FOUNDATION_6_0_OR_NEWER
        _trackedImageManager.trackables.trackablesChanged += OnImageTrackChanged;
#else
        _trackedImageManager.trackedImagesChanged += OnImageTrackChanged;
#endif
        SetupSceneElement();
    }

    private void OnDisable()
    {
#if AR_FOUNDATION_6_0_OR_NEWER
        _trackedImageManager.trackables.trackablesChanged -= OnImageTrackChanged;
#else
        _trackedImageManager.trackedImagesChanged -= OnImageTrackChanged;
#endif
    }

    private void SetupSceneElement()
    {
        foreach (var prefab in prefabsToSpawn)
        {
            GameObject instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            instance.name = prefab.name;
            instance.SetActive(false);
            _arObjects[prefab.name] = instance;
        }
    }

#if AR_FOUNDATION_6_0_OR_NEWER
    private void OnImageTrackChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
            UpdateTrackedImage(trackedImage);

        foreach (var trackedImage in eventArgs.updated)
            UpdateTrackedImage(trackedImage);

        foreach (var trackedImage in eventArgs.removed)
            DeactivateTrackedImage(trackedImage);
    }
#else
    private void OnImageTrackChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
            UpdateTrackedImage(trackedImage);

        foreach (var trackedImage in eventArgs.updated)
            UpdateTrackedImage(trackedImage);

        foreach (var trackedImage in eventArgs.removed)
            DeactivateTrackedImage(trackedImage);
    }
#endif

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;
        if (!_arObjects.ContainsKey(imageName)) return;

        GameObject arObject = _arObjects[imageName];

        if (trackedImage.trackingState == TrackingState.None || trackedImage.trackingState == TrackingState.Limited)
        {
            arObject.SetActive(false);
            maincanvas.SetActive(false); 
            return;
        }
        arObject.SetActive(true);
        maincanvas.SetActive(true);
        arObject.transform.SetPositionAndRotation(trackedImage.transform.position + new Vector3(0, 0.05f, 0), trackedImage.transform.rotation);
        arObject.transform.localScale = Vector3.one * 0.05f;
    }

    private void DeactivateTrackedImage(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;
        if (_arObjects.ContainsKey(imageName))
        {
            _arObjects[imageName].SetActive(false);
            maincanvas.SetActive(false);
        }
    }
}
