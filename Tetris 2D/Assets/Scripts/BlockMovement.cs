using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    [SerializeField] private readonly bool canRotate = true;
    [SerializeField] LayerMask bounds;
    private Transform blockTransform;
    private readonly float moveDistance = 0.5f;
    private readonly float rotateDistance = 90f;
    private bool pauseRotations = false;
    private float moveSpeed;
    private float moveSpeedIncreaseMultiplier = 1;
    private bool blockActive = true;
    private Array childrenTransforms;

    private void Start()
    {
        blockTransform = this.transform;
        moveSpeed = FindObjectOfType<GameManager>().currentSpeed;
        childrenTransforms = GetComponentsInChildren<Transform>();
    }

    public void InitializeBlock()
    {
        StartCoroutine(AutoMoveDown());
    }

    IEnumerator AutoMoveDown()
    {
        yield return new WaitForSeconds(moveSpeed * moveSpeedIncreaseMultiplier);
        while (blockActive)
        {
            yield return new WaitForSeconds(moveSpeed * moveSpeedIncreaseMultiplier);

            if (!MoveDown())
                blockActive = false;
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

    #region Block Movements

    private bool MoveDown(List<Transform> childrenToIgnore = null)
    {
        bool canMoveDown = false;

        if (blockActive)
        {
            foreach (Transform childTransform in childrenTransforms)
            {
                if (!childrenToIgnore.Contains(childTransform))
                {
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(childTransform.position, Vector2.down, 0.5f, bounds);

                    if (raycastHit2D.collider != null)
                    {
                        canMoveDown = false; //block cannot move down because something is blocking it
                        break;
                    }
                    else
                        canMoveDown = true;
                }
            }
        }        

        if (canMoveDown)
            blockTransform.position += new Vector3(0, -moveDistance, 0);

        return canMoveDown;
    }

    private bool MoveUp(List<Transform> childrenToIgnore = null)
    {
        bool canMoveUp = false;

        if (blockActive)
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
                    else
                        canMoveUp = true;
                }
            }
        }        

        if (canMoveUp)
            blockTransform.position += new Vector3(0, moveDistance, 0);

        return canMoveUp;
    }

    public bool MoveRight(List<Transform> childrenToIgnore = null)
    {
        bool canMoveRight = false;

        if (blockActive)
        {
            foreach (Transform childTransform in childrenTransforms)
            {
                if (!childrenToIgnore.Contains(childTransform))
                {
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(childTransform.position, Vector2.right, 0.5f, bounds);

                    if (raycastHit2D.collider != null)
                    {
                        canMoveRight = false; //block cannot move right because something is blocking it
                        break;
                    }
                    else
                        canMoveRight = true;
                }
            }
        }        

        if (canMoveRight)
            blockTransform.position += new Vector3(moveDistance, 0, 0);

        return canMoveRight;
    }    

    public bool MoveLeft(List<Transform> childrenToIgnore = null)
    {
        bool canMoveLeft = false;

        if (blockActive)
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
                    else
                        canMoveLeft = true;
                }
            }
        }        

        if (canMoveLeft)
            blockTransform.position += new Vector3(-moveDistance, 0, 0);

        return canMoveLeft;
    }

    #endregion


    public void RotateBlock(bool rotateLeft)
    {
        Transform originalPosition = blockTransform;

        if (canRotate && blockActive && !pauseRotations)
        {
            pauseRotations = true;

            if (rotateLeft)
                blockTransform.Rotate(new Vector3(0, 0, rotateDistance), Space.Self);
            else
                blockTransform.Rotate(new Vector3(0, 0, -rotateDistance), Space.Self);

            if (!TryFixRotationErrors(rotateLeft)) //If rotation errors weren't able to be fixed, go back to original position
                blockTransform = originalPosition;
            
            pauseRotations = false;
        }        
    }

    private bool TryFixRotationErrors(bool originalRotateLeft)
    {        
        Transform originalPosition = blockTransform;

        bool tryFixRotationErrors = true;
        List<Transform> childrenOutOfBounds = new List<Transform>();
        

        bool fixingPosition = true;

        while (tryFixRotationErrors)
        {
            childrenOutOfBounds.Clear();

            //Check if any children are out of bounds, and if so add them to the list of children out of bounds
            foreach (Transform childTransform in childrenTransforms)
            {
                RaycastHit2D boxcastHit2D = Physics2D.BoxCast(childTransform.position, new Vector2(0.25f, 0.25f), 0f, Vector2.zero, 0f, bounds);

                if (boxcastHit2D.collider != null) //if boxcast hit something (child is out of bounds)
                {
                    childrenOutOfBounds.Add(childTransform);
                    //Debug.Log("Rotation hit: " + childTransform.position);
                }
            }

            if (childrenOutOfBounds.Count <= 0) //if nothing is out of bounds, no rotation errors => return true
                return true;

            //childrenOutOfBounds.FirstOrDefault().transform.position
            //get any child and try to move, if cannot then rotate?
        }

        //Debug.Log("See if position needs to be fixed");
        bool positionNeedsChecking = true;
        int rotationsChecked = 0;
        while (positionNeedsChecking && rotationsChecked < 3)
        {
            if (/*original: !TryFixRotationErrors() */ positionNeedsChecking)
            {
                //Debug.Log("Position not fixed - do alternated rotation");
                switch (rotationsChecked)
                {
                    default: //Debug.Log("Default alternate rotation (You shouldn't see this)");
                        if (originalRotateLeft)
                            blockTransform.Rotate(new Vector3(0, 0, rotateDistance), Space.Self);
                        else
                            blockTransform.Rotate(new Vector3(0, 0, -rotateDistance), Space.Self);
                        break;
                    case 0: //check rotation of other direction
                        if (originalRotateLeft)
                            blockTransform.Rotate(new Vector3(0, 0, rotateDistance * 2), Space.Self);
                        else
                            blockTransform.Rotate(new Vector3(0, 0, -rotateDistance * 2), Space.Self);
                        Debug.Log("Try rotate oposite direction");
                        break;
                    case 1: //original rotation + 1 (back one)
                        if (originalRotateLeft)
                            blockTransform.Rotate(new Vector3(0, 0, -rotateDistance), Space.Self);
                        else
                            blockTransform.Rotate(new Vector3(0, 0, rotateDistance), Space.Self);
                        Debug.Log("Try rotate original + 1 direction");
                        break;
                    case 2: //rotate unavailable - rotate back to original
                        Debug.Log("Rotation unavailable - going back to original rotation");
                        blockTransform = originalPosition;
                        positionNeedsChecking = false;
                        break;
                }
            }
            else
            {
                positionNeedsChecking = false;
                Debug.Log("Position fixed / didn't need fixing");
                break;
            }

            rotationsChecked++;
        }

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
                    Debug.Log("Rotation hit: " + childTransform.position);
                }
            }

            if (childrenOutOfBounds.Count > 0)
            {
                //for each child out of bounds, move block
                foreach (Transform childTransform in childrenOutOfBounds)
                {
                    if (childTransform.position.x > blockTransform.position.x) //if child out of bound's x pos is to the RIGHT of parent
                    {
                        Debug.Log("Try move left to fix position");
                        //move to the left - if the block can't then the position wasn't able to be fixed = rotate back
                        if (!MoveLeft(childrenOutOfBounds))
                        {
                            Debug.Log("Move left didnt work");
                            blockTransform = originalPosition;
                            return false;
                        }
                        Debug.Log("Move left worked");
                    }
                    else if (childTransform.position.x < blockTransform.position.x) //if child out of bound's x pos is to the LEFT of parent
                    {
                        Debug.Log("Try move right to fix position");
                        //move to the right - if the block can't then the position wasn't able to be fixed = rotate back
                        if (!MoveRight(childrenOutOfBounds))
                        {
                            Debug.Log("Move right didnt work");
                            blockTransform = originalPosition;
                            return false;
                        }
                        Debug.Log("Move right worked");
                    }
                    else if (childTransform.position.y < blockTransform.position.y) //if child out of bound's y pos is BELOW the parent
                    {
                        Debug.Log("Try move up to fix position");
                        //move up - if the block can't then the position wasn't able to be fixed = rotate back
                        if (!MoveUp(childrenOutOfBounds))
                        {
                            Debug.Log("Move up didnt work");
                            blockTransform = originalPosition;
                            return false;
                        }
                        Debug.Log("Move up worked");
                    }
                    else //child out of bound's y pos is ABOVE the parent
                    {
                        Debug.Log("Try move down to fix position");
                        //move down - if the block can't then the position was able to be fixed = rotate back
                        if (!MoveDown(childrenOutOfBounds))
                        {
                            Debug.Log("Move down didnt work");
                            blockTransform = originalPosition;
                            return false;
                        }
                        Debug.Log("Move down worked");
                    }
                }

                foreach (Transform childTransform in childrenOutOfBounds)
                {
                    if (childTransform.position.x > 2) //out of bounds right
                    {
                        //move left
                    }
                    else if (childTransform.position.x < -2.5) //out of bounds left
                    {
                        //move right
                    }
                    else if (childTransform.position.y < -5) //out of bounds bottom
                    {
                        //move up
                    }
                }
            }
            else
                fixingPosition = false;
        }

        return true;

    }

}
