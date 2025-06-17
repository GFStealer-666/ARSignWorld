using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizTopicSelector : MonoBehaviour
{
    // Select the specific topic from dropdown 
    [Header("Panel Info")]  
    [SerializeField] private string panelLabel = "Unlabeled Panel";
    [Header("UI Elements")]
    [SerializeField] private TMP_Dropdown topicSelector;

    [Header("Topics for This Panel")]
    [SerializeField] private QuizTopicSO[] quizTopics;
    [SerializeField] private QuizTopicSO selectedTopic;

    private void Start()
    {
        SetupDropdown();
    }

    private void SetupDropdown()
    {
        topicSelector.ClearOptions();

        foreach (var topic in quizTopics)
        {
            topicSelector.options.Add(new TMP_Dropdown.OptionData(topic.name));
        }

        topicSelector.onValueChanged.AddListener(OnTopicChanged);

        if (quizTopics.Length > 0)
            selectedTopic = quizTopics[0];
    }

    private void OnTopicChanged(int index)
    {
        selectedTopic = quizTopics[index];
    }

    public void ChooseEasy() => StartQuiz(selectedTopic, QuizEnum.QuizDifficulty.Easy);
    public void ChooseHard() => StartQuiz(selectedTopic, QuizEnum.QuizDifficulty.Hard);

    public void ChooseRandomTopicAndDifficulty()
    {
        if (quizTopics == null || quizTopics.Length == 0)
        {
            Debug.LogWarning("No topics available for random selection.");
            return;
        }

        var randomTopic = quizTopics[Random.Range(0, quizTopics.Length)];
        var randomDifficulty = (QuizEnum.QuizDifficulty)Random.Range(0, 2); // Easy or Hard

        StartQuiz(randomTopic, randomDifficulty);
    }
    public void ChooseRandomDifficulty()
    {
        var randomDifficulty = (QuizEnum.QuizDifficulty)Random.Range(0, 2); // Easy or Hard
        var randomTopic = quizTopics[Random.Range(0, quizTopics.Length)];
        StartQuiz(randomTopic, randomDifficulty);
    }

    /// Sets up data and loads the quiz scene
    private void StartQuiz(QuizTopicSO topic, QuizEnum.QuizDifficulty difficulty)
    {
        if (topic == null)
        {
            Debug.LogWarning("Quiz topic is null.");
            return;
        }

        QuizDataHolder.selectedTopic = topic;
        QuizDataHolder.selectedDifficulty = difficulty;
        
        //Mainmenu.LoadScene(SceneIndex.QuizGame);
    }
}
