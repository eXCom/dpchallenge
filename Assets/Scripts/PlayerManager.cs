using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public string TopPlayerName; // top player name
    public int TopPlayerScore; // top player score
    public string CurrentPlayerName; // current player name
    public int CurrentPlayerScore; // current player score

    private void Awake()
    {
        //Debug.Log(Application.persistentDataPath + "/savefile.json");
        //Debug.Log("PlayerManager Awake() is called");
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadTopPlayer();
    }

    [System.Serializable]
    class SaveData
    {
        public string topPlayerName;
        public int topPlayerScore;
    }

    public void SaveTopPlayer(string topPlayerName, int topPlayerScore)
    {
        SaveData data = new SaveData();
        data.topPlayerName = topPlayerName;
        data.topPlayerScore = topPlayerScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadTopPlayer()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            TopPlayerName = data.topPlayerName;
            TopPlayerScore = data.topPlayerScore;
        }
    }
}
