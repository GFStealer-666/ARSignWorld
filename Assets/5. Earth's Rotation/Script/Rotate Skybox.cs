using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    public float rotationSpeed = 1f;  // Rotation speed (degrees per second)

    void Update()
    {
        // Rotate the skybox
        float rotation = Time.time * rotationSpeed;
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }
}
