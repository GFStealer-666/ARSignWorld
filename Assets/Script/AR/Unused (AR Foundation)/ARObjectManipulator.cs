using UnityEngine;

public class ARObjectManipulator : MonoBehaviour
{
    [Header("Scale Settings")]
    public float minScale = 0.5f;
    public float maxScale = 2f;

    private float initialDistance;
    private Vector3 initialScale;
    private Quaternion initialRotation;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 delta = new Vector3(touch.deltaPosition.x, 0, touch.deltaPosition.y) * 0.001f;
                transform.position += delta;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            if (t1.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(t0.position, t1.position);
                initialScale = transform.localScale;
                initialRotation = transform.rotation;
            }
            else
            {
                float currentDistance = Vector2.Distance(t0.position, t1.position);
                if (Mathf.Approximately(initialDistance, 0)) return;

                float scaleFactor = currentDistance / initialDistance;
                Vector3 newScale = initialScale * scaleFactor;

                // Clamp scale
                newScale = Vector3.Max(newScale, Vector3.one * minScale);
                newScale = Vector3.Min(newScale, Vector3.one * maxScale);
                transform.localScale = newScale;

                // Rotation
                Vector2 prevDir = (t0.position - t0.deltaPosition) - (t1.position - t1.deltaPosition);
                Vector2 currDir = t0.position - t1.position;

                float angle = Vector2.SignedAngle(prevDir, currDir);
                transform.rotation = initialRotation * Quaternion.Euler(0, -angle, 0);
            }
        }
    }
}
