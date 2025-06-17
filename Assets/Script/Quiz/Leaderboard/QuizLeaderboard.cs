using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
public class QuizLeaderboard : MonoBehaviour
{
    [Header("Data Fetcher")]
    [SerializeField] private PlayerQuizDataFetcher dataFetcher;

    [Header("Leaderboard Settings")]
    [SerializeField] private bool isLeaderboardEnabled = true;
    [SerializeField] private int maxLeaderboardSize = 10;

    [Header("Leaderboard Data")]
    public List<GoLangPlayerQuizData> Players => players;
    [SerializeField] private List<GoLangPlayerQuizData> players = new List<GoLangPlayerQuizData>();
    private Dictionary<string, QuizTopicSO> topicDictionary = new Dictionary<string, QuizTopicSO>();
    [SerializeField] private List<QuizTopicSO> availableTopics = new List<QuizTopicSO>();
    [SerializeField] private List<QuizTopicSO> defaultTopics = new List<QuizTopicSO>();
    private void Awake()
    {
        if (dataFetcher == null)
        {
            dataFetcher = GetComponent<PlayerQuizDataFetcher>();
        }

        if (isLeaderboardEnabled)
        {
            FetchPlayersData();
        }

        SetupDictionary();

        GetTopPlayersBy(p => p.TimeTakenSeconds, maxLeaderboardSize, false);
    }
    /// <summary>
    /// Fetches player data from the data fetcher and populates the players list.
    /// Normalizes userType and difficulty to English for all players.
    public void FetchPlayersData(System.Action onComplete = null)
    {
        if (!isLeaderboardEnabled || dataFetcher == null) return;

        dataFetcher.FetchPlayers(players =>
        {
            this.players = players ?? new List<GoLangPlayerQuizData>();

            // Normalize userType and difficulty to English for all players
            foreach (var p in this.players)
            {
                p.userType = QuizTranslationHelper.UserTypeToEnglish(p.userType);
                p.difficulty = QuizTranslationHelper.DifficultyToEnglish(p.difficulty);
                // If you have topic names in Thai and need to convert, add a similar line here
            }

            onComplete?.Invoke();
        });
    }
    /// <summary>
    /// Sets up the topic dictionary from the default topics.   
    /// This method initializes the topicDictionary with unique keys based on topic name and user type.
    /// It ensures that each topic is only added once, preventing duplicates.
    private void SetupDictionary()
    {
        topicDictionary.Clear();
        foreach (var topic in defaultTopics)
        {
            string key = $"{topic.topicName}_{topic.quizUserType}";
            if (!topicDictionary.ContainsKey(key))
            {
                topicDictionary[key] = topic;
            }
        }

        // topicDictionary = defaultTopics.ToDictionary(
        // t => $"{t.topicName}_{t.quizUserType}", t => t);
    }
    /// <summary>
    /// Retrieves a QuizTopicSO by topic name and user type.
    public QuizTopicSO GetTopicSO(string topicName, QuizEnum.QuizUserType userType)
    {
        string key = $"{topicName}_{userType}";
        if (topicDictionary.TryGetValue(key, out QuizTopicSO topicSO))
        {
            return topicSO;
        }
        else
        {
            Debug.LogWarning($"Topic '{topicName}' with userType '{userType}' not found in default topics.");
            return null;
        }
    }
    // Topic retrieval methods

    /// <summary>
    /// Updates the available topics based on the user type.    
    /// This method filters the players list to find unique topics for the specified user type
    public void UpdateAvailableTopics(QuizEnum.QuizUserType userType)
    {
        var topicNames = players
            .Where(p => p.userType == userType.ToString())
            .Select(p => p.topic)
            .Distinct();

        availableTopics = topicNames
            .Select(topicName => GetTopicSO(topicName, userType))
            .Where(so => so != null)
            .ToList();
    }

    /// <summary>  
    /// Retrieves a list of available QuizTopicSOs for a specific user type.
    /// To display available topics in the UI based on user type.
    public List<QuizTopicSO> GetAvailableTopicSOsForUserType(QuizEnum.QuizUserType userType)
    {
        var topicNames = players
            .Where(p => p.userType == userType.ToString())
            .Select(p => p.topic)
            .Distinct();

        return topicNames
            .Select(topicName => GetTopicSO(topicName, userType))
            .Where(so => so != null)
            .ToList();
    }
    
    // Leaderboard retrieval methods

