using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MM_Manager : MonoBehaviour
{
    [SerializeField] Text HighScore_UI;

    private void Start()
    {
        SetHighScoreUI();       
    }

    public void PlayButton()
    {
        Application.LoadLevel(1);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quitting game...");
    }

    public void ResetButton()
    {
        FindObjectOfType<HighScoreData>().SaveNewHighScore(0);
        SetHighScoreUI();
    }

    public void SetHighScoreUI()
    {
        HighScore_UI.text = "HighScore: " + FindObjectOfType<HighScoreData>().GetHighScore().ToString();
    }
}
