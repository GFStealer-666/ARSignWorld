using UnityEngine;

public class Spin : MonoBehaviour
{
    public Vector3 rotationAxis = new Vector3(0, 1, 0); // Default: Y-axis
    public float rotationSpeed = 45f; // Degrees per second

    void Update()
    {
        transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime);
    }
}
