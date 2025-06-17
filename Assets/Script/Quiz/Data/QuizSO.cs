using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "NewQuizQuestion", menuName = "Quiz/Question", order = 1)]
public class QuizSO : ScriptableObject
{
    public string questionText; 
    public string[] choices = new string[4]; 
    public int correctAnswerIndex; 
    public string videoPath;
    public void ShuffleAnswers()
    {
        //(O2n)
        string correctAnswer = choices[correctAnswerIndex]; // store correct answer as string 

        // using algortim to swtich up index of correct answer
        for (int i = 0; i < choices.Length; i++)
        {
            int j = Random.Range(0, choices.Length);
            string temp = choices[i];
            choices[i] = choices[j];
            choices[j] = temp;
        }

        for (int i = 0; i < choices.Length; i++)
        {
            if (choices[i] == correctAnswer)
            {
                correctAnswerIndex = i;
                break;
            }
        }
    }
}
