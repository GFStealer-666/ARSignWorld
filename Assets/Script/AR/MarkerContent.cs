using UnityEngine;
using UnityEngine.Video;


[CreateAssetMenu(fileName = "MarkerContent", menuName = "Marker/MarkerContent", order = 0)]
public class MarkerContent : ScriptableObject
{
    public string markerName; // e.g. "Marker-1"
    public string description;
    public string videoPath;

}
