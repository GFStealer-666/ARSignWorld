using UnityEngine;
using TMPro;
using System;
public class QuizScoreManager : MonoBehaviour
{
    [SerializeField] private QuizAnswerValidator quizAnswerValidator;
    [SerializeField] private int score, maxScore , stars;
    public static event Action<int> OnScoreChanged, OnMaxScoreChanged;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            OnScoreChanged?.Invoke(score);
        }
    }
    public int MaxScore
    {
        get => maxScore;
        set
        {
            maxScore = value;
            OnMaxScoreChanged?.Invoke(maxScore);
        }
    }
    public int Stars => CalculateStars();
    private void OnEnable()
    {
        quizAnswerValidator.OnCorrectAnswer += IncrementScore;
    }
    private void OnDisable()
    {
        quizAnswerValidator.OnCorrectAnswer -= IncrementScore;
    }
    public int CalculateStars()
    {
        stars = 0;

        if (maxScore == 0) return 0;

        float percentage = (float)score / maxScore;

        if (percentage >= 1f)
            stars = 3;
        else if (percentage >= 0.5f)
            stars = 2;
        else if (percentage > 0f)
            stars = 1;
        else
            stars = 0;

        return stars;
    }
    private void IncrementScore()
    {
        Score = score + 1;
    }
    public void SetMaxScore(int value)
    {
        MaxScore = value;
    }
    public void ResetScore()
    {
        Score = 0;
    }
}
