using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class QuizLeaderboardEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject[] starImages;
    [SerializeField] private Image medalImage;
    [SerializeField] private Image numberImage;

    public void SetEntry(string playerName, int score, string time, int stars, int rank, Sprite sprite)
    {
        nameText.text = playerName;
        scoreText.text = $"{score} คะแนน";
        timeText.text = $"{time}";

        for (int i = 0; i < stars; i++)
        {
            starImages[i].SetActive(true);
        }
        numberImage.sprite = sprite;
        if (rank <= 3)
        {
            numberImage.gameObject.SetActive(false);
            medalImage.gameObject.SetActive(true);
            medalImage.sprite = sprite;
            
        }
        else
        {
            medalImage.gameObject.SetActive(false);
            numberImage.gameObject.SetActive(true);
            numberImage.sprite = sprite;
    }
    }
}
