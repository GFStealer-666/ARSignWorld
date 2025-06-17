using UnityEngine;
using TMPro;

public class DayCounter : MonoBehaviour
{
    public TextMeshPro textMeshPro;     // Assign your 3D TextMeshPro here
    public float interval = 3f;         // Time in seconds between each increment
    public string prefix1 = "Year: ";   // Optional text before the number
    public string prefix2 = "  Day: ";

    private float timer = 0f; // Timer to track the interval
    private int count = 0; // For days
    private int count2 = 0; // For years


     void Start()
    {
        if (textMeshPro != null)
            textMeshPro.text = prefix1 + count2.ToString() + prefix2 + count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
       if (textMeshPro == null) return;

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            count++;
            textMeshPro.text = prefix1 + count2.ToString() + prefix2 + count.ToString();
            timer = 0f;
        }

        if (count >= 365)
        {
            count = 0;
            count2++;
            textMeshPro.text = prefix1 + count2.ToString() + prefix2 + count.ToString();
        }
    }
}
