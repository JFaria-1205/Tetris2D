using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private GameObject currentBlock;
    private BlockMovement movement;

    public void UpdateBlock(GameObject newBlock)
    {
        currentBlock = newBlock;
        movement = currentBlock.gameObject.GetComponent<BlockMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Input
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) //Move Right
            movement.MoveRight();

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) //Move Left
            movement.MoveLeft();

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) //Increase Block Down Speed
            movement.IncreaseBlockSpeed(true);

        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) //Stop Increasing Block Down Speed
            movement.IncreaseBlockSpeed(false);

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) //Rotate Right (Right Click)
            movement.RotateBlock(true);

        if (Input.GetMouseButtonDown(1)) //Rotate Left (Left Click)
            movement.RotateBlock(false);

        if (Input.GetKeyUp(KeyCode.Space)) //Hard Drop
            Debug.Log("Hard drop not yet implemented");
        #endregion
    }

    IEnumerator StartMoveRight()
    {
        return null;
    }

    IEnumerator StartMoveLeft()
    {
        return null;
    }
}
