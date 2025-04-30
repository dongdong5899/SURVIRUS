using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public SaveData(string name, int score)
    {
        _name = name;
        _score = score;
    }

    public string _name;
    public int _score;
}

public static class SaveSystem
{
    private static string SavePath = Application.persistentDataPath + "/save/";

    public static void Save(SaveData saveData, string saveFileName)
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        string saveJson = JsonUtility.ToJson(saveData);
        string saveFilePath = SavePath + saveFileName + ".json";

        File.WriteAllText(saveFilePath, saveJson);
    }

    public static SaveData Load(string saveFileName)
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (!File.Exists(saveFilePath))
        {
            Debug.Log("오류 파일 없음");

            return null;
        }

        string saveFile = File.ReadAllText(saveFilePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(saveFile);
        return saveData;
    }
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private void Awake()
    {
        if (ScoreManager.Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ScoreSave(string name, int score)
    {
        SaveData saveData = new SaveData(name, score);
        SaveSystem.Save(saveData, "score");
    }

    public SaveData ScoreLoad()
    {
        return SaveSystem.Load("score");
    }
}
