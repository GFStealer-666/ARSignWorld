using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
public class QuizLeaderboardUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuizLeaderboard leaderboard;
    [Header("UI Elements")]
    [SerializeField] private Transform leaderboardContent;
    [SerializeField] private QuizLeaderboardEntryUI entryPrefab;
    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown userTypeDropdown;
    [SerializeField] private TMP_Dropdown topicDropdown;
    [SerializeField] private TMP_Dropdown difficultyDropdown;
    [SerializeField] private TMP_Dropdown sortByDropdown;

    [Header("Number Sprites")]

    // Array of sprites for numbers 1-10, used for leaderboard ranking display
    [SerializeField] private Sprite[] numberSprites; 
    

    private void Start()
    {
        // Populate UserType dropdown
        userTypeDropdown.ClearOptions();
        var userTypes = System.Enum.GetValues(typeof(QuizEnum.QuizUserType)).Cast<QuizEnum.QuizUserType>().ToList();
        userTypeDropdown.AddOptions(userTypes.Select(u => QuizTranslationHelper.UserTypeToThai(u.ToString())).ToList());

        // Populate Difficulty dropdown
        difficultyDropdown.ClearOptions();
        var difficulties = System.Enum.GetValues(typeof(QuizEnum.QuizDifficulty)).Cast<QuizEnum.QuizDifficulty>().ToList();
        difficultyDropdown.AddOptions(difficulties.Select(d => QuizTranslationHelper.DifficultyToThai(d.ToString())).ToList());

        // Populate Sort By dropdown
        sortByDropdown.ClearOptions();
        sortByDropdown.AddOptions(new List<string> { $"คะแนน", "เวลา" });


        // Add listeners
        userTypeDropdown.onValueChanged.AddListener(_ => UpdateTopicDropdown());

        userTypeDropdown.onValueChanged.AddListener(_ => UpdateLeaderboard());
        topicDropdown.onValueChanged.AddListener(_ => UpdateLeaderboard());
        difficultyDropdown.onValueChanged.AddListener(_ => UpdateLeaderboard());
        sortByDropdown.onValueChanged.AddListener(_ => UpdateLeaderboard());

        userTypeDropdown.value = 0;
        userTypeDropdown.RefreshShownValue();
        RefreshLeaderboard();
    }
    
    /// <summary>
    /// Updates the topic dropdown based on the selected user type.
    /// This method is called when the user changes the user type dropdown value.
    private void UpdateTopicDropdown()
    {
        var selectedUserType = (QuizEnum.QuizUserType)userTypeDropdown.value;
        leaderboard.UpdateAvailableTopics(selectedUserType);

        topicDropdown.ClearOptions();
        var topics = leaderboard.GetAvailableAllTopics();
        topicDropdown.AddOptions(topics.Select(t => t.topicName).ToList());

        if (topics.Count > 0)
        {
            topicDropdown.value = 0;
            topicDropdown.RefreshShownValue();
        }

        UpdateLeaderboard();
    }
    /// <summary>
    /// Refreshes the leaderboard by fetching the latest player data.   
    /// This can be called to update the leaderboard UI with the most recent data.
    public void RefreshLeaderboard()
    {
        // Store current selections
        int prevUserType = userTypeDropdown.value;
        int prevTopic = topicDropdown.value;
        int prevDifficulty = difficultyDropdown.value;
        int prevSortBy = sortByDropdown.value;

        leaderboard.FetchPlayersData(() =>
        {
            // After data is fetched, update available topics and UI
            var selectedUserType = (QuizEnum.QuizUserType)prevUserType;
            leaderboard.UpdateAvailableTopics(selectedUserType);

            // Repopulate topic dropdown
            topicDropdown.ClearOptions();
            var topics = leaderboard.GetAvailableAllTopics();
            topicDropdown.AddOptions(topics.Select(t => t.topicName).ToList());

            // Restore previous topic selection if possible
            if (topics.Count > 0)
            {
                topicDropdown.value = Mathf.Clamp(prevTopic, 0, topics.Count - 1);
                topicDropdown.RefreshShownValue();
            }

            // Restore other dropdowns
            userTypeDropdown.value = prevUserType;
            difficultyDropdown.value = prevDifficulty;
            sortByDropdown.value = prevSortBy;

            UpdateLeaderboard();
        });
    }

    /// <summary>
    /// Updates the leaderboard based on the selected filters. 
    /// This method is called when the user changes any dropdown value.
    private void UpdateLeaderboard()
    {
        var selectedUserType = (QuizEnum.QuizUserType)userTypeDropdown.value;
        var selectedTopic = leaderboard.GetAvailableAllTopics().Count > 0 ? leaderboard.GetAvailableAllTopics()[topicDropdown.value].topicName : "";
        var selectedDifficulty = (QuizEnum.QuizDifficulty)difficultyDropdown.value;
        var sortBy = QuizTranslationHelper.SortByThaiToEnglish( sortByDropdown.options[sortByDropdown.value].text);

        List<GoLangPlayerQuizData> results = leaderboard.GetTopFilteredPlayers(
            selectedUserType,
            selectedTopic,
            selectedDifficulty,
            sortBy,
            true
        );

        RenderLeaderboard(results);
    }

    /// <summary>
    /// Updates the leaderboard UI with the latest player data.
    /// This method is called when player data is fetched successfully.
    private void RenderLeaderboard(List<GoLangPlayerQuizData> results)
    {
        // Clear old entries
        foreach (Transform child in leaderboardContent)
            Destroy(child.gameObject);

        // Add new entries
        for (int i = 0; i < results.Count; i++)
        {
            var player = results[i];
            var entry = Instantiate(entryPrefab, leaderboardContent);
            Sprite numberSprite = (i < numberSprites.Length) ? numberSprites[i] : null;
            entry.SetEntry(player.userName, player.score, player.timeTaken, player.stars, i+1,numberSprite);
        }
    }
}
