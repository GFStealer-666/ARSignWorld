using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class PlayerQuizDataFetcher : MonoBehaviour
{
    protected string apiUrl = "https://arsignworlddatabase-arsign.up.railway.app/leaderboard";

    public void FetchPlayers(System.Action<List<GoLangPlayerQuizData>> onFetched)
    {
        StartCoroutine(GetPlayersCoroutine(onFetched));
    }

    private IEnumerator GetPlayersCoroutine(System.Action<List<GoLangPlayerQuizData>> onFetched)
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = "{\"players\":" + request.downloadHandler.text + "}";
            PlayerQuizDataList dataList = JsonUtility.FromJson<PlayerQuizDataList>(json);
            onFetched?.Invoke(dataList.players);
        }
        else
        {
            Debug.LogError("Failed to fetch players: " + request.error);
            onFetched?.Invoke(null);
        }
    }
}

public class PlayerQuizDataList
{
    public List<GoLangPlayerQuizData> players;
}