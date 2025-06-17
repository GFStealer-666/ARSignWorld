using UnityEngine;

public class TidalAndLightController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform moon;
    [SerializeField] private Transform sun;
    [SerializeField] private Transform center;
    [SerializeField] private Transform water;
    [SerializeField] private Light[] streetLights;

    [Header("Trigger Points")]
    [SerializeField] private Transform nightTrigger;  // Position where night/high tide occurs
    [SerializeField] private Transform dayTrigger;    // Position where day/low tide occurs
    [Header("Water Level Points")]
    [SerializeField] private Transform highTidePoint; 
    [SerializeField] private Transform lowTidePoint;

    [Header("Settings")]
    [SerializeField] private float triggerDistance = 1f;
    [SerializeField] private float orbitRadius = 10f;
    [SerializeField] private float orbitSpeed = 10f;
    [SerializeField] private float moonRotationSpeed = 30f;
    [SerializeField] private float sunRotationSpeedMultiplier = 1f;
    [SerializeField] private float tideHeight = 2f;
    [SerializeField] private float tideSpeed = 1f;

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;

    private float worldScale = 1f;
    private float currentAngle = 0f;
    private Vector3 initialWaterPos;
    private bool rising = false;
    private bool lowering = false;

    private void Start()
    {
        InitializeScale();
        InitializePositions();
        ScaleLights();
    }

    private void Update()
    {
        UpdateCelestialBodies();
        CheckDayNightTriggers();
    }

    private void InitializeScale()
    {
        worldScale = transform.lossyScale.x;

        // Only scale these values
        orbitRadius *= worldScale;
        tideHeight *= worldScale;
        tideSpeed *= worldScale;

        // Don't scale triggerDistance as we'll use local space comparisons
        if (showDebug)
        {
            // Debug.Log($"Initialized with scale: {worldScale}");
            // Debug.Log($"Scaled orbit radius: {orbitRadius}");
        }
    }

    private void InitializePositions()
    {
        if (water != null && lowTidePoint != null)
            initialWaterPos = lowTidePoint.position; // Use lowTidePoint as initial position
    }

    private void ScaleLights()
    {
        if (streetLights == null) return;

        foreach (Light light in streetLights)
        {
            if (light != null)
            {
                light.range *= worldScale;
                light.intensity *= Mathf.Sqrt(worldScale); // Square root for more natural scaling
            }
        }
    }

    private void UpdateCelestialBodies()
    {
        // Update angle
        currentAngle += orbitSpeed * Time.deltaTime;
        currentAngle %= 360f;

        // Use local space for orbit calculation
        Vector3 localOrbitPosition = new Vector3(
            Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitRadius,
            Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitRadius,
            0f
        );

        // Transform to world space relative to center
        moon.position = center.TransformPoint(localOrbitPosition);
        sun.position = center.TransformPoint(-localOrbitPosition); // Opposite side

        // Update rotations
        moon.Rotate(Vector3.forward * moonRotationSpeed * Time.deltaTime, Space.Self);
        sun.Rotate(Vector3.forward * moonRotationSpeed * sunRotationSpeedMultiplier * Time.deltaTime, Space.Self);
    }

    private void CheckDayNightTriggers()
    {
        // Convert all positions to center's local space
        Vector3 moonLocalPos = center.InverseTransformPoint(moon.position);
        Vector3 nightLocalPos = center.InverseTransformPoint(nightTrigger.position);
        Vector3 dayLocalPos = center.InverseTransformPoint(dayTrigger.position);

        // Calculate distances in local space
        float distanceToNight = Vector3.Distance(moonLocalPos, nightLocalPos);
        float distanceToDay = Vector3.Distance(moonLocalPos, dayLocalPos);
        
        // Scale trigger distance with orbit radius for consistent detection
        float scaledTriggerDistance = triggerDistance * (orbitRadius / 10f); // 10f is the default orbit radius

        if (showDebug)
        {
            // Debug.Log($"Local distance to night: {distanceToNight:F2}");
            // Debug.Log($"Local distance to day: {distanceToDay:F2}");
            // Debug.Log($"Scaled trigger distance: {scaledTriggerDistance:F2}");
            Debug.DrawLine(moon.position, nightTrigger.position, Color.red);
            Debug.DrawLine(moon.position, dayTrigger.position, Color.blue);
        }

        // Check triggers using local space distances
        if (distanceToNight < scaledTriggerDistance && !rising)
        {
            rising = true;
            lowering = false;
            ToggleStreetLights(true);
        }
        else if (distanceToDay < scaledTriggerDistance && !lowering)
        {
            rising = false;
            lowering = true;
            ToggleStreetLights(false);
        }

        UpdateWaterLevel();
    }

    private void UpdateWaterLevel()
    {
        if (water == null || highTidePoint == null || lowTidePoint == null) return;

        float targetY;
        if (rising)
        {
            //Debug.Log("Rising water level");
            targetY = highTidePoint.position.y;
        }
        else if (lowering)
        {
            //  Debug.Log("Lowering water level");
            targetY = lowTidePoint.position.y;
        }
        else
        {
            return; // No movement needed
        }

        Vector3 target = new Vector3(water.position.x, targetY, water.position.z);
        water.position = Vector3.MoveTowards(water.position, target, tideSpeed * Time.deltaTime);

        // Check if we reached the target
        if (Mathf.Abs(water.position.y - targetY) < 0.01f)
        {
            rising = false;
            lowering = false;
        }
    }

    private void ToggleStreetLights(bool turnOn)
    {
        foreach (Light light in streetLights)
        {
            if (light != null)
                light.enabled = turnOn;
        }
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        InitializeScale();
        InitializePositions();
        ScaleLights();
    }
}
