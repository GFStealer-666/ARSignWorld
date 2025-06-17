using UnityEngine;

public class LightScaleController : MonoBehaviour
{
    [Header("Light References")]
    [SerializeField] private Light[] sceneLights;

    [Header("Settings")]
    [SerializeField] private bool autoFindLights = true;
    [SerializeField] private bool useSquareRootScaling = true;
    [SerializeField] private float intensityMultiplier = 1f;

    private Vector3 lastScale;
    private float initialScale;

    private void Start()
    {
        if (autoFindLights)
        {
            // Find all non-directional lights in children
            sceneLights = GetComponentsInChildren<Light>();
            sceneLights = System.Array.FindAll(sceneLights, light => light.type != LightType.Directional);
        }

        // Store initial values
        lastScale = transform.lossyScale;
        initialScale = lastScale.x;
        
        // Apply initial scaling
        UpdateLightScales();
    }

    private void Update()
    {
        // Check if scale changed
        if (lastScale != transform.lossyScale)
        {
            UpdateLightScales();
            lastScale = transform.lossyScale;
        }
    }

    private void UpdateLightScales()
    {
        if (sceneLights == null || sceneLights.Length == 0) return;

        float scaleFactor = transform.lossyScale.x / initialScale;

        foreach (Light light in sceneLights)
        {
            if (light != null)
            {
                // Scale light range directly with scale
                light.range *= scaleFactor;

                // Scale intensity with optional square root for more natural falloff
                float intensityScale = useSquareRootScaling ? 
                    Mathf.Sqrt(scaleFactor) : scaleFactor;
                light.intensity = light.intensity * intensityScale * intensityMultiplier;
            }
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateLightScales();
        }
    }
}