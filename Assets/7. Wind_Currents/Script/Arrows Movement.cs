using UnityEngine;

public class ArrowsMovement : MonoBehaviour
{
    public Transform sphere;             // The sphere the object moves around
    public Vector3 startLocalPos;        // Local position from center of sphere (normalized)
    public Vector3 endLocalPos;

    public Vector3 startRotationEuler;   // Starting rotation (in degrees)
    public Vector3 endRotationEuler;     // Ending rotation (in degrees)

    public float radius = 5f;            // Distance from sphere center
    public float speed = 1f;             // Time to move from start to end

    private float timer = 0f;
    private bool goingForward = true;

    void Update()
    {
        if (sphere == null) return;

        timer += Time.deltaTime / speed;

        float t = Mathf.PingPong(timer, 1f); // Ping-pong between 0 and 1

        // Interpolate between start and end positions
        Vector3 localDir = Vector3.Slerp(startLocalPos.normalized, endLocalPos.normalized, t);
        transform.position = sphere.position + localDir * radius;

        // Interpolate rotation
        Quaternion startRot = Quaternion.Euler(startRotationEuler);
        Quaternion endRot = Quaternion.Euler(endRotationEuler);
        transform.rotation = Quaternion.Slerp(startRot, endRot, t);
    }

    // Optional: draw debug lines
    private void OnDrawGizmosSelected()
    {
        if (sphere != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(sphere.position + startLocalPos.normalized * radius, 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(sphere.position + endLocalPos.normalized * radius, 0.1f);
        }
    }
}
