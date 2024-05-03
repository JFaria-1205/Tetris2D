using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> blockTypes = new List<GameObject>();
    private Vector3 initialSpawnPoint = new Vector3(0, 4.5f, 0);

    public void SpawnBlock(out GameObject currentBlock)
    {
        GameObject spawnedBlock = Instantiate(blockTypes[Random.Range(0, blockTypes.Count)], initialSpawnPoint, Quaternion.identity);

        bool checkSpawn = true;
        while (checkSpawn)
        {
            CheckBlockSpawnPoint(spawnedBlock, out checkSpawn);
        }

        currentBlock = spawnedBlock;

        spawnedBlock.GetComponent<BlockMovement>().StartAutoMoving();
    }

    private void CheckBlockSpawnPoint(GameObject blockToCheck, out bool checkAgain)
    {
        checkAgain = false;

        List<Transform> childrenTransforms = new List<Transform>();

        for (int i = 0; i < 4; i++)
        {
            childrenTransforms.Add(blockToCheck.transform.GetChild(i));
        }

        foreach (Transform transform in childrenTransforms)
        {
            if (transform.position.y > 4.5f)
            {
                blockToCheck.transform.position += new Vector3(0, -0.5f, 0);
                checkAgain = true;
            }
        }
    }
}
