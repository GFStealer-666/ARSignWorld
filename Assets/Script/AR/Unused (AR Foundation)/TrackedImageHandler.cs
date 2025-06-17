// using UnityEngine;
// using UnityEngine.XR.ARFoundation;
// using UnityEngine.XR.ARSubsystems;
// using System.Collections.Generic;

// public class TrackedImageHandler : MonoBehaviour
// {
//     public ARTrackedImageManager trackedImageManager;
//     public List<GameObject> arPrefabs;

//     private Dictionary<string, GameObject> spawnedPrefabs = new();

//     void Awake()
//     {
//         foreach (var prefab in arPrefabs)
//         {
//             var go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
//             go.name = prefab.name;
//             go.SetActive(false);
//             spawnedPrefabs.Add(prefab.name, go);
//         }
//     }

//     void OnEnable() => trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
//     void OnDisable() => trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

//     void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
//     {
//         foreach (var trackedImage in eventArgs.added)
//             UpdateImage(trackedImage);

//         foreach (var trackedImage in eventArgs.updated)
//             UpdateImage(trackedImage);

//         foreach (var trackedImage in eventArgs.removed)
//         {
//             if (spawnedPrefabs.ContainsKey(trackedImage.referenceImage.name))
//                 spawnedPrefabs[trackedImage.referenceImage.name].SetActive(false);
//         }
//     }

//     void UpdateImage(ARTrackedImage trackedImage)
//     {
//         foreach (var go in spawnedPrefabs.Values)
//             go.SetActive(false);

//         var name = trackedImage.referenceImage.name;
//         var obj = spawnedPrefabs[name];
//         obj.SetActive(true);
//         obj.transform.position = trackedImage.transform.position;
//         obj.transform.rotation = trackedImage.transform.rotation;
//     }
// }
