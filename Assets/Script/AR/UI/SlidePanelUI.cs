using UnityEngine;
using UnityEngine.UI;

public class SlidePanelUI : MonoBehaviour
{
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private float hiddenX = 600f; // how far offscreen
    [SerializeField] private float shownX = 0f;     // original position
    [SerializeField] private float slideSpeed = 500f;
    [SerializeField] private Button toggleButton;

    private bool isHidden = false;

    private void Start()
    {
        toggleButton.onClick.AddListener(TogglePanel);
    }

    public void TogglePanel()
    {
        isHidden = !isHidden;
        StopAllCoroutines();
        StartCoroutine(SlideToPosition(isHidden ? hiddenX : shownX));
    }

    private System.Collections.IEnumerator SlideToPosition(float targetX)
    {
        Vector2 currentPos = panelTransform.anchoredPosition;
        Vector2 targetPos = new Vector2(targetX, currentPos.y);

        while (Vector2.Distance(panelTransform.anchoredPosition, targetPos) > 0.1f)
        {
            panelTransform.anchoredPosition = Vector2.MoveTowards(
                panelTransform.anchoredPosition,
                targetPos,
                slideSpeed * Time.deltaTime
            );
            yield return null;
        }

        panelTransform.anchoredPosition = targetPos; // snap exactly
    }
}
