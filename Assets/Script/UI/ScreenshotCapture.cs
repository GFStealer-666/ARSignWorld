using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenshotCapture : MonoBehaviour
{
    public void CaptureScreenshot()
    {
        StartCoroutine(TakeScreenshot());
    }

    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame(); // wait for the frame to finish rendering

        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string filename = $"Screenshot_{timestamp}.png";

#if UNITY_ANDROID
        string path = Path.Combine(Application.persistentDataPath, filename);
#else
        string path = Path.Combine(Application.dataPath, filename);
#endif

        ScreenCapture.CaptureScreenshot(filename);
        Debug.Log($"Screenshot saved to: {path}");
    }
}
