using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] LayerMask bounds;
    [SerializeField] Text UI_Score;
    [SerializeField] Text UI_Level;
    [SerializeField] Text UI_LineClears;
    [SerializeField] Text UI_HighScore;
    [SerializeField] GameObject singleBlock;
    [SerializeField] RawImage nextBlockImage;
    public int currentLevel { get; private set; }
    public float currentGravity { get; private set; }
    private List<float> gravityValues = new List<float>();

    private int totalLinesCleared = 0;
    private int linesClearedInLevel = 0;
    private int playerScore = 0;
    private int playerHighScore;

    BlockSpawner blockSpawner;
    GameObject currentBlock;
    GameObject nextBlock = null;


    void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        playerScore = 0;
        UpdateScoreUI();

        PopulateGravityList();
        UpdateLevelAndGravity(1);
        UpdateLevelUI();

        UpdateLineClearsUI();

        blockSpawner = GetComponent<BlockSpawner>();
        SpawnBlock();
    }

    private void SpawnBlock()
    {
        blockSpawner.SpawnBlock(out currentBlock, out GameObject nextBlock, out bool gameOver);
        GetComponent<PlayerController>().UpdateBlock(currentBlock);
        UpdateNextBlockUI(nextBlock);

        if (gameOver)
            GameOver();
    }

    public void BlockLocked()
    {
        //check for clear and award points if cleared and update lines cleared
        RowsCleared(out int amountOfRowsCleared);
        //Debug.Log("Rows Cleared: " + amountOfRowsCleared);

        if (amountOfRowsCleared > 0)
        {
            AwardScorePoints(amountOfRowsCleared);
            linesClearedInLevel += amountOfRowsCleared;
            totalLinesCleared += amountOfRowsCleared;
            UpdateLineClearsUI();

            if (linesClearedInLevel >= 10 && currentLevel < 15)
                UpdateLevelAndGravity();
        }

        Invoke("SpawnBlock", 0.75f);
    }

    private void GameOver()
    {
        Debug.Log("Game over loser, you fucking suck");
    }

    private void AwardScorePoints(int numberOfRowsCleared)
    {
        int pointMultiplier;

        switch (numberOfRowsCleared)
        {
            default:
                pointMultiplier = 100;
                break;
            case 1: //single
                pointMultiplier = 100;
                break;
            case 2: //double
                pointMultiplier = 300;
                break;
            case 3: //triple
                pointMultiplier = 500;
                break;
            case 4: //tetris
                pointMultiplier = 800;
                break;
        }

        playerScore += pointMultiplier * currentLevel;

        UpdateScoreUI();
    }

    private void RowsCleared(out int amountCleared)
    {
        amountCleared = 0;
        float highestRowYValue = -5;

        List<Transform> childrenTransforms = currentBlock.GetComponentsInChildren<Transform>().ToList();
        childrenTransforms.Remove(currentBlock.transform);

        List<Transform> childrenToCheckRows = new List<Transform>();

        foreach (Transform child1 in childrenTransforms)
        {
            bool addChild = true;

            foreach (Transform child2 in childrenToCheckRows)
            {
                if (child2.position.y == child1.position.y)
                {
                    addChild = false;
                    break;
                }
            }

            if (addChild)
                childrenToCheckRows.Add(child1);
        }

        List<Transform> blocksToCheck = new List<Transform>();

        foreach (Transform childToCheck in childrenToCheckRows)
        {
            float castStartPos = -2.5f;
            int blocksHit = 0;
            while (blocksHit < 10)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast((new Vector2(castStartPos, childToCheck.position.y)), Vector2.right, 0.1f, bounds);
                if (raycastHit2D.collider != null) //if you hit a block
                {
                    //Debug.DrawLine((new Vector2(castStartPos, childToCheck.position.y)), (new Vector2(castStartPos + 0.1f, childToCheck.position.y)), Color.green, 2f);
                    //Debug.Log(raycastHit2D.collider.name);   
                    blocksToCheck.Add(raycastHit2D.collider.transform);
                    castStartPos += 0.5f;
                    blocksHit++;
                }
                else
                {
                    //Debug.DrawLine((new Vector2(castStartPos, childToCheck.position.y)), (new Vector2(castStartPos + 0.1f, childToCheck.position.y)), Color.red, 2f);
                    break;
                }                    
            }

            //Debug.Log("Blocks hit for row at y = " + childToCheck.position.y.ToString() + ": " + blocksHit);

            if (blocksHit >= 10)
            {
                if (childToCheck.position.y >= highestRowYValue)
                    highestRowYValue = childToCheck.position.y;

                foreach (Transform child in blocksToCheck)
                {
                    child.parent.gameObject.GetComponent<BlockClearingTracker>().RemoveChild(child.gameObject);
                }

                amountCleared++;
            }

            blocksToCheck.Clear();
        }

        if (amountCleared > 0)
            MoveBlocksDownAfterClear(highestRowYValue, amountCleared);
    }

    private void MoveBlocksDownAfterClear(float highestRowClearedYPos, int rowsCleared)
    {
        var blocksToBeMoved = GameObject.FindGameObjectsWithTag("Block");      
        foreach (GameObject child in blocksToBeMoved)
        {
            if (child.transform.position.y > highestRowClearedYPos)
            {
                child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y - (0.5f * rowsCleared), child.transform.position.z);
            }
        }
    }

    private void UpdateLevelAndGravity(int level = -1)
    {
        if (level != -1)
        {            
            Mathf.Clamp(level, 1, 15);
            currentLevel = level;            
        }                      
        else
            currentLevel = Mathf.Clamp(currentLevel+1, 1, 15);

        currentGravity = gravityValues[currentLevel-0];

        linesClearedInLevel = 0;

        UpdateLevelUI();
    }

    private void PopulateGravityList()
    {                                 // Level
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

    #region UI_Functions
    private void UpdateScoreUI()
    {
        UI_Score.text = (playerScore.ToString());
        if (playerScore >= playerHighScore)
        {
            playerHighScore = playerScore;
            UI_HighScore.text = (playerHighScore.ToString());
        }
    }

    private void UpdateLevelUI()
    {
        UI_Level.text = (currentLevel.ToString());
    }

    private void UpdateNextBlockUI(GameObject nextBlock)
    {
        nextBlockImage.texture = AssetPreview.GetAssetPreview(nextBlock);
    }

    private void UpdateLineClearsUI()
    {
        UI_LineClears.text = ("Lines : " + totalLinesCleared.ToString());
    }
    #endregion
}
