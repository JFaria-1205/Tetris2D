using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentLevel = 1;
    public Dictionary<int, float> levelAndSpeed = new Dictionary<int, float>();

    private void Awake()
    {
        PopulateLevelAndSpeedDictionary();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PopulateLevelAndSpeedDictionary()
    {
        levelAndSpeed.Add(1, 1.5f);
        levelAndSpeed.Add(2, 1.5f);
        levelAndSpeed.Add(3, 1.5f);
        levelAndSpeed.Add(4, 1.5f);

    }
}
