using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Tetromino : ScriptableObject
{
    [SerializeField] private String tetrominoLetterNickname;
    [SerializeField] private GameObject tetrominoPrefab;
    [SerializeField] private Texture tetrominoUiImage;

    public String GetTetrominoName()
    {
        return tetrominoLetterNickname;
    }

    public GameObject GetTetrominoPrefab()
    {
        return tetrominoPrefab;
    }

    public Texture GetTetrominoUiImage()
    {
        return tetrominoUiImage;
    }
}
