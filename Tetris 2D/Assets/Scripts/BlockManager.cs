using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlockManager : MonoBehaviour
{
    [SerializeField] List<GameObject> blockTypes = new List<GameObject>();
    private Vector3 initialSpawnPoint = new Vector3(0, 4.5f, 0);
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        SpawnBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnBlock()
    {
        GameObject currObject = Instantiate(blockTypes[Random.Range(0, blockTypes.Count-1)], initialSpawnPoint, Quaternion.identity);

        List<Transform> childrenTransforms = new List<Transform>();

        for (int i = 0; i < 4; i++)
        {
            childrenTransforms.Add(currObject.transform.GetChild(i));
        }

        foreach (Transform transform in childrenTransforms)
        {
            if (transform.position.y > 4.5f)
            {
                currObject.transform.position -= new Vector3(0, -0.5f, 0);
            }
        }

        currObject.GetComponent<AutoMove>().Invoke("StartMoving", gameManager.levelAndSpeed.GetValueOrDefault(gameManager.currentLevel));
    }
}
