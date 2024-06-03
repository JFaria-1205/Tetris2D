using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int highScore_data;

    public PlayerData (HighScoreData highScoreData)
    {
        highScore_data = highScoreData.GetHighScore();
    }
}
