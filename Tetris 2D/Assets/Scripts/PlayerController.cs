using System.Collections;
using System.Collections.Generic;
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
        if (Input.GetKey(KeyCode.D)) //Right
        {
            Debug.Log("I am Right");
        }
        if (Input.GetKey(KeyCode.A)) //Left
        {
            Debug.Log("I am Left");
        }
        if (Input.GetKey(KeyCode.S)) //Down
        {
            Debug.Log("I am Down");
        }
        if (Input.GetMouseButtonDown(0)) //Right Click
        {
            movement.RotateBlock(true);
        }
        if (Input.GetMouseButtonDown(1)) //Left Click
        {
            movement.RotateBlock(false);
        }
    }
}
