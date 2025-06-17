using UnityEngine;

public class Rotation : MonoBehaviour
{
     [Header("Rotation Around Object B")]
    public Transform targetObjectB;
    public float orbitSpeed = 20f;
    public float orbitRadius = 5f;

    [Header("Self Rotation")]
    public float selfRotationSpeed = 50f;
    public Vector3 selfRotationAngles = new Vector3(0, 1, 0);

    private float currentAngle = 0f;

    void Update()
    {
        if (targetObjectB == null) return;

        // Orbit Angle
        currentAngle += orbitSpeed * Time.deltaTime;
        float rad = currentAngle * Mathf.Deg2Rad;

        // Position around Object B
        Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * orbitRadius;
        transform.position = targetObjectB.position + offset;

        // Self-rotate Object A
        transform.Rotate(selfRotationAngles.normalized * selfRotationSpeed * Time.deltaTime);
    }
}
