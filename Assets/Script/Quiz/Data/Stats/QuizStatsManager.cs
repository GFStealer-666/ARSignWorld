using UnityEngine;
using System.Collections;
using System.Linq;
using Quiz.Data;
using System.Diagnostics;

public class QuizStatsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuizManager quizManager;
    [SerializeField] private QuizScoreManager scoreManager;
    [SerializeField] private QuizAnswerValidator quizAnswerValidator;
    [Header("API Uploader")]
    [SerializeField] private GoogleSheetUploader googleSheetUploader;
    [SerializeField] private GoAPIUploader goAPIUploader;
    [Header("Optional Settings")]
    [SerializeField] private bool CollectData = true;
    [SerializeField] private bool CollectDataInThai = true;
    [SerializeField] private bool CollectAnswerBreakdown = true;
    [Header("Debug")]
    [SerializeField] private QuizStat quizStats;
    private float quizStartTime;

    private void OnEnable()
    {
        if (quizManager != null)
        {
            quizManager.OnQuizStart.AddListener(HandleQuizStart);
            quizManager.OnQuizEnd.AddListener(HandleQuizEnd);
        }

        if (quizAnswerValidator != null)
        {
            quizAnswerValidator.OnAnswerValidated += HandleAnswerValidated;
        }
    }

    private void OnDisable()
    {
        if (quizManager != null)
        {
            quizManager.OnQuizStart.RemoveListener(HandleQuizStart);
            quizManager.OnQuizEnd.RemoveListener(HandleQuizEnd);
        }
        if (quizAnswerValidator != null)
        {
            quizAnswerValidator.OnAnswerValidated -= HandleAnswerValidated;
        }
    }
    // Create Stat tracking
    private void HandleQuizStart()
    {
        quizStats = null; // Reset stats for new quiz
        if (!CollectData) return;
        quizStartTime = Time.time;
        quizStats = new QuizStat();
        quizStats.isCollectAnswerBreakdown = CollectAnswerBreakdown;
        quizStats.resultData = new QuizResultData
        {
            uniqueId = SystemInfo.deviceUniqueIdentifier,
            appversion = Application.version.ToString(),
            userName = QuizDataHolder.Username,
            gender = QuizDataHolder.Gender.ToString(),
            age = QuizDataHolder.Age,
            selectedTopic = QuizDataHolder.selectedTopic.topicName,
            difficulty = QuizDataHolder.selectedDifficulty.ToString(),
            userType = QuizDataHolder.selectedUserType.ToString()
        };
        
    }

    private void HandleQuizEnd()
    {
        if (!CollectData) return;
        if (quizStats == null) return;

        // Calculate final data
        quizStats.resultData.totalScore = scoreManager.Score;
        quizStats.resultData.maxScore = scoreManager.MaxScore;
        quizStats.resultData.correctAccuracy = CalculateAccuracy();
        quizStats.resultData.timeTaken = CalculateTotalTime();

        if (CollectDataInThai)
        {
            TranslateDataToThai(quizStats.resultData);
        }
        // Send single QuizData object to database
        StartCoroutine(SendToDatabase(quizStats));
        
        string json = JsonUtility.ToJson(quizStats);
        UnityEngine.Debug.Log("Sending JSON: " + json);
    }
    private void HandleAnswerValidated(QuizAnswerDetail detail)
    {
        if (!CollectData) return;
        quizStats.answerDetails.Add(detail);
        quizStats.topicStats.questionHistory.Add(detail);
        
        UpdateStats();
    }

    private void UpdateStats()
    {
        quizStats.resultData.totalCorrect = quizStats.answerDetails.Count(x => x.wasCorrect == "True");
        quizStats.resultData.totalWrong = quizStats.answerDetails.Count(x => x.wasCorrect != "True");
        quizStats.resultData.stars = scoreManager.Stars;
        
        float totalTime = quizStats.answerDetails.Sum(x => float.TryParse(x.timeToAnswer, out float t) ? t : 0f);
        quizStats.topicStats.averageTimePerQuestion = quizStats.answerDetails.Count > 0
            ? Mathf.Round((totalTime / quizStats.answerDetails.Count) * 100f) / 100f
            : 0f;
    }
    private string CalculateAccuracy()
    {
        if (scoreManager == null || scoreManager.MaxScore == 0)
            return "0%";

        float accuracy = (float)scoreManager.Score / scoreManager.MaxScore * 100f;
        // Floor to 2 decimal places
        float floored = Mathf.Floor(accuracy * 100f) / 100f;
        return $"{floored:F2}%";
    }

    private string CalculateTotalTime()
    {
        float seconds = Time.time - quizStartTime;
        // Ceil to 2 decimal places
        if (seconds >= 60f)
        {
            int minutes = (int)(seconds / 60f);
            int remainingSeconds = (int)(seconds % 60f);
            return $"{minutes} นาที {remainingSeconds} วินาที";
        }
        else
        {
            return $"{seconds:F2} วินาที";
        }
            
    }

    private void TranslateDataToThai(QuizResultData quizResultData)
    {
        switch (QuizDataHolder.selectedDifficulty)
        {
            case QuizEnum.QuizDifficulty.Easy:
                quizResultData.difficulty = "ง่าย";
                break;
            case QuizEnum.QuizDifficulty.Hard:
                quizResultData.difficulty = "ยาก";
                break;
        }

        switch (QuizDataHolder.selectedUserType)
        {
            case QuizEnum.QuizUserType.Normal:
                quizResultData.userType = "ผู้ใช้ทั่วไป";
                break;
            case QuizEnum.QuizUserType.Cripple:
                quizResultData.userType = "ผู้บกพร่องทางการได้ยิน";
                break;
        }

        switch (QuizDataHolder.Gender)
        {
            case QuizEnum.QuizUserGender.Male:
                quizResultData.gender = "เพศชาย";
                break;
            case QuizEnum.QuizUserGender.Female:
                quizResultData.gender = "เพศหญิง";
                break;
            case QuizEnum.QuizUserGender.LGBTQ:
                quizResultData.gender = "เพศทางเลือก";
                break;
            default:
                quizResultData.gender = "ไม่ระบุ";
                break;
        }

        if (float.TryParse(quizResultData.timeTaken, out float seconds))
        {
            if (seconds >= 60f)
            {
                int minutes = (int)(seconds / 60f);
                int remainingSeconds = (int)(seconds % 60f);
                quizResultData.timeTaken =
                $"{minutes} นาที {remainingSeconds} วินาที";
            }
            else
            {
                quizResultData.timeTaken = $"{seconds:F2} วินาที";
            }
        }
        quizResultData.appversion = $"เวอร์ชั่น {quizResultData.appversion}";

        foreach (QuizAnswerDetail quizAnswerDetail in quizStats.answerDetails)
        {
            quizAnswerDetail.wasCorrect = quizAnswerDetail.wasCorrect == "True" ? "ถูกต้อง" : "ผิด";
            quizAnswerDetail.timeToAnswer = $"{quizAnswerDetail.timeToAnswer} วินาที";
        }
    }
    private IEnumerator SendToDatabase(QuizStat data)
    {
        googleSheetUploader.UploadQuizStat(data);
        goAPIUploader.Upload(data);
        yield return null;
    }
}