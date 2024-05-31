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
    [SerializeField] GameObject tetrominoPrefab;
    [SerializeField] Image tetrominoUiImage;
}
