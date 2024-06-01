using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MM_Manager : MonoBehaviour
{
    [SerializeField] Text HighScore_UI;
    [SerializeField] GameObject HighScoreDataSaverObject;

    private void Start()
    {
        SetHighScoreUI();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("GameScene");
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
        if(FindObjectOfType<HighScoreData>() == null)
        {
            Instantiate(HighScoreDataSaverObject);
        }
        HighScore_UI.text = "HighScore: " + FindObjectOfType<HighScoreData>().GetHighScore().ToString();
    }
}
