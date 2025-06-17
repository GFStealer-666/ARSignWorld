using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using Quiz.Data;

public class GoAPIUploader : MonoBehaviour
{
    private string apiUrl = "https://arsignworlddatabase-arsign.up.railway.app/submit-quiz";

    public void Upload(QuizStat data)
    {
        StartCoroutine(PostData(ConvertToGoData(data)));
    }

    private IEnumerator PostData(GoLangPlayerQuizData data)
    {
        string json = JsonUtility.ToJson(data);
        Debug.Log("Sending JSON: " + json);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Upload Success: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Upload Failed: " + request.error + "\n" + request.downloadHandler.text);
        }
    }

    public static GoLangPlayerQuizData ConvertToGoData(QuizStat stat)
    {
        var data = new GoLangPlayerQuizData
        {
            appversion = stat.resultData.appversion,
            uniqueId = stat.resultData.uniqueId,
            userName = stat.resultData.userName,
            gender = stat.resultData.gender,
            age = stat.resultData.age,
            topic = stat.resultData.selectedTopic,
            difficulty = stat.resultData.difficulty,
            userType = stat.resultData.userType,
            score = stat.resultData.totalScore,
            maxScore = stat.resultData.maxScore,
            accuracy = stat.resultData.correctAccuracy,
            correct = stat.resultData.totalCorrect,
            wrong = stat.resultData.totalWrong,
            stars = stat.resultData.stars,
            timeTaken = stat.resultData.timeTaken,
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            isCollectAdvancedData = stat.isCollectAnswerBreakdown,
            answerDetails = new List<QuizAnswerDetail>()
        };

        foreach (var detail in stat.answerDetails)
        {
            if (float.TryParse(detail.timeToAnswer.Replace(" วินาที", "").Trim(), out float parsedTime))
            {
                data.answerDetails.Add(new QuizAnswerDetail
                {
                    questionText = detail.questionText,
                    selectedAnswer = detail.selectedAnswer,
                    correctAnswer = detail.correctAnswer,
                    timeToAnswer = parsedTime.ToString("0.##"), // Send as number-like string
                    wasCorrect = detail.wasCorrect == "ถูกต้อง" ? "true" : "false"
                });
            }
        }

        return data;
    }
}

[System.Serializable]
public class GoLangPlayerQuizData
{
    public string appversion;
    public string uniqueId;
    public string userName;
    public string gender;
    public int age;
    public string topic;
    public string difficulty;
    public string userType;
    public int score;
    public int maxScore;
    public string accuracy;
    public int correct;
    public int wrong;
    public int stars;
    public string timeTaken;
    public string timestamp;
    public bool isCollectAdvancedData;
    public List<QuizAnswerDetail> answerDetails;

    public float TimeTakenSeconds
    {
        get
        {
            return ParseTimeToSeconds(timeTaken);
        }
    }

    private float ParseTimeToSeconds(string time)
    {
        float seconds = 0;
        if (string.IsNullOrEmpty(time)) return 0;

        if (time.Contains("นาที"))
        {
            var parts = time.Split(new[] { "นาที" }, StringSplitOptions.None);
            if (float.TryParse(parts[0].Trim(), out float mins))
                seconds += mins * 60;
            if (parts.Length > 1 && parts[1].Contains("วินาที"))
            {
                var secStr = parts[1].Replace("วินาที", "").Trim();
                if (float.TryParse(secStr, out float secs))
                    seconds += secs;
            }
        }
        else if (time.Contains("วินาที"))
        {
            var secStr = time.Replace("วินาที", "").Trim();
            if (float.TryParse(secStr, out float secs))
                seconds += secs;
        }
        return seconds;
    }

}
