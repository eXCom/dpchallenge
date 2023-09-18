using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public string TopPlayerName; // top player name
    public int TopPlayerScore; // top player score
    public string CurrentPlayerName; // current player name
    public int CurrentPlayerScore; // current player score
    private int maxLeaderboardPlayersAmount = 10;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadTopPlayer();
    }

    public void SaveTopPlayer(string currentPlayerName, int currentPlayerScore)
    {
        string pathToLeaderboardFile = Application.persistentDataPath + "/leaderboard.json";

        if (File.Exists(pathToLeaderboardFile))
        {
            // Load the JSON data from the file (optional)
            string loadedJson = File.ReadAllText(pathToLeaderboardFile);
            LeaderboardDataList lbList = JsonUtility.FromJson<LeaderboardDataList>(loadedJson);

            if (lbList.LeaderboardList.Count < maxLeaderboardPlayersAmount)
            {
                // we still have space in our leaderboard, so just add a new player result to the list
                List<LeaderboardData> newLbList = new List<LeaderboardData>();
                foreach (var player in lbList.LeaderboardList)
                {
                    newLbList.Add(new LeaderboardData { name = player.name.Trim(), score = player.score });
                }
                newLbList.Add(new LeaderboardData { name = currentPlayerName.Trim(), score = currentPlayerScore });
                string leaderboardListJSON = JsonUtility.ToJson(new LeaderboardDataList { LeaderboardList = newLbList.OrderByDescending(o => o.score).ToList() });

                File.WriteAllText(pathToLeaderboardFile, leaderboardListJSON);
            } else
            {
                // Find the player with the lowest score and remove it from the list
                LeaderboardData playerWithLowestScore = null;
                int lowestScore = int.MaxValue;
                List<LeaderboardData> newLbList = new List<LeaderboardData>();
                foreach (var player in lbList.LeaderboardList)
                {
                    newLbList.Add(new LeaderboardData { name = player.name.Trim(), score = player.score });

                    if (player.score < lowestScore)
                    {
                        lowestScore = player.score;
                        playerWithLowestScore = newLbList.Last();
                    }
                }

                if (playerWithLowestScore != null)
                {
                    newLbList.Remove(playerWithLowestScore);
                }
                newLbList.Add(new LeaderboardData { name = currentPlayerName.Trim(), score = currentPlayerScore });
                string leaderboardListJSON = JsonUtility.ToJson(new LeaderboardDataList { LeaderboardList = newLbList.OrderByDescending(o => o.score).ToList() });

                File.WriteAllText(pathToLeaderboardFile, leaderboardListJSON);
            }

        } else
        {
            List<LeaderboardData> leaderboardData = new List<LeaderboardData>();
            leaderboardData.Add(new LeaderboardData { name = currentPlayerName.Trim(), score = currentPlayerScore });
            string leaderboardListJSON = JsonUtility.ToJson(new LeaderboardDataList { LeaderboardList = leaderboardData });

            File.WriteAllText(pathToLeaderboardFile, leaderboardListJSON);
        }
    }

    public void LoadTopPlayer()
    {
        string pathToLeaderboardFile = Application.persistentDataPath + "/leaderboard.json";

        if (File.Exists(pathToLeaderboardFile))
        {
            string loadedJson = File.ReadAllText(pathToLeaderboardFile);
            LeaderboardDataList lbList = JsonUtility.FromJson<LeaderboardDataList>(loadedJson);

            if (lbList.LeaderboardList.Count > 0)
            {
                TopPlayerName = lbList.LeaderboardList[0].name;
                TopPlayerScore = lbList.LeaderboardList[0].score;
                Debug.Log($"Top player name is {TopPlayerName} with the score {TopPlayerScore}");
            }
            else
            {
                TopPlayerName = "";
                TopPlayerScore = 0;
            }
                
        }
    }
}

[System.Serializable]
class LeaderboardData
{
    public string name;
    public int score;
}

[System.Serializable]
class LeaderboardDataList
{
    public List<LeaderboardData> LeaderboardList;
}