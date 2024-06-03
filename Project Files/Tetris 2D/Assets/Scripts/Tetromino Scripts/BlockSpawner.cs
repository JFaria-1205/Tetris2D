using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] List<Tetromino> blockTypes = new List<Tetromino>();
    private Vector3 initialSpawnPoint = new Vector3(0, 4.5f, 0);
    public List<GameObject> spawnBag = new List<GameObject>();
    private GameObject nextBlockToSpawn;
    [SerializeField] LayerMask bounds;

    private void Awake()
    {
        RefreshSpawnBag();
    }

    public void SpawnBlock(out GameObject currentBlock, out Texture nextBlockUiImage, out bool gameOver)
    {
        gameOver = false;

        if (nextBlockToSpawn == null)
            nextBlockToSpawn = GetBlock();

        GameObject spawnedBlock = Instantiate(nextBlockToSpawn, initialSpawnPoint, Quaternion.identity);

        bool checkSpawn = true;
        while (checkSpawn)
        {
            CheckBlockSpawnPoint(spawnedBlock, out checkSpawn);
        }

        currentBlock = spawnedBlock;
        nextBlockToSpawn = GetBlock();
        nextBlockUiImage = GetNextBlockUiImage(nextBlockToSpawn);

        if (CheckGameOver(spawnedBlock))
            gameOver = true;
        else if (!FindObjectOfType<PauseMenu>().IsGamePaused())
            spawnedBlock.GetComponent<BlockMovement>().InitializeBlock();

        
    }

    private void CheckBlockSpawnPoint(GameObject blockToCheck, out bool checkAgain)
    {
        checkAgain = false;

        Array childrenTransforms = blockToCheck.GetComponentsInChildren<Transform>();

        foreach (Transform childTransform in childrenTransforms)
        {
            if (childTransform.position.y > 4.5f)
            {
                blockToCheck.transform.position += new Vector3(0, -0.5f, 0);
                checkAgain = true;
            }
        }
    }

    private Texture GetNextBlockUiImage(GameObject nextBlock)
    {
        foreach (Tetromino tetromino in blockTypes)
        {
            if (tetromino.GetTetrominoPrefab() == nextBlock)
            {
                return tetromino.GetTetrominoUiImage();
            }
        }

        return null;
    }

    private GameObject GetBlock()
    {
        if (spawnBag.Count <= 0 || spawnBag == null)
        {
            RefreshSpawnBag();
        }

        GameObject block = spawnBag[UnityEngine.Random.Range(0, spawnBag.Count)];
        spawnBag.Remove(block);
        return block;
    }

    private void RefreshSpawnBag()
    {
        spawnBag.Clear();

        
        foreach (Tetromino tetromino in blockTypes)
        {
            spawnBag.Add(tetromino.GetTetrominoPrefab());
        }
    }

    private bool CheckGameOver(GameObject currBlock)
    {
        Array childrenTransforms = currBlock.GetComponentsInChildren<Transform>();

        foreach (Transform childTransform in childrenTransforms)
        {
            if (Physics2D.BoxCast(childTransform.position, new Vector2(0.25f, 0.25f), 0f, Vector2.zero, 0f, bounds).collider != null) //if child is out of bounds
            {
                return true;
            }
        }

        return false;
    }
}
