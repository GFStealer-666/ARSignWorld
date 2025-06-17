using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class QuizButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI choice;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Button button;
    [SerializeField] private Sprite defaultButton, correctButton, wrongButton;
    public static event Action<string> OnButtonPressed;
    private bool IsLocked = false;
    public void SetChoice(string value)
    {
        choice.text = value;
    }
    public void OnButtonClick()
    {
        if (IsLocked) return;
        OnButtonPressed?.Invoke(choice.text);
    }
    public string GetChoice() => choice.text;
    public void CorrectColor()
    {
        if (backgroundImage != null)
            backgroundImage.sprite = correctButton;
    }
    public void WrongColor()
    {
        if (backgroundImage != null)
            backgroundImage.sprite = wrongButton;
    }
    public void ResetColor()
    {
        if (backgroundImage != null)
            backgroundImage.sprite = defaultButton;
    }
    public void LockButton() => IsLocked = true;
    public void UnlockButton() => IsLocked = false;
    
}
