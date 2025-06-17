using UnityEngine;

public class LightRotation : MonoBehaviour
{
    public Transform target; // Object A

    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.position);
        }
    }
}
