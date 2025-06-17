using UnityEngine;
using UnityEngine.UI;

public class ModelTransformController : MonoBehaviour
{
    [Header("Target Transform")]
    [SerializeField] private Transform modelToTransform;

    [Header("UI Controls")]
    [SerializeField] private Slider rotationSlider;
    [SerializeField] private Slider scaleSlider;

    [Header("Transform Settings")]

    [SerializeField] private float rotationMin = 0f;
    [SerializeField] private float rotationMax = 360f;
    [SerializeField] private float scaleMin = 0.5f;
    [SerializeField] private float scaleMax = 2f;
    private void Start()
    {
        SetupRotationSlider();
        SetupScaleSlider();
    }

    private void SetupRotationSlider()
    {
        rotationSlider.minValue = rotationMin;
        rotationSlider.maxValue = rotationMax;
        rotationSlider.wholeNumbers = true;
        rotationSlider.onValueChanged.AddListener(RotateModel);
    }

    private void SetupScaleSlider()
    {
        scaleSlider.minValue = scaleMin;
        scaleSlider.maxValue = scaleMax;
        scaleSlider.value = 1f;
        scaleSlider.onValueChanged.AddListener(ScaleModel);
    }

    private void RotateModel(float angle)
    {
        if (modelToTransform != null)
        {
            Vector3 currentRotation = modelToTransform.localEulerAngles;
            modelToTransform.localEulerAngles = new Vector3(currentRotation.x, angle, currentRotation.z);
        }
    }

    private void ScaleModel(float scale)
    {
        if (modelToTransform != null)
        {
            modelToTransform.localScale = Vector3.one * scale;
        }
    }

    public void SetTransform(Transform newTransform)
    {
        modelToTransform = newTransform;
        if (modelToTransform != null)
        {
            RotateModel(rotationSlider.value);
            ScaleModel(scaleSlider.value);
        }
    }
}