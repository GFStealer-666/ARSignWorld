using System.Collections.Generic;


// Handle quiz answer data, including question text, user answer, correct answer, and buttons.
// This class is used to store the data for each quiz question and the user's response.
public class QuizAnswerData
{
    public string UserAnswer { get; private set; }
    public string CorrectAnswer { get; private set; }

    public QuizAnswerData( string userAnswer, string correctAnswer)
    {
        UserAnswer = userAnswer;
        CorrectAnswer = correctAnswer;
    }
}