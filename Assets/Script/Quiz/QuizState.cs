using UnityEngine;

// Handle the state of the quiz, including the current topic, quiz, difficulty, user type, and whether the user has answered.
// This script is used to manage the quiz state and provide methods to interact with it.
public class QuizState : MonoBehaviour
{
    [SerializeField] private QuizTopicSO currentQuizTopic;
    [SerializeField] private QuizSO currentQuiz;
    
    [SerializeField] private string currentCorrectAnswer;
    [SerializeField]  QuizEnum.QuizDifficulty currentDifficulty;
    [SerializeField] private QuizEnum.QuizUserType currentUserType;

    private bool hasAnswered;

    public void SetupState(QuizTopicSO topic, QuizEnum.QuizDifficulty difficulty, QuizEnum.QuizUserType userType)
    {
        currentQuizTopic = topic;
        currentDifficulty = difficulty;
        currentUserType = userType;
        hasAnswered = false;
    }

    public void SetCurrentQuiz(QuizSO quiz)
    {
        currentQuiz = quiz;
        currentCorrectAnswer = quiz.choices[quiz.correctAnswerIndex];
        hasAnswered = false;
    }

    public bool CanAnswer() => !hasAnswered;
    public void SetAnswered() => hasAnswered = true;
    public string GetCorrectAnswer() => currentCorrectAnswer;
}