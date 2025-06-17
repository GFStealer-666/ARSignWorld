using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using System.IO;

public class QuizDisplay_Cripple : MonoBehaviour, IQuizDisplay
{
    [SerializeField] private List<QuizButton> quizButtons;
    [SerializeField] private TextMeshProUGUI quizQuestion;
    [SerializeField] private VideoPlayer videoPlayer;

    public void SetQuizQuestion(string value) => quizQuestion.text = value;

    public void SetQuizButtons(List<string> values)
    {
        for (int i = 0; i < quizButtons.Count; i++)
            quizButtons[i].SetChoice(values[i]);
    }

    public void ResetAllButtonColors()
    {
        foreach (var button in quizButtons)
            button.ResetColor();
    }

    public void ClearQuiz()
    {
        foreach (var button in quizButtons)
            button.SetChoice("");
        quizQuestion.text = "";
        //videoPlayer.Stop();
    }

    public List<QuizButton> GetQuizButtons() => quizButtons;

    public void PlaySignLanguageVideo(string videoPath)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, videoPath);
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = fullPath;
        videoPlayer.Play();
    }
}
