using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    public float Hp;
    public float Dm;
    public float RespawnDelay;

    private static int _di = 0;
    private string[] _difficultyNameArr = { "Easy", "Nomal", "Hard", "Sunfish" };
    public string _difficultyName;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _difficultyName = _difficultyNameArr[_di];
        Difficulty(_difficultyName);
    }

    void Update()
    {
        
    }

    public void DifficultyChange()
    {
        _di++;
        _di %= 4;
        _difficultyName = _difficultyNameArr[_di];
        Difficulty(_difficultyName);
    }

    private void Difficulty(string dn)
    {
        switch (dn)
        {
            case "Easy":
                Hp = 0.4f;
                Dm = 0.4f;
                RespawnDelay = 2f;
                break;
            case "Nomal":
                Hp = 0.8f;
                Dm = 0.8f;
                RespawnDelay = 1f;
                break;
            case "Hard":
                Hp = 1.2f;
                Dm = 1.2f;
                RespawnDelay = 0.7f;
                break;
            case "Sunfish":
                Hp = 0.8f;
                Dm = 1000f;
                RespawnDelay = 1f;
                break;
        }
    }
}
