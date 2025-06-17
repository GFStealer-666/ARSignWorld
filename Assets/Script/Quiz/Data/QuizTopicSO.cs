using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewQuizTopic", menuName = "Quiz/Topic", order = 0)]
public class QuizTopicSO : ScriptableObject
{
    public string topicName;
    public QuizEnum.QuizUserType quizUserType;
    public List<QuizSO> quizList;
}