using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MainMenuUIHandler : MonoBehaviour
{
    public GameObject MainMenuElementsCanvas;
    public GameObject LeaderboardCanvas;
    public TextMeshProUGUI userName;
    public TextMeshProUGUI userBestScore;
    public static PlayerManager Instance;
    public GameObject rowPrefab;
    public Transform rowsParent;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerManager.Instance != null)
        {
            //Debug.Log("Player instance is not null");
            userBestScore.text = $"Best Score: {PlayerManager.Instance.TopPlayerName} : {PlayerManager.Instance.TopPlayerScore}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.CurrentPlayerName = userName.text;
            //PlayerManager.Instance.SaveUser(userName.text);
            SceneManager.LoadScene("main");
        }
        else
        {
            Debug.LogError("PlayerManager.Instance is null.");
        }

        //Debug.Log(userName.text);
    }

    public void DisplayLeaderboard()
    {
        MainMenuElementsCanvas.SetActive(false);
        LeaderboardCanvas.SetActive(true);
        LoadLeaderboardPlayers();
    }

    public void BackToMenuFromLeaderboard()
    {
        MainMenuElementsCanvas.SetActive(true);
        LeaderboardCanvas.SetActive(false);
    }

    public void LoadLeaderboardPlayers()
    {
        foreach(Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        string pathToLeaderboardFile = Application.persistentDataPath + "/leaderboard.json";
        if (File.Exists(pathToLeaderboardFile))
        {
            // Load the JSON data from the file (optional)
            string loadedJson = File.ReadAllText(pathToLeaderboardFile);
            LeaderboardDataList lbList = JsonUtility.FromJson<LeaderboardDataList>(loadedJson);

            int index = 1;
            foreach (var item in lbList.LeaderboardList)
            {
                GameObject newGo = Instantiate(rowPrefab, rowsParent);
                TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();

                if (texts.Length >= 1)
                {
                    texts[0].text = index.ToString();
                    texts[1].text = item.name;
                    texts[2].text = item.score.ToString();
                }
                else
                {
                    Debug.LogError("No TextMeshProUGUI component found in the rowPrefab.");
                }

                index++;
            }
        }

            
    }
}
