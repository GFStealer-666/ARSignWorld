using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuizScoreUI : MonoBehaviour
{
    // Handles the display of the quiz score UI, including the score, max score, and star ratings.
    [SerializeField] private TextMeshProUGUI scoreUi , scoreUi_max;
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private List<GameObject> starSprites; 
    [SerializeField] private QuizManager quizManager;
    [SerializeField] private QuizScoreManager quizScoreManager;
    void OnEnable()
    {
        QuizScoreManager.OnMaxScoreChanged += UpdateMaxScore;
        QuizScoreManager.OnScoreChanged += UpdateScore;

        quizManager.OnQuizEnd.AddListener(DisplayScoreUI);
    }
    void OnDisable()
    {
        QuizScoreManager.OnMaxScoreChanged -= UpdateMaxScore;
        QuizScoreManager.OnScoreChanged -= UpdateScore;

        quizManager.OnQuizEnd.RemoveListener(DisplayScoreUI);
    }

    private void UpdateScore(int score)
    {
        scoreUi.text = score.ToString();
    }
    private void UpdateMaxScore(int score)
    {
        scoreUi_max.text = score.ToString();
    }

    private void DisplayScoreUI()
    {
        int star = quizScoreManager.Stars;
        for (int i = 0; i < star; i++)
        {
            starSprites[i].SetActive(true);
        }
        //Debug.Log($"Getting : {star}");
    }

    public void QuitQuizUI()
    {
        foreach(GameObject gameObject in starSprites)
        {
            gameObject.SetActive(false);
        }
    }
}
