using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private GameObject currentBlock;
    private BlockMovement movement;
    private PauseMenu pauseMenu;

    private bool movingRight = false;
    private bool holdingRight = false;
    private bool canceledRight = false;

    private bool movingLeft = false;
    private bool holdingLeft = false;
    private bool canceledLeft = false;

    private float movingInitialDelay = 0.18f;
    private float movingRepeatedDelay = 0.05f;
    private float movingRightInitialDelayCount = 0f;
    private float movingLeftInitialDelayCount = 0f;
    private float delayAdd = 0.01f;

    private void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    public void ResetMovement()
    {
        movingRight = false;
        movingLeft = false;
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
        HandleInput();
    }

    private void HandleInput()
    {
        if (!pauseMenu.IsGamePaused()) //if the game is not paused then you can move
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) //moving right
            {
                holdingRight = true;
                if (!movingRight)
                {
                    MoveRight();
                }
            }
            else //not moving right
            {
                movingRightInitialDelayCount = 0;
                holdingRight = false;
                movingRight = false;
                StopCoroutine(HoldMoveRight());
                canceledRight = false;

                if (canceledLeft && holdingLeft)
                {
                    canceledLeft = false;
                    MoveLeft();
                }
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) //moving left
            {
                holdingLeft = true;
                if (!movingLeft)
                {
                    MoveLeft();
                }
            }
            else //not moving left
            {
                movingLeftInitialDelayCount = 0;
                holdingLeft = false;
                movingLeft = false;
                StopCoroutine(HoldMoveLeft());
                canceledLeft = false;

                if (canceledRight && holdingRight)
                {
                    canceledRight = false;
                    MoveRight();
                }
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) //Increase Block Down Speed
                movement.IncreaseBlockSpeed(true);

            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) //Stop Increasing Block Down Speed
                movement.IncreaseBlockSpeed(false);

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) //Rotate Right (Left Click)
                movement.RotateBlock(true);

            if (Input.GetMouseButtonDown(1)) //Rotate Left (Right Click (alt click))
                movement.RotateBlock(false);

            if (Input.GetKeyUp(KeyCode.Space)) //Hard Drop
                CameraShake.Shake(0.3f, 5f, 0.05f, 0.1f);

            if (movingRight && movingLeft)
            {
                if (canceledRight)
                {
                    movingRightInitialDelayCount = 0;
                    StopCoroutine(HoldMoveRight());
                }
                else if (canceledLeft)
                {
                    movingLeftInitialDelayCount = 0;
                    StopCoroutine(HoldMoveLeft());
                }
            }
        }
    }

    #region MoveRight
    private void MoveRight()
    {
        movingRight = true;
        if (movingLeft)
        {
            movingLeftInitialDelayCount = 0;
            canceledLeft = true;
            StopCoroutine(HoldMoveLeft());
        }
        StartCoroutine(HoldMoveRight());       
    }

    private IEnumerator HoldMoveRight()
    {
        movement.MoveRight();

        while (movingRight && movingRightInitialDelayCount < movingInitialDelay)
        {
            movingRightInitialDelayCount += delayAdd;
            yield return new WaitForSeconds(delayAdd);
        }

        while (movingRight)
        {
            if (movement.MoveRight())
                yield return new WaitForSeconds(movingRepeatedDelay);
            else
                break;
        }
    }
    #endregion

    #region MoveLeft
    private void MoveLeft()
    {
        movingLeft = true;
        if (movingRight)
        {
            movingRightInitialDelayCount = 0;
            canceledRight = true;
            StopCoroutine(HoldMoveRight());            
        }            
        StartCoroutine(HoldMoveLeft());
    }

    private IEnumerator HoldMoveLeft()
    {
        
        movement.MoveLeft();

        while (movingLeft && movingLeftInitialDelayCount < movingInitialDelay)
        {
            movingLeftInitialDelayCount += delayAdd;
            yield return new WaitForSeconds(delayAdd);
        }

        while (movingLeft)
        {
            if (movement.MoveLeft())
                yield return new WaitForSeconds(movingRepeatedDelay);
            else
                break;
        }
    }
    #endregion
}
