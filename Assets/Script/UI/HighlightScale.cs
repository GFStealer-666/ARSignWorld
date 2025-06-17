using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float scaleMultiplier = 1.1f; // How much bigger when highlighted

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * scaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
