using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    [SerializeField] private bool canRotate = true;
    [SerializeField] LayerMask bounds;
    private Transform objectTransform;
    private float moveDistance = 0.5f;
    private float rotateSpeed = 90f;
    private bool canMoveDown = true;
    private bool blockActive = true;
    private float moveSpeed;
    private Array childrenTransforms;

    private void Start()
    {
        objectTransform = this.transform;
        moveSpeed = FindObjectOfType<GameManager>().currentSpeed;

        childrenTransforms = GetComponentsInChildren<Transform>();
    }

    public void StartAutoMovingDown()
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
                objectTransform.position += new Vector3(0, -moveDistance, 0);
            }
            else
            {
                blockActive = false;
            }
        }
        Lock();
    }

    private void Lock()
    {
        StopAllCoroutines();

        foreach (Transform childTransform in childrenTransforms)
        {
            childTransform.gameObject.layer = LayerMask.NameToLayer("Bounds");
        }

        FindObjectOfType<GameManager>().BlockLocked();
    }

    private void CanBlockMoveDown()
    {
        foreach (Transform childTransform in childrenTransforms)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(childTransform.position, Vector2.down, 0.5f, bounds);

            if (raycastHit2D.collider != null)
            {
                canMoveDown = false; //block cannot move downwards because something is blocking it
                break;
            }
            else
            {
                canMoveDown = true;
            }
        }
    }

    public bool MoveRight(List<Transform> childrenToIgnore = null)
    {
        bool canMoveRight = true;

        foreach (Transform childTransform in childrenTransforms)
        {
            if (childrenToIgnore != null)
            {
                if (!childrenToIgnore.Contains(childTransform))
                {
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(childTransform.position, Vector2.right, 0.5f, bounds);
                    if (raycastHit2D.collider != null)
                    {
                        canMoveRight = false;
                        break;
                    }

                } //else skip child check
            }
            else
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(childTransform.position, Vector2.right, 0.5f, bounds);
                if (raycastHit2D.collider != null)
                {
                    canMoveRight = false;
                    break;
                }
            }
        }

        if (canMoveRight)
            objectTransform.position += new Vector3(moveDistance, 0, 0);

        return canMoveRight;
    }

    public bool MoveLeft(List<Transform> childrenToIgnore = null)
    {
        bool canMoveLeft = true;

        foreach (Transform childTransform in childrenTransforms)
        {
            if (childrenToIgnore != null)
            {
                if (!childrenToIgnore.Contains(childTransform))
                {
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(childTransform.position, Vector2.left, 0.5f, bounds);
                    if (raycastHit2D.collider != null)
                    {
                        canMoveLeft = false;
                        break;
                    }

                } //else skip child check
            }
            else
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(childTransform.position, Vector2.left, 0.5f, bounds);
                if (raycastHit2D.collider != null)
                {
                    canMoveLeft = false;
                    break;
                }
            }
        }

        if (canMoveLeft)
            objectTransform.position += new Vector3(-moveDistance, 0, 0);

        return canMoveLeft;
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

            FixBlockPosition();

            /*
            if(!FixBlockPosition()) //if position was unable to be fixed, try rotate other way
            {
                Debug.Log("Cannot rotate, rotating back");
                if (rotateLeft)
                {
                    objectTransform.Rotate(new Vector3(0, 0, -rotateSpeed), Space.Self);
                }
                else
                {
                    objectTransform.Rotate(new Vector3(0, 0, rotateSpeed), Space.Self);
                }
            }
            */

            CanBlockMoveDown();
        }  
    }

    private bool FixBlockPosition()
    {
        bool fixingPosition = true;
        List<Transform> childrenOutOfBounds = new List<Transform>();

        while (fixingPosition)
        {
            foreach (Transform childTransform in childrenTransforms)
            {
                RaycastHit2D boxcastHit2D = Physics2D.BoxCast(childTransform.position, new Vector2(0.25f, 0.25f), 0f, Vector2.down, 0.25f, bounds);

                if (boxcastHit2D.collider != null)
                {
                    childrenOutOfBounds.Add(childTransform);

                    if (childTransform.position.x > objectTransform.position.x) //if child that hit the cast's x pos is to the RIGHT of parent
                    {
                        //move to the left
                        if (!MoveLeft(childrenOutOfBounds))
                            return false;
                    }
                    else //if child that hit the cast's x pos is to the LEFT of parent
                    {
                        //move to the right
                        if (!MoveRight(childrenOutOfBounds))
                            return false;
                    }
                    break;
                }
            }
            fixingPosition = false;
        }

        return true;

    }

}
