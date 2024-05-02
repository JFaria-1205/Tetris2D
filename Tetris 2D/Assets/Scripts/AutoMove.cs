using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    [SerializeField] Transform objectTransform;
    private Vector3 moveDistance = new Vector3(0, -0.5f, 0);
    private bool canMove = true;
    public float moveDelay;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        moveDelay = gameManager.levelAndSpeed.GetValueOrDefault(gameManager.currentLevel);
    }

    private void StartMoving()
    {
        StartCoroutine(MoveDown());
    }

    IEnumerator MoveDown()
    {
        while (canMove)
        {
            objectTransform.position += moveDistance;
            yield return new WaitForSeconds(moveDelay);
        }
    }
}
