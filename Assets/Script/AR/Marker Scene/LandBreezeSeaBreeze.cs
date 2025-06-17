using UnityEngine;

public class LandBreezeSeaBreeze : MonoBehaviour
{
    [Header("References")]
    public Transform moon;
    public Transform sun;
    public Transform centerPoint;

    [Header("Moon Orbit Settings")]
    public float orbitRadius = 10f;
    public float orbitSpeed = 10f; // degrees/second
    public float moonRotationSpeed = 30f;

    [Header("Sun Rotation Settings")]
    public float sunRotationSpeedMultiplier = 1f;   
    private float currentAngle = 0f;

    [Header("Groups to Toggle")]
    public GameObject[] groupA;  // Enable at Position A, Disable at B
    public GameObject[] groupB;  // Enable at Position B, Disable at A

    [Header("Moon Trigger Directions")]
    public Vector3 positionADirection = Vector3.forward;
    public Vector3 positionBDirection = Vector3.back;
    public float angleThreshold = 10f;

    [Header("Scale Settings")]
    [SerializeField] private bool showDebug = true;
    private Vector3 lastScale;
    private float worldScale = 1f;

    private bool triggeredA = false;
    private bool triggeredB = false;
    
    private void Start()
    {
        lastScale = transform.lossyScale;
        InitializeScale();
    }

    private void InitializeScale()
    {
        worldScale = transform.lossyScale.x;
        orbitRadius *= worldScale;

        if (showDebug)
        {
            // Debug.Log($"Initialized with scale: {worldScale}");
            // Debug.Log($"Scaled orbit radius: {orbitRadius}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if scale changed
        if (lastScale != transform.lossyScale)
        {
            RecalculateScale();
            lastScale = transform.lossyScale;
        }

        UpdateCelestialBodies();
        CheckTriggers();
    }

    private void RecalculateScale()
    {
        float newScale = transform.lossyScale.x;
        float scaleFactor = newScale / worldScale;
        worldScale = newScale;
        orbitRadius *= scaleFactor;
    }

    private void UpdateCelestialBodies()
    {
        // Update angle
        currentAngle += orbitSpeed * Time.deltaTime;
        currentAngle %= 360f;

        // Calculate orbit positions in local space
        Vector3 localMoonOrbit = new Vector3(
            Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitRadius,
            Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitRadius,
            0f
        );

        Vector3 localSunOrbit = new Vector3(
            Mathf.Cos((currentAngle + 180f) * Mathf.Deg2Rad) * orbitRadius,
            Mathf.Sin((currentAngle + 180f) * Mathf.Deg2Rad) * orbitRadius,
            0f
        );

        // Transform to world space relative to center
        moon.position = centerPoint.TransformPoint(localMoonOrbit);
        sun.position = centerPoint.TransformPoint(localSunOrbit);

        // Rotate moon
        moon.Rotate(Vector3.forward * moonRotationSpeed * Time.deltaTime, Space.Self);
    }

    private void CheckTriggers()
    {
        if (moon == null || centerPoint == null) return;

        // Convert positions to local space for consistent angle checks
        Vector3 localMoonDir = centerPoint.InverseTransformPoint(moon.position).normalized;
        Vector3 localDirA = centerPoint.InverseTransformDirection(positionADirection.normalized);
        Vector3 localDirB = centerPoint.InverseTransformDirection(positionBDirection.normalized);

        float angleToA = Vector3.Angle(localMoonDir, localDirA);
        float angleToB = Vector3.Angle(localMoonDir, localDirB);

        if (showDebug)
        {
            Debug.DrawRay(centerPoint.position, positionADirection.normalized * orbitRadius, Color.red);
            Debug.DrawRay(centerPoint.position, positionBDirection.normalized * orbitRadius, Color.blue);
        }

        // Check triggers
        if (angleToA <= angleThreshold && !triggeredA)
        {
            SetGroupState(groupA, true);
            SetGroupState(groupB, false);
            triggeredA = true;
            triggeredB = false;
        }

        if (angleToB <= angleThreshold && !triggeredB)
        {
            SetGroupState(groupA, false);
            SetGroupState(groupB, true);
            triggeredB = true;
            triggeredA = false;
        }
    }

    void SetGroupState(GameObject[] group, bool state)
    {
        foreach (GameObject obj in group)
        {
            if (obj != null)
                obj.SetActive(state);
        }
    }

        
}
