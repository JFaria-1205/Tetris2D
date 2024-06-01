using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Tetromino : ScriptableObject
{
    public String tetrominoLetterNickname;
    public GameObject tetrominoPrefab;
    public Texture tetrominoUiImage;

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
