using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] public List<GameObject> blockTypes = new List<GameObject>();
    private Vector3 initialSpawnPoint = new Vector3(0, 4.5f, 0);
    private int nextBlockToSpawnIndex;

    private void Start()
    {
        nextBlockToSpawnIndex = UnityEngine.Random.Range(0, blockTypes.Count);
    }

    public void SpawnBlock(out GameObject currentBlock, out int nextBlockIndex)
    {
        GameObject spawnedBlock = Instantiate(blockTypes[nextBlockToSpawnIndex], initialSpawnPoint, Quaternion.identity);        

        bool checkSpawn = true;
        while (checkSpawn)
        {
            CheckBlockSpawnPoint(spawnedBlock, out checkSpawn);
        }

        currentBlock = spawnedBlock;
        nextBlockToSpawnIndex = UnityEngine.Random.Range(0, blockTypes.Count);
        nextBlockIndex = nextBlockToSpawnIndex;

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
}
