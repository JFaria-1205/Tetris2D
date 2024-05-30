using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private GameObject currentBlock;
    private BlockMovement movement;
    private PauseMenu pauseMenu;

    private bool holdingRight = false;
    private bool holdingLeft = false;
    private float moveRightDelay = 0f;
    private float moveLeftDelay = 0f;
    private float movingInitialDelay = 0.18f;
    private float movingRepeatedDelay = 0.04f;

    private void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    public void UpdateBlock(GameObject newBlock)
    {
        currentBlock = newBlock;
        movement = currentBlock.gameObject.GetComponent<BlockMovement>();
    }

    public void ChangePauseStateForBlock(bool paused)
    {
        movement.GamePaused(paused);
    }

    // Update is called once per frame
    void Update()
    {
        #region Input
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !pauseMenu.IsGamePaused()) //Move Right
            Move();

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !pauseMenu.IsGamePaused()) //Move Left
            Move(false);

        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && !pauseMenu.IsGamePaused()) //Increase Block Down Speed
            movement.IncreaseBlockSpeed(true);

        if ((Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) && !pauseMenu.IsGamePaused()) //Stop Increasing Block Down Speed
            movement.IncreaseBlockSpeed(false);

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !pauseMenu.IsGamePaused()) //Rotate Right (Right Click)
            movement.RotateBlock(true);

        if ((Input.GetMouseButtonDown(1)) && !pauseMenu.IsGamePaused()) //Rotate Left (Left Click)
            movement.RotateBlock(false);

        if ((Input.GetKeyUp(KeyCode.Space)) && !pauseMenu.IsGamePaused()) //Hard Drop
            Debug.Log("Hard drop not yet implemented");

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu.PauseOrResumeGame();
        }
            

        //Hold move right and left not working!!!
        /*
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) //Move Right
        {
            if (!holdingRight)
            {
                holdingRight = true;
                StopCoroutine(MoveLeftHold());
                Move();
                StartCoroutine(MoveRightHold());
            }
        }
        else //Stop Moving Right
        {
            moveRightDelay = 0f;
            StopCoroutine(MoveRightHold());
            holdingRight = false;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
        {
            if (!holdingLeft)
            {
                holdingLeft = true;
                StopCoroutine(MoveRightHold());
                Move(false);
                StartCoroutine(MoveLeftHold());
            }            
        }
        else //Stop Moving Left
        {
            moveLeftDelay = 0f;
            StopCoroutine(MoveLeftHold());
            holdingLeft = false;
        }
        */
        #endregion
    }

    private void Move(bool moveRight = true)
    {
        if (moveRight)
            movement.MoveRight();
        else
            movement.MoveLeft();
    }

    //Hold move right and left not working!!!
    /*
    IEnumerator MoveRightHold()
    {
        float delayAdd = 0.1f;
        moveRightDelay = 0f;
        while (moveRightDelay < movingInitialDelay)
        {
            yield return new WaitForSeconds(delayAdd);
            moveRightDelay += delayAdd;
        }

        while (true)
        {
            if (!movement.MoveRight())
                break;
            yield return new WaitForSeconds(movingRepeatedDelay);
        }
    }

    IEnumerator MoveLeftHold()
    {
        float delayAdd = 0.1f;
        moveLeftDelay = 0f;
        movement.MoveLeft();
        while (moveLeftDelay < movingInitialDelay)
        {
            yield return new WaitForSeconds(delayAdd);
            moveLeftDelay += delayAdd;
        }

        while (true)
        {
            if (!movement.MoveLeft())
                break;
            yield return new WaitForSeconds(movingRepeatedDelay);
        }
    }
    */
}
