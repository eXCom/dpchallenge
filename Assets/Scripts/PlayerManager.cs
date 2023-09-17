using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public string UserName; // new variable declared

    private void Awake()
    {
        Debug.Log(Application.persistentDataPath + "/savefile.json");
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
        LoadUser();
    }

    [System.Serializable]
    class SaveData
    {
        public string userName;
    }

    public void SaveUser(string userName)
    {
        UserName = userName;
        SaveData data = new SaveData();
        data.userName = userName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadUser()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            UserName = data.userName;
        }
    }
}
