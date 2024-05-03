using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private GameObject currentBlock;

    public void UpdateBlock(GameObject newBlock)
    {
        currentBlock = newBlock;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
