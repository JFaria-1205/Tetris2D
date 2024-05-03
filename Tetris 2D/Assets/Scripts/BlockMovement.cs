using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{

    [SerializeField] LayerMask bottomBoundsLayerMask;
    [SerializeField] LayerMask rightBoundsLayerMask;
    [SerializeField] LayerMask leftBoundsLayerMask;

    private Transform objectTransform;
    private float moveDistance = 0.5f;
    private float rotateSpeed = 90f;
    [SerializeField] private bool canRotate = true;
    private bool canMoveDown = true;
    private bool blockActive = true;
    private float moveSpeed;

    private void Start()
    {
        objectTransform = this.transform;
        moveSpeed = FindObjectOfType<GameManager>().currentSpeed;
    }

    public void StartAutoMoving()
    {
        StartCoroutine(AutoMoveDown());
    }

    IEnumerator AutoMoveDown()
    {
        yield return new WaitForSeconds(moveSpeed);
        while (blockActive)
        {
            yield return new WaitForSeconds(moveSpeed);
            CanBlockMoveDown();

            if (canMoveDown)
            {
                //Debug.Log("Block move down");
                objectTransform.position += new Vector3(0, -moveDistance, 0);
            }
            else
            {
                //Debug.Log("Block cannot move down");
                blockActive = false;
            }
        }
        Lock();
    }

    private void Lock()
    {
        StopAllCoroutines();

        Array childrenTransforms = GetComponentsInChildren<Transform>();

        foreach (Transform childTransform in childrenTransforms)
        {
            childTransform.gameObject.layer = LayerMask.NameToLayer("BottomBound");
        }

        FindObjectOfType<GameManager>().BlockLocked();
    }

    private void CanBlockMoveDown()
    {
        Array childrenTransforms = GetComponentsInChildren<Transform>();

        foreach (Transform childTransform in childrenTransforms)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(childTransform.position, Vector2.down, 0.5f, bottomBoundsLayerMask);
            if (raycastHit2D.collider != null)
            {
                Debug.Log("Block cannot move down");
                canMoveDown = false; //block cannot move downwards because something is blocking it
                break;
            }
            else
            {
                Debug.Log("Block can move down");
                canMoveDown = true;
            }
        }
    }

    public void RotateBlock(bool rotateLeft)
    {
        if (canRotate)
        {
            if (rotateLeft)
            {
                objectTransform.Rotate(new Vector3(0, 0, rotateSpeed), Space.Self);
            }
            else
            {
                objectTransform.Rotate(new Vector3(0, 0, -rotateSpeed), Space.Self);
            }
            CanBlockMoveDown();
        }
        
    }
}
