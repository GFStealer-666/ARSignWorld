using System;
using System.Collections.Generic;

namespace Quiz.Data
{
    [Serializable]
    public class QuizStat
    {
        // Basic result data
        public QuizResultData resultData;

        // Detailed answer tracking
        public List<QuizAnswerDetail> answerDetails;

        // Topic statistics
        public TopicStats topicStats;
        public bool isCollectAnswerBreakdown = true; 
        public QuizStat()
        {
            resultData = new QuizResultData();
            answerDetails = new List<QuizAnswerDetail>();
            topicStats = new TopicStats();
        }
    }

    [Serializable]
    public class TopicStats
    {
        public float averageTimePerQuestion;
        public List<QuizAnswerDetail> questionHistory;
        public TopicStats()
        {
            questionHistory = new List<QuizAnswerDetail>();
        }
    }
    
    [Serializable]
    public class QuizAnswerDetail
    {
        public string questionText;
        public string selectedAnswer;
        public string correctAnswer;
        public string timeToAnswer;
        public string wasCorrect;
    }

    [Serializable]
    public class QuizResultData
    {
        public string uniqueId;
        public string appversion;
        public string userName;
        public string gender;
        public int age;
        public string selectedTopic; //
        public string difficulty; //
        public string userType; //
        public string correctAccuracy; //
        public int totalScore; //
        public int maxScore;
        public int totalCorrect;
        public int totalWrong;
        public int stars;
        public string timeTaken;
    }
}