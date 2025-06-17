using UnityEngine;

public class QuizUICanvasSwitcher : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField] private GameObject normalCanvas;
    [SerializeField] private GameObject crippleCanvas;
    [Header("Display References")]
    [SerializeField] private MonoBehaviour normalDisplay;  
    [SerializeField] private MonoBehaviour crippleDisplay;  
    [SerializeField] private QuizManager quizManager;

    private void Awake()
    {
        ValidateReferences();
        SetupCanvas();
    }

    private void ValidateReferences()
    {
        if (normalCanvas == null || crippleCanvas == null)
        {
            Debug.LogError($"[{nameof(QuizUICanvasSwitcher)}] Canvas references missing!");
            enabled = false;
            return;
        }

        if (normalDisplay == null || crippleDisplay == null)
        {
            enabled = false;
            return;
        }

        if (quizManager == null)
        {
            quizManager = GetComponent<QuizManager>();
            if (quizManager == null)
            {
                enabled = false;
                return;
            }
        }
    }

    private void SetupCanvas()
    {
        bool isCripple = QuizDataHolder.selectedUserType == QuizEnum.QuizUserType.Cripple;
        
        // Switch canvases
        normalCanvas.SetActive(!isCripple);
        crippleCanvas.SetActive(isCripple);

        // Set appropriate display
        MonoBehaviour chosenDisplay = isCripple ? crippleDisplay : normalDisplay;
        quizManager.OverrideQuizDisplay(chosenDisplay);
    }
}
