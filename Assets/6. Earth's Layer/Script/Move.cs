using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 2f;           // Speed of movement
    public float distance = 3f;        // Total distance from the start point
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
    }
}
