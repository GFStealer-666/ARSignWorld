using UnityEngine;
using UnityEngine.UI;

public class ModelViewController : MonoBehaviour
{
    [SerializeField] private Button toggleButton;
    [SerializeField] private Image toggleIcon;
    [SerializeField] private Slider rotatorSlider, scaleSlider;
    [SerializeField] private Sprite showSprite, hideSprite;
    [SerializeField] private ModelTransformController modelTransformController;

    private Canvas modelCanvas;
    private bool isCanvasVisible;

    private void Awake()
    {
        if (toggleButton != null && toggleIcon != null)
        {
            toggleIcon.sprite = showSprite;
            toggleButton.interactable = false;

            rotatorSlider.gameObject.SetActive(false);
            scaleSlider.gameObject.SetActive(false);
        }
    }

    public void Setup(Canvas canvas, Transform pivotPoint)
    {
        modelCanvas = canvas;
        isCanvasVisible = false;
        modelCanvas.gameObject.SetActive(false);
        toggleIcon.sprite = showSprite;
        toggleButton.interactable = (modelCanvas != null);

        if (modelTransformController != null && pivotPoint != null)
            modelTransformController.SetTransform(pivotPoint);
    }

    public void ToggleCanvas()
    {
        if (modelCanvas == null) return;

        isCanvasVisible = !isCanvasVisible;
        modelCanvas.gameObject.SetActive(isCanvasVisible);
        toggleIcon.sprite = isCanvasVisible ? hideSprite : showSprite;
    }
    public void SetInteractable(bool interactable)
    {
        toggleButton.interactable = interactable;
        if (interactable && modelCanvas != null)
        {
            modelCanvas.gameObject.SetActive(isCanvasVisible);
        }

        if(rotatorSlider != null && scaleSlider != null)
        {
            rotatorSlider.gameObject.SetActive(interactable);
            scaleSlider.gameObject.SetActive(interactable);
        }
        else
        {
            modelCanvas.gameObject.SetActive(false);
            isCanvasVisible = false;
            toggleIcon.sprite = showSprite;
        }
    }
}