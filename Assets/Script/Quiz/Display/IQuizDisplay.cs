using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public interface IQuizDisplay
{
    void SetQuizQuestion(string value);
    void SetQuizButtons(List<string> values);
    void ResetAllButtonColors();
    void ClearQuiz();
    List<QuizButton> GetQuizButtons();
    void PlaySignLanguageVideo(string videoPath);
}
