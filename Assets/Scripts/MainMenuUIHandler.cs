using System.Collections;
using System.Collections.Generic;
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
    }

    public void BackToMenuFromLeaderboard()
    {
        MainMenuElementsCanvas.SetActive(true);
        LeaderboardCanvas.SetActive(false);
    }
}
