using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SignPanelController : MonoBehaviour
{
    [SerializeField] private RectTransform infoPanel;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private float startX = 400f;
    [SerializeField] private float hiddenY = -360f;
    [SerializeField] private float shownY = 380f;
    
    [SerializeField]
    private Coroutine slideCoroutine;

    private void OnDisable()
    {
        // Stop any running coroutines when disabled
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
            slideCoroutine = null;
        }
    }
    private void Start()
    {
        infoPanel.anchoredPosition = new Vector2(startX, hiddenY);
        infoPanel.gameObject.SetActive(true);
    }

    public void ShowPanel() => AnimatePanel(shownY);
    public void HidePanel() => AnimatePanel(hiddenY);

    private void AnimatePanel(float targetY)
    {
        if (slideCoroutine != null)
            StopCoroutine(slideCoroutine);
        
        slideCoroutine = StartCoroutine(SlidePanel(targetY));
    }

    private IEnumerator SlidePanel(float targetY)
    {
        float elapsed = 0f;
        Vector2 start = infoPanel.anchoredPosition;
        Vector2 end = new Vector2(start.x, targetY);

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);
            infoPanel.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        infoPanel.anchoredPosition = end;
    }
}