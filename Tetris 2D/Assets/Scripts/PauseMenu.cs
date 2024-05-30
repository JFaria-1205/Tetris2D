using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pause_Menu;
    [SerializeField] GameObject button_Menu;
    [SerializeField] GameObject countdown_Menu;
    [SerializeField] Text countdownNumber;

    private PlayerController playerController;

    private E_PauseState pauseState = E_PauseState.Unpaused;

    private bool resumeCountdownInitiated = false;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public bool IsGamePaused()
    {
        if (pauseState == 0) //unpaused
            return false;
        else //paused
            return true;
    }

    public void PauseOrResumeGame()
    {
        if (IsGamePaused())
        {
            if (!resumeCountdownInitiated)
            {
                StartCoroutine(ResumeGame());
            }
        }
        else
        {            
            countdown_Menu.SetActive(false);
            button_Menu.SetActive(true);
            pause_Menu.SetActive(true);
            pauseState = E_PauseState.Paused;
            resumeCountdownInitiated = false;
            playerController.ChangePauseStateForBlock(IsGamePaused());
        }
    }

    private IEnumerator ResumeGame()
    {
        resumeCountdownInitiated = true;
        button_Menu.SetActive(false);
        countdown_Menu.SetActive(true);

        for (int countdownTimer = 3; countdownTimer > 0; countdownTimer--)
        {
            countdownNumber.text = countdownTimer.ToString();
            yield return new WaitForSeconds(1f);
        }
        
        pauseState = E_PauseState.Unpaused;
        pause_Menu.SetActive(false);
        playerController.ChangePauseStateForBlock(IsGamePaused());
    }

    public void QuitToMenuButton()
    {
        Application.LoadLevel(0);
    }
}
