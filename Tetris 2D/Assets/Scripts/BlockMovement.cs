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
    private float rotateDistance = 90f;
    private float moveSpeed;
    private bool blockCanMoveDown = true;
    private bool blockActive = true;
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

            if (blockCanMoveDown)
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
                blockCanMoveDown = false; //block cannot move downwards because something is blocking it
                break;
            }
            else
            {
                blockCanMoveDown = true;
            }
        }
    }

    public bool MoveRight(List<Transform> childrenToIgnore = null)
    {
        bool canMoveRight = true;

        if (childrenToIgnore == null) //regular move
        {
            foreach (Transform childTransform in childrenTransforms)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(childTransform.position, Vector2.right, 0.5f, bounds);
                if (raycastHit2D.collider != null)
                {
                    canMoveRight = false;
                    break;
                }
            }
        }
        else //moving rotated piece
        {
            foreach (Transform childTransform in childrenTransforms)
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
        }

        if (canMoveRight)
        {
            objectTransform.position += new Vector3(moveDistance, 0, 0);
            CanBlockMoveDown();
        }

        return canMoveRight;
    }

    public bool MoveLeft(List<Transform> childrenToIgnore = null)
    {
        bool canMoveLeft = true;

        if (childrenToIgnore == null) //regular move
        {
            foreach (Transform childTransform in childrenTransforms)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(childTransform.position, Vector2.left, 0.5f, bounds);
                if (raycastHit2D.collider != null)
                {
                    canMoveLeft = false;
                    break;
                }
            }
        }
        else //moving rotated piece
        {
            foreach (Transform childTransform in childrenTransforms)
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
        }

        if (canMoveLeft)
        {
            objectTransform.position += new Vector3(-moveDistance, 0, 0);
            CanBlockMoveDown();
        }  

        return canMoveLeft;
    }

    private bool MoveDown (List<Transform> childrenToIgnore) //no optional parameter becuase this is just for rotation fix so there has to be a list of children to ignore
    {
        bool canMoveDown = true;

        if (childrenToIgnore != null)
        {
            foreach (Transform childTransform in childrenTransforms)
            {
                if (!childrenToIgnore.Contains(childTransform))
                {
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(childTransform.position, Vector2.down, 0.5f, bounds);
                    if (raycastHit2D.collider != null)
                    {
                        canMoveDown = false;
                        break;
                    }
                }
            }
        }
        else
            Debug.Log("ERROR: Function 'MoveDown()' in 'BlockMovement.cs' requires a valid list of children to ignore.");

        if (canMoveDown)
            objectTransform.position += new Vector3(0, -moveDistance, 0);

        return canMoveDown;
    }

    private bool MoveUp(List<Transform> childrenToIgnore) //no optional parameter becuase this is just for rotation fix so there has to be a list of children to ignore
    {
        bool canMoveUp = true;

        if (childrenToIgnore != null)
        {
            foreach (Transform childTransform in childrenTransforms)
            {
                if (!childrenToIgnore.Contains(childTransform))
                {
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(childTransform.position, Vector2.up, 0.5f, bounds);
                    if (raycastHit2D.collider != null)
                    {
                        canMoveUp = false;
                        break;
                    }
                }
            }
        }
        else
            Debug.Log("ERROR: Function 'MoveUp()' in 'BlockMovement.cs' requires a valid list of children to ignore.");

        if (canMoveUp)
            objectTransform.position += new Vector3(0, moveDistance, 0);

        return canMoveUp;
    }

    public void RotateBlock(bool rotateLeft)
    {
        Transform originalTransform = objectTransform;

        if (canRotate)
        {
            canRotate = false;

            if (rotateLeft)
                objectTransform.Rotate(new Vector3(0, 0, rotateDistance), Space.Self);
            else
                objectTransform.Rotate(new Vector3(0, 0, -rotateDistance), Space.Self);


            bool positionNeedsFixing = true;
            int rotationsChecked = 0;
            while (positionNeedsFixing || rotationsChecked < 3)
            {
                if (!FixBlockPosition())
                {
                    switch (rotationsChecked)
                    {
                        default: //rotate same direction as original rotation
                            if (rotateLeft)
                                objectTransform.Rotate(new Vector3(0, 0, rotateDistance), Space.Self);
                            else
                                objectTransform.Rotate(new Vector3(0, 0, -rotateDistance), Space.Self);
                            break;
                        case 0: //check rotation of other direction
                            if (rotateLeft)
                                objectTransform.Rotate(new Vector3(0, 0, rotateDistance*2), Space.Self);
                            else
                                objectTransform.Rotate(new Vector3(0, 0, -rotateDistance*2), Space.Self);
                            break;
                        case 1: //original rotation + 1 (back one)
                            if (rotateLeft)
                                objectTransform.Rotate(new Vector3(0, 0, -rotateDistance), Space.Self);
                            else
                                objectTransform.Rotate(new Vector3(0, 0, rotateDistance), Space.Self);
                            break;
                        case 2: //rotate unavailable - rotate back to original
                            objectTransform = originalTransform;
                            positionNeedsFixing = false;
                            break;
                    }
                }
                else
                    positionNeedsFixing = false;

                rotationsChecked++;
            }

            CanBlockMoveDown();
        }

        canRotate = true;
    }

    private bool FixBlockPosition()
    {
        bool fixingPosition = true;
        Transform originalPosition = objectTransform;
        List<Transform> childrenOutOfBounds = new List<Transform>();

        while (fixingPosition)
        {
            childrenOutOfBounds.Clear();

            //get list of children out of bounds
            foreach (Transform childTransform in childrenTransforms)
            {
                RaycastHit2D boxcastHit2D = Physics2D.BoxCast(childTransform.position, new Vector2(0.25f, 0.25f), 0f, Vector2.down, 0.25f, bounds);

                if (boxcastHit2D.collider != null)
                {
                    childrenOutOfBounds.Add(childTransform);
                }
            }

            if (childrenOutOfBounds.Count > 0)
            {
                //for each child out of bounds, move block
                foreach (Transform childTransform in childrenOutOfBounds)
                {
                    if (childTransform.position.x > objectTransform.position.x) //if child out of bound's x pos is to the RIGHT of parent
                    {
                        //move to the left - if the block can't then the position wasn't able to be fixed = rotate back
                        if (!MoveLeft(childrenOutOfBounds))
                        {
                            objectTransform = originalPosition;
                            return false;
                        }
                    }
                    else if (childTransform.position.x < objectTransform.position.x) //if child out of bound's x pos is to the LEFT of parent
                    {
                        //move to the right - if the block can't then the position wasn't able to be fixed = rotate back
                        if (!MoveRight(childrenOutOfBounds))
                        {
                            objectTransform = originalPosition;
                            return false;
                        }
                    }
                    else if (childTransform.position.y < objectTransform.position.y) //if child out of bound's y pos is BELOW the parent
                    {
                        //move up - if the block can't then the position wasn't able to be fixed = rotate back
                        if (!MoveUp(childrenOutOfBounds))
                        {
                            objectTransform = originalPosition;
                            return false;
                        }
                    }
                    else //child out of bound's y pos is ABOVE the parent
                    {
                        //move down - if the block can't then the position was able to be fixed = rotate back
                        if (!MoveDown(childrenOutOfBounds))
                        {
                            objectTransform = originalPosition;
                            return false;
                        }
                    }
                }
            }
            else
                fixingPosition = false;
        }

        return true;

    }

}
