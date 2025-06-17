using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
public class QuizManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private QuizState quizState;
    [SerializeField] private QuizGenerator quizGenerator;
    [SerializeField] private QuizAnswerValidator quizAnswerValidator;
    [SerializeField] private QuizScoreManager quizScore;
    [Header("Settings")]
    [SerializeField] private float delayQuizDisplayTime = 1.5f;
    private static IQuizDisplay quizDisplay;
    private QuizSO currentQuiz;
    public UnityEvent OnQuizEnd, OnQuizStart;
    public static IQuizDisplay QuizDisplay => quizDisplay;
    private void OnEnable()
    {
        QuizButton.OnButtonPressed += CheckQuizAnswer;
    }
    private void OnDisable()
    {
        QuizButton.OnButtonPressed -= CheckQuizAnswer;
    }
    private void Start()
    {
        SetUpQuizTopic(QuizDataHolder.selectedTopic,
                      QuizDataHolder.selectedDifficulty,
                      QuizDataHolder.selectedUserType); 
        
    }

    public void OverrideQuizDisplay(MonoBehaviour newDisplay)
    {
        quizDisplay = newDisplay as IQuizDisplay;
        if (quizDisplay == null)
        {
            Debug.LogError("Quiz display not found!");
            enabled = false;
        }

    }

    public void SetUpQuizTopic(QuizTopicSO topic, QuizEnum.QuizDifficulty difficulty, QuizEnum.QuizUserType userType)
    {
        quizState.SetupState(topic, difficulty, userType);
        quizGenerator.InitializeQuizList(topic, difficulty, userType);
        quizScore.SetMaxScore(quizGenerator.RemainingQuizCount);
        SetupNewQuiz();
        OnQuizStart?.Invoke();
    }

    public void SetupNewQuiz()
    {
        if (quizGenerator.RemainingQuizCount == 0)
        {
            quizDisplay.ClearQuiz();
            OnQuizEnd?.Invoke();
            return;
        }
        quizAnswerValidator.ResetAnswerTime();
        quizDisplay.ResetAllButtonColors();
        currentQuiz = quizGenerator.GetNextQuiz();
        quizState.SetCurrentQuiz(currentQuiz);

        // Setup display
        quizDisplay.SetQuizQuestion(currentQuiz.questionText);
        quizDisplay.SetQuizButtons(currentQuiz.choices.ToList());

        if (quizDisplay is QuizDisplay_Cripple && currentQuiz.videoPath != null)
        {
            quizDisplay.PlaySignLanguageVideo(currentQuiz.videoPath);
        }

        foreach (var btn in quizDisplay.GetQuizButtons())
        {
            btn.UnlockButton();
        }
    }


    public void CheckQuizAnswer(string value)
    {
        if (!quizState.CanAnswer()) return;
        quizState.SetAnswered();


        // create new QuizAnswerData everytime an answer is checked
        var answerData = new QuizAnswerData(
            value,
            quizState.GetCorrectAnswer()
        );

        quizAnswerValidator.ValidateAnswer(answerData , currentQuiz);
        Invoke(nameof(SetupNewQuiz), delayQuizDisplayTime);
        quizAnswerValidator.ResetAnswerTime();
    }
}