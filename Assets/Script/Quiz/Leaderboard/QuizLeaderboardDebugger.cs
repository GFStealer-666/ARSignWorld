// using UnityEngine;
// using UnityEditor;
// using System.Linq;
// using Unity.VisualScripting;
// using System.Collections.Generic;
// using System;

// public class QuizLeaderboardDebugger : EditorWindow
// {
//     private QuizLeaderboard leaderboard;
//     private string searchName = "";
//     private int searchScore = 11;
//     private int userTypeIndex, topicIndex, difficultyIndex;
//     private bool enableFiltering = false;
//     private bool enableScoreSearch = false;
//     private Vector2 scrollPos; 
//     [MenuItem("Tools/Quiz Leaderboard Debugger")]
//     public static void ShowWindow()
//     {
//         GetWindow<QuizLeaderboardDebugger>("Quiz Leaderboard Debugger");
//     }

//     // ...existing code...

//     private void OnGUI()
//     {
//         scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
//         GUILayout.Label("Player Statistics", EditorStyles.boldLabel);
//         //leaderboard = (QuizLeaderboard)EditorGUILayout.ObjectField("Leaderboard", leaderboard, typeof(QuizLeaderboard), true);

//         if (leaderboard == null)
//             leaderboard = FindFirstObjectByType<QuizLeaderboard>();

//         if (leaderboard == null)
//         {
//             EditorGUILayout.HelpBox("No QuizLeaderboard found in the scene. Please add one.", MessageType.Warning);
//             return;
//         }

//         if (leaderboard.Players == null || leaderboard.Players.Count == 0)
//         {
//             EditorGUILayout.HelpBox("No player data found in the QuizLeaderboard.", MessageType.Info);
//             return;
//         }

//         var players = leaderboard.Players;

//         // Statistics
//         EditorGUILayout.LabelField("Player Count", leaderboard.GetPlayerCount().ToString());
//         EditorGUILayout.LabelField("Mean Score", leaderboard.GetAverageScore().ToString("F2"));

//         int userCount = players.DistinctBy(p => p.userName).Count();
//         EditorGUILayout.LabelField("Unique Users", userCount.ToString());
    
//         int uniqueTopics = players.Select(p => p.topic).Distinct().Count();
//         int uniqueUserTypes = players.Select(p => p.userType).Distinct().Count();
//         int uniqueDifficulties = players.Select(p => p.difficulty).Distinct().Count();

//         EditorGUILayout.LabelField($"Unique Topics: {uniqueTopics}");

//         EditorGUILayout.LabelField("Statistics", EditorStyles.boldLabel);

//         var fastestplayer = players.OrderBy(p => p.TimeTakenSeconds).FirstOrDefault();
//         EditorGUILayout.LabelField("Fastest Player", fastestplayer != null ?
//         $"{fastestplayer.userName} | Time: {fastestplayer.timeTaken}" : "N/A");

//         var slowestplayer = players.OrderByDescending(p => p.TimeTakenSeconds).FirstOrDefault();
//         EditorGUILayout.LabelField("SlowestPlayer", slowestplayer != null ?
//         $"{slowestplayer.userName} | Time : {slowestplayer.timeTaken}" : "N/A");

//         var lastestPlayer = players.OrderByDescending(p => p.timestamp).FirstOrDefault();
//         EditorGUILayout.LabelField("Latest Player", lastestPlayer != null ?
//         $"{lastestPlayer.userName} | Time : {lastestPlayer.timeTaken} | Score: {lastestPlayer.score}" : "N/A");


//         int maxScore = players.Max(p => p.score);
//         var topPlayers = players.Where(p => p.score == maxScore).ToList();
//         EditorGUILayout.Space();
//         EditorGUILayout.LabelField("Player(s) with Highest Score", EditorStyles.boldLabel);
//         foreach (var p in topPlayers)
//         {
//             EditorGUILayout.LabelField($"{p.userName} | Score: {p.score} | Time: {p.timeTaken}");
//         }

//         enableFiltering = EditorGUILayout.Toggle("Enable Filtering", enableFiltering);
//         List<GoLangPlayerQuizData> topfilteredPlayers = new List<GoLangPlayerQuizData>();

//         EditorGUILayout.Space();
//         if (enableFiltering)
//         {
//             EditorGUILayout.LabelField("Player Filters", EditorStyles.centeredGreyMiniLabel);

//             // Create dropdowns based on available records in database
//             string[] userTypes = players.Select(p => p.userType).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToArray();
//             string[] topics = players.Select(p => p.topic).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToArray();
//             string[] difficulties = players.Select(p => p.difficulty).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToArray();

//             userTypeIndex = EditorGUILayout.Popup("User Type", userTypeIndex, userTypes);
//             topicIndex = EditorGUILayout.Popup("Topic", topicIndex, topics);
//             difficultyIndex = EditorGUILayout.Popup("Difficulty", difficultyIndex, difficulties);

//             EditorGUILayout.Space();

//             topfilteredPlayers = players
//             .Where(p => string.Equals(p.userType, userTypes[userTypeIndex], StringComparison.Ordinal))
//             .Where(p => string.Equals(p.topic, topics[topicIndex], StringComparison.Ordinal))
//             .Where(p => string.Equals(p.difficulty, difficulties[difficultyIndex], StringComparison.Ordinal))
//             .OrderByDescending(p => p.score)
//             .ThenBy(p => p.TimeTakenSeconds)
//             .Take(10)
//             .ToList();

//             GUILayout.Label("Filtered Players", EditorStyles.boldLabel);
//             foreach (var p in topfilteredPlayers)
//             {
//                 EditorGUILayout.LabelField($"{p.userName} | Score: {p.score} | Time: {p.timeTaken}");
//             }


//             EditorGUILayout.Space();
//             if (topfilteredPlayers.Count == 0)
//             {
//                 EditorGUILayout.LabelField("No players found with the selected filters.");
//                 return;
//             }
//         }
//         // Search
//         EditorGUILayout.LabelField("Search Player", EditorStyles.boldLabel);

//         searchName = EditorGUILayout.TextField("Player Name", searchName);
//         if (!string.IsNullOrEmpty(searchName))
//         {
//             var nameFound = players.Where(p => p.userName.ToLower().Contains(searchName.ToLower())).ToList();
//             EditorGUILayout.LabelField($"Found: {nameFound.Count}");
//             foreach (var p in nameFound)
//             {
//                 EditorGUILayout.LabelField($"{p.userName} | Topic {p.topic} | Score: {p.score} | Time: {p.timeTaken}");
//             }
//         }

//         searchScore = EditorGUILayout.IntField("Score to Search", searchScore);
//         if (!string.IsNullOrEmpty(searchScore.ToString()))
//         {
//             var scoreFound = players.Where(p => p.score == searchScore).OrderBy(p => p.TimeTakenSeconds).ToList();
//             EditorGUILayout.LabelField($"Found: {scoreFound.Count}");
//             foreach (var p in scoreFound)
//             {

//                 EditorGUILayout.LabelField($"{p.userName} | Topic {p.topic} | Score: {p.score} | Time: {p.timeTaken}");
//             }
//         }
//         EditorGUILayout.EndScrollView();
//     }
// }