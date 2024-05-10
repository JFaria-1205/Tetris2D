using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentLevel { get; private set; }
    public float currentGravity { get; private set; }
    private List<float> gravityValues = new List<float>();
    private int totalLinesCleared = 0;
    private int linesClearedInLevel = 0;
    BlockSpawner blockSpawner;
    GameObject currentBlock;


    void Start()
    {
        PopulateGravityList();
        UpdateLevelAndGravity(1);
        blockSpawner = GetComponent<BlockSpawner>();
        SpawnBlock();
    }

    private void SpawnBlock()
    {
        blockSpawner.SpawnBlock(out currentBlock);
        GetComponent<PlayerController>().UpdateBlock(currentBlock);
    }

    public void BlockLocked()
    {
        //check for clear and award points if cleared and update lines cleared


        //update level and speed
        if (linesClearedInLevel >= 10 && currentLevel < 15)
        {
            //UpdateLevelAndSpeed();
        }


        //spawn next block
        SpawnBlock();
    }

    private void UpdateLevelAndGravity(int level = -1)
    {
        if (level != -1)
        {            
            Mathf.Clamp(level, 1, 15);
            currentLevel = level;            
        }                      
        else
            currentLevel += 1;

        currentGravity = gravityValues[currentLevel-0];

        linesClearedInLevel = 0;
    }

    private void PopulateGravityList()
    {
                                      // Level
        gravityValues.Add(0.01667f);  // 1
        gravityValues.Add(0.021017f); // 2
        gravityValues.Add(0.026977f); // 3
        gravityValues.Add(0.035256f); // 4
        gravityValues.Add(0.04693f);  // 5
        gravityValues.Add(0.06361f);  // 6
        gravityValues.Add(0.0879f);   // 7
        gravityValues.Add(0.1236f);   // 8
        gravityValues.Add(0.1775f);   // 9
        gravityValues.Add(0.2598f);   // 10
        gravityValues.Add(0.388f);    // 11
        gravityValues.Add(0.59f);     // 12
        gravityValues.Add(0.92f);     // 13
        gravityValues.Add(1.46f);     // 14
        gravityValues.Add(2.36f);     // 15
    }
}
