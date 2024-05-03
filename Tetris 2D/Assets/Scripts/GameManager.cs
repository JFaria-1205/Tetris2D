using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentLevel { get; private set; }
    public float currentSpeed { get; private set; }
    private Dictionary<int, float> levelAndSpeed = new Dictionary<int, float>();
    BlockSpawner blockSpawner;
    GameObject currentBlock;

    private void Awake()
    {
        PopulateLevelAndSpeedDictionary();
    }

    void Start()
    {
        UpdateLevelAndSpeed(true, 1);
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
        //check for clear and award points if cleared

        //update level and speed

        //spawn next block
        SpawnBlock();
    }

    private void UpdateLevelAndSpeed(bool custom = false, int level = 1)
    {
        if (custom) 
            currentLevel = level;             
        else
            currentLevel += 1;

        currentSpeed = levelAndSpeed[currentLevel];
    }

    private void PopulateLevelAndSpeedDictionary()
    {
        levelAndSpeed.Add(1, 0.5f);
        levelAndSpeed.Add(2, 1.5f);
        levelAndSpeed.Add(3, 1.5f);
        levelAndSpeed.Add(4, 1.5f);

    }
}
