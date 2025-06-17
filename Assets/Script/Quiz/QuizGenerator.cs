using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuizGenerator : MonoBehaviour
{
    // Generates a list of quizzes based on the selected topic, difficulty, and user type.
    private const int CrippleEasyCount = 3;
    private const int CrippleHardCount = 5;
    private const int NormalEasyCount = 5;
    private const int NormalHardCount = 10;
    [SerializeField] private List<QuizSO> currentQuizList = new List<QuizSO>();
    public void InitializeQuizList(QuizTopicSO topic, QuizEnum.QuizDifficulty difficulty, QuizEnum.QuizUserType userType)
    {
        int quizCount = GetQuizCount(difficulty, userType);
        currentQuizList = topic.quizList
            .OrderBy(x => UnityEngine.Random.value)
            .Take(quizCount)
            .ToList();
    }

    public QuizSO GetNextQuiz()
    {
        if (currentQuizList.Count == 0) return null;

        int index = UnityEngine.Random.Range(0, currentQuizList.Count);
        var quiz = currentQuizList[index];
        currentQuizList.RemoveAt(index);
        return quiz;
    }

    public int RemainingQuizCount => currentQuizList.Count;

    private int GetQuizCount(QuizEnum.QuizDifficulty difficulty, QuizEnum.QuizUserType userType) =>
        userType switch
        {
            QuizEnum.QuizUserType.Cripple => difficulty == QuizEnum.QuizDifficulty.Easy ? CrippleEasyCount : CrippleHardCount,
            _ => difficulty == QuizEnum.QuizDifficulty.Easy ? NormalEasyCount : NormalHardCount
        };
}
















    // public List<QuizSO> GenerateQuizList(QuizTopicSO topic, QuizEnum.QuizDifficulty difficulty, QuizEnum.QuizUserType userType)
    // {
    //     if (topic == null || topic.quizList.Count == 0) return new List<QuizSO>();

    //     int quizCount = GetQuizCount(difficulty, userType);
    //     List<QuizSO> allQuizzes = new List<QuizSO>(topic.quizList);
    //     List<QuizSO> selectedQuizzes = new List<QuizSO>();

    //     while (selectedQuizzes.Count < quizCount && allQuizzes.Count > 0)
    //     {
    //         int index = UnityEngine.Random.Range(0, allQuizzes.Count);
    //         selectedQuizzes.Add(allQuizzes[index]);
    //         allQuizzes.RemoveAt(index);
    //     }

    //     return selectedQuizzes;
    // }

    