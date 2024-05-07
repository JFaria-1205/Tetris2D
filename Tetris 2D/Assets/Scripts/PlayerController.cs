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
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) //Right
            movement.MoveRight();

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) //Left
            movement.MoveLeft();

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) //Down
            Debug.Log("I am Down");

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J)) //Right Click
            movement.RotateBlock(true);

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.K)) //Left Click
            movement.RotateBlock(false);

        if (Input.GetKeyUp(KeyCode.Space))
            Debug.Log("I am Space");
        #endregion
    }
}
