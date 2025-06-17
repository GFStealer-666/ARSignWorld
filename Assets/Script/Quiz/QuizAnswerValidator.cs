using UnityEngine;
using System;
using Quiz.Data;
public class QuizAnswerValidator : MonoBehaviour
{
    public event Action<QuizAnswerDetail> OnAnswerValidated;
    public event Action OnCorrectAnswer;
    private float answerStartTime;
    public void ValidateAnswer(QuizAnswerData data, QuizSO quiz)
    {
        if (data == null) return;
        bool isCorrect = data.CorrectAnswer == data.UserAnswer;
        float timeToAnswer = Time.time - answerStartTime;

        string isCorrectText = isCorrect ? "True" : "False";
        UpdateButtonStates(data);
        var answerDetail = new QuizAnswerDetail
        {
            questionText = quiz.questionText,
            selectedAnswer = data.UserAnswer,
            correctAnswer = data.CorrectAnswer,
            timeToAnswer = timeToAnswer.ToString("F2"),
            wasCorrect = isCorrectText
        };

        if (isCorrect) OnCorrectAnswer?.Invoke();

        OnAnswerValidated?.Invoke(answerDetail);
    }
    public void ResetAnswerTime()
    {
        answerStartTime = Time.time;
    }
    private void UpdateButtonStates(QuizAnswerData data)
    {
        if (data == null) return;

        foreach (QuizButton btn in QuizManager.QuizDisplay.GetQuizButtons())
        {
            if (btn.GetChoice() == data.CorrectAnswer)
                btn.CorrectColor();
            else if (btn.GetChoice() == data.UserAnswer)
                btn.WrongColor();

            btn.LockButton();
        }
    }
}

