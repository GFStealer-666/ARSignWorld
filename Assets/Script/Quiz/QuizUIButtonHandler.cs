using UnityEngine;

public class QuizUIButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject cripplePanel, normalPanel, inputDataPanel;
    public void DifficultyButtonClicked()
    {
        cripplePanel.SetActive(false);
        normalPanel.SetActive(false);
        inputDataPanel.SetActive(true);
    }
}