    /// <summary>
    /// Retrieves leaderboard results based on user type, topic, difficulty, and sorting options.
    /// This method allows filtering by user type, topic, and difficulty, and sorting by score or time taken.
    public List<GoLangPlayerQuizData> GetTopFilteredPlayers(
        QuizEnum.QuizUserType userType,
        string topic,
        QuizEnum.QuizDifficulty difficulty,
        string sortBy,
        bool descending)
    {
        var filtered = players.Where(p =>
            p.userType == userType.ToString() &&
            p.topic == topic &&
            p.difficulty == difficulty.ToString()
        );


        // It's the same filtering logic as before, but now we can sort based on the sortBy parameter
        if (sortBy == "Score")
        {
            filtered = descending // check whether if want high to low or not
                ? filtered.OrderByDescending(p => p.score).
                ThenBy(p => p.TimeTakenSeconds).
                ThenBy(p => p.answerDetails[0].timeToAnswer).
                ThenBy(p => p.answerDetails[1].timeToAnswer) // Secondary sort by time if scores are equal
                : filtered.OrderBy(p => p.score).ThenByDescending(p => p.TimeTakenSeconds);
        }
        else if (sortBy == "Time")
        {
            filtered = descending
                ? filtered.OrderBy(p => p.TimeTakenSeconds).ThenByDescending(p => p.score) // Secondary sort by score if times are equal
                : filtered.OrderByDescending(p => p.TimeTakenSeconds).ThenBy(p => p.score);
        }

        return filtered.Take(maxLeaderboardSize).ToList();
    }
    /// <summary>
    /// Retrieves the rank of a player based on their username, user type, topic, difficulty, and sorting options.
    /// Returns the rank as a 1-based index, or -1 if the player is not found.
    public int GetPlayerRank(string userName, QuizEnum.QuizUserType userType, string topic, QuizEnum.QuizDifficulty difficulty, string sortBy, bool descending)
    {
        var filtered = players.Where(p =>
            p.userType == userType.ToString() &&
            p.topic == topic &&
            p.difficulty == difficulty.ToString()
        );

        if (sortBy == "Score")
            filtered = descending ? filtered.OrderByDescending(p => p.score).ThenBy(p => p.TimeTakenSeconds)
                                : filtered.OrderBy(p => p.score).ThenByDescending(p => p.TimeTakenSeconds);
        else if (sortBy == "Time")
            filtered = descending ? filtered.OrderBy(p => p.TimeTakenSeconds).ThenByDescending(p => p.score)
                                : filtered.OrderByDescending(p => p.TimeTakenSeconds).ThenBy(p => p.score);

        var list = filtered.ToList();
        return list.FindIndex(p => p.userName == userName) + 1; // 1-based rank, -1 if not found
    }

    // Top players retrieval methods with sorting options
    /// <summary>
    /// Retrieves the top players based on a specified key selector and count.
    public List<GoLangPlayerQuizData> GetTopPlayersBy(Func<GoLangPlayerQuizData, object> keySelector, int count, bool descending = true)
    {
        var query = descending
            ? players.OrderByDescending(keySelector)
         : players.OrderBy(keySelector);
        return query.Take(count).ToList();
    }

    /// <summary>
    /// Retrieves the top players based on topic, difficulty, and user type, sorted by score.
    public List<GoLangPlayerQuizData> GetDefaultTopPlayers(string topic, string difficulty, string userType) // Default by score
    {
        var querry = players.Where(p =>
            (string.IsNullOrEmpty(topic) || p.topic == topic) &&
            (string.IsNullOrEmpty(difficulty) || p.difficulty == difficulty) &&
            (string.IsNullOrEmpty(userType) || p.userType == userType))
            .OrderByDescending(p => p.score)
            .Take(maxLeaderboardSize)
            .ToList();
        return querry;
    }
    /// <summary>
    /// Retrieves the top players based on topic, difficulty, and user type, sorted by time taken.
    public List<GoLangPlayerQuizData> GetPlayersByScore()
    {
        this.players = players.
        OrderByDescending(p => p.score).
        Take(maxLeaderboardSize).
        ToList();

        return players;
    }
    public List<GoLangPlayerQuizData> GetPlayersByTime()
    {
        this.players = players.
        OrderBy(p => p.TimeTakenSeconds).
        Take(maxLeaderboardSize).
        ToList();

        return players;
    }
    /// <summary>
    /// Retrieves the personal best score for a player based on username, topic, and difficulty.
    /// If multiple entries exist, it returns the one with the highest score and lowest time taken.
    public GoLangPlayerQuizData GetPersonalBest(string userName, string topic, string difficulty)
    {
        return players.Where(p => p.userName == userName && p.topic == topic && p.difficulty == difficulty)
                    .OrderByDescending(p => p.score)
                    .ThenBy(p => p.TimeTakenSeconds)
                    .FirstOrDefault();
    }

    public List<GoLangPlayerQuizData> GetPlayerUserByType(string userType)
    {
        return players.Where(p => p.userType == userType).ToList();
    }
    public List<GoLangPlayerQuizData> GetPlayersByTopic(string topic)
    {
        return players.Where(p => p.topic == topic).ToList();
    }

    public List<GoLangPlayerQuizData> GetPlayersByAge(int age)
    {
        return players.Where(p => p.age == age).ToList();
    }

    // Single player data retrieval methods
    public GoLangPlayerQuizData GetPlayerByUniqueId(string uniqueId)
    {
        return players.FirstOrDefault(p => p.uniqueId == uniqueId);
    }
    public GoLangPlayerQuizData GetPlayerByName(string userName)
    {
        return players.FirstOrDefault(p => p.userName == userName);
    }

    // Other Utility data methods
    public float GetAverageScore()
    {
        return players.Any() ? (float)players.Average(p => p.score) : 0f;
    }
    public List<QuizTopicSO> GetAvailableAllTopics()
    {
        return availableTopics;
    }

    public int GetPlayerCount()
    {
        return players.Where(p => !string.IsNullOrEmpty(p.userName)).Count();
    }
    
    public void ClearLeaderboard()
    {
        players.Clear();
    }
}
