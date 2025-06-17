using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Quiz.Data; // If QuizStat is in a namespace

public class GoogleSheetUploader : MonoBehaviour
{
    private string googleScriptUrl
     = "https://script.google.com/macros/s/AKfycby8a7Z4JtV6z23Vr-kbaxsPyLxZ4_AJ7Oxzkb6F_Z2zoVY0KMK4xMUuENaODklimTK1/exec";
    [SerializeField] private string apiKey = "Lalalal1sa";
    
    public void UploadQuizStat(QuizStat quizStat)
    {
        GoogleSheetData flatData = ToGoogleSheetData(quizStat);
        StartCoroutine(PostToGoogleSheet(flatData));
    }

    private IEnumerator PostToGoogleSheet(GoogleSheetData data)
    {
        string json = JsonUtility.ToJson(data);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(googleScriptUrl, UnityWebRequest.kHttpVerbPOST);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("X-API-KEY", apiKey);  // key ต้องตรงกับ Script

        yield return request.SendWebRequest();

        Debug.Log("RESPONSE: " + request.downloadHandler.text);

    }


    private GoogleSheetData ToGoogleSheetData(QuizStat stat)
    {
        var data = new GoogleSheetData
        {
            projectId = "ARWorld",
            apiKey = apiKey,
            uniqueId = stat.resultData.uniqueId,
            appversion = stat.resultData.appversion,    
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
            isCollectAdvancedData = stat.isCollectAnswerBreakdown
        };
        if (stat.isCollectAnswerBreakdown)
        {
            data.answerDetails = stat.answerDetails;
        }
        return data;
    }


    [System.Serializable]
    public class GoogleSheetData
    {
        public string projectId;
        public string apiKey;
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

    }


}
