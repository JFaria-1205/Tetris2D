using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreData : MonoBehaviour
{
    private int highScore = 0;

    public static HighScoreData instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScoreFromSaveData();
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public void SaveNewHighScore(int newHighScore)
    {
        highScore = newHighScore;
        SaveSystem.SavePlayerStats(this);
    }

    public void LoadHighScoreFromSaveData()
    {
        PlayerData data = SaveSystem.LoadPlayerStats();

        if (data != null )
        {
            highScore = data.highScore_data;
        }
        else
        {
            highScore = 0;
            SaveNewHighScore(highScore);
        }
    }
}
