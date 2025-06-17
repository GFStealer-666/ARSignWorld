using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class QuizDisplay_Normal : MonoBehaviour , IQuizDisplay
{
    [Header("UI References")]
    [SerializeField] private List<QuizButton> quizButtons = new();
    [SerializeField] private TextMeshProUGUI quizQuestion;

    /// <summary>
    /// Sets the text on each quiz choice button.
    /// </summary>
    public void SetQuizButtons(List<string> values)
    {
        for (int i = 0; i < quizButtons.Count; i++)
        {
            // Avoid index error if value list is shorter
            if (i < values.Count)
                quizButtons[i].SetChoice(values[i]);
            else
                quizButtons[i].SetChoice("");
        }
    }

    /// <summary>
    /// Sets the main quiz question text.
    /// </summary>
    public void SetQuizQuestion(string value)
    {
        quizQuestion.text = value;
    }

    /// <summary>
    /// Clears question and answer display.
    /// </summary>
    public void ClearQuiz()
    {
        foreach (var btn in quizButtons)
        {
            btn.SetChoice("");
        }

        quizQuestion.text = "";

    }

    /// <summary>
    /// Resets all answer button colors to default.
    /// </summary>
    public void ResetAllButtonColors()
    {
        foreach (var button in quizButtons)
        {
            button.ResetColor();
        }
    }

    /// <summary>
    /// Plays the sign-language video clip.
    /// </summary>
    public void PlaySignLanguageVideo(string videoPath)
    {
    }

    /// <summary>
    /// Returns the list of quiz answer buttons.
    /// </summary>
    public List<QuizButton> GetQuizButtons()
    {
        return quizButtons;
    }
}
