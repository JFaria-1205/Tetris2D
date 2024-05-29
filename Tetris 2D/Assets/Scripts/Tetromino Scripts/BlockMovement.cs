using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    [SerializeField] private bool canRotate = true;
    [SerializeField] LayerMask bounds;
    private Transform blockTransform;
    private readonly float moveDistance = 0.5f;
    private readonly float rotateDistance = -90f;
    private bool pauseRotations = false;

    //Gravity values (drop speed)
    private float gravity;
    private float currentGravityValue;
    private float gravityIncreased = 2.36f;
    private float gravityTimer = 0;
    private float waitTimer = 0.01667f;

    private bool blockActive = true;
    private Array childrenTransforms;

    private void Start()
    {
        blockTransform = this.transform;
        currentGravityValue = FindObjectOfType<GameManager>().currentGravity;
        childrenTransforms = GetComponentsInChildren<Transform>();
        gravity = currentGravityValue;
    }

    public void InitializeBlock()
    {
        StartCoroutine(AutoMoveDown());
    }

    
    IEnumerator AutoMoveDown()
    {
        yield return new WaitForSeconds(waitTimer);
        while (blockActive)
        {
            yield return new WaitForSeconds(waitTimer);
            gravityTimer += gravity;

            if (gravityTimer >= 1)
            {
                gravityTimer = 0;

                if (!MoveDown())
                    blockActive = false;
            }            
        }
        yield return new WaitForSeconds(0.25f);
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
            if (childrenToIgnore == null) //move down normal
            {
                foreach (Transform childTransform in childrenTransforms)
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
            else //ignore children in list and move down
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
            if (childrenToIgnore == null) //move up normal
            {
                foreach (Transform childTransform in childrenTransforms)
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
            else //ignore children in list and move down
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
            if (childrenToIgnore == null) //move right normal
            {
                foreach (Transform childTransform in childrenTransforms)
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
            else //ignore children in list and move down
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
            if (childrenToIgnore == null) //move left normal
            {
                foreach (Transform childTransform in childrenTransforms)
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
            else //ignore children in list and move down
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
            
        }        

        if (canMoveLeft)
            blockTransform.position += new Vector3(-moveDistance, 0, 0);

        return canMoveLeft;
    }

    public void IncreaseBlockSpeed(bool increase)
    {
        if (increase)
            gravity = gravityIncreased;
        else
            gravity = currentGravityValue;
    }

    #endregion

    #region Block Rotation
    public void RotateBlock(bool rotateRight)
    {
        Transform originalPosition = blockTransform;

        if (canRotate && blockActive && !pauseRotations)
        {
            pauseRotations = true;

            if (rotateRight)
                blockTransform.Rotate(new Vector3(0, 0, rotateDistance), Space.Self);
            else
                blockTransform.Rotate(new Vector3(0, 0, -rotateDistance), Space.Self);

            int rotationChecks = 0;
            bool rotationAllowed = false;
            while (rotationChecks < 2)
            {
                if (GetChildrenOutOfBounds() == null)
                {
                    rotationAllowed = true;
                    break;
                }                    
                else if (TryMovingToFixRotation())
                {
                    rotationAllowed = true;
                    break;
                }

                rotationChecks++;
                DoAlternateRotation(rotationChecks, rotateRight);
            }

            if (!rotationAllowed)
                blockTransform = originalPosition;

            pauseRotations = false;
        }        
    }

    private bool TryMovingToFixRotation()
    {
        Transform preMovePosition = blockTransform;

        List<Transform> childrenOutOfBounds = GetChildrenOutOfBounds();        

        foreach (Transform currentChild in childrenOutOfBounds)
        {
            bool moveSuccessful = false;

            int moveIntoBoundsDirectionsAvailable = 0;
            if (currentChild.position.x != blockTransform.position.x)
                moveIntoBoundsDirectionsAvailable++;
            if (currentChild.position.y < blockTransform.position.y)
                moveIntoBoundsDirectionsAvailable++;

            if (Physics2D.BoxCast(currentChild.position, new Vector2(0.25f, 0.25f), 0f, Vector2.zero, 0f, bounds).collider != null) //if child is out of bounds
            {
                if (currentChild.position.x > blockTransform.position.x)  //if child out of bound's x pos is to the RIGHT of parent
                {
                    int checks = 0;
                    while (checks < 2)
                    {
                        if (MoveLeft(childrenOutOfBounds))//move to the left - if the block can't then the position wasn't able to be fixed = try alt rotation/undo rotation
                        {
                            Debug.Log("Move Left");
                            if (Physics2D.BoxCast(currentChild.position, new Vector2(0.25f, 0.25f), 0f, Vector2.zero, 0f, bounds).collider != null) //child is still out of bounds
                            {
                                if (moveIntoBoundsDirectionsAvailable > 1) //if other direction moves available, undo move, otherwise, just loop and move again
                                {
                                    blockTransform = preMovePosition;
                                    break;
                                }
                            }
                            else //child position fixed
                            {
                                moveSuccessful = true;
                                break;
                            }
                        }
                        checks++;
                    }
                }
                else if (currentChild.position.x < blockTransform.position.x) //if child out of bound's x pos is to the LEFT of parent  
                {
                    int checks = 0;
                    while (checks < 2)
                    {
                        if (MoveRight(childrenOutOfBounds))//move to the right - if the block can't then the position wasn't able to be fixed = try alt rotation/undo rotation
                        {
                            Debug.Log("Move Right");
                            if (Physics2D.BoxCast(currentChild.position, new Vector2(0.25f, 0.25f), 0f, Vector2.zero, 0f, bounds).collider != null) //child is still out of bounds
                            {
                                if (moveIntoBoundsDirectionsAvailable > 1) //if other direction moves available, undo move, otherwise, just loop and move again
                                {
                                    blockTransform = preMovePosition;
                                    break;
                                }
                            }
                            else //child position fixed
                            {
                                moveSuccessful = true;
                                break;
                            }
                        }
                        checks++;
                    }
                }
                if (currentChild.position.y < blockTransform.position.y) //if child out of bound's y pos is ABOVE the parent
                {
                    int checks = 0;
                    while (checks < 2)
                    {
                        if (MoveUp(childrenOutOfBounds))//move up - if the block can't then the position wasn't able to be fixed = try alt rotation/undo rotation
                        {
                            Debug.Log("Move Up");
                            if (Physics2D.BoxCast(currentChild.position, new Vector2(0.25f, 0.25f), 0f, Vector2.zero, 0f, bounds).collider != null) //child is still out of bounds
                            {
                                if (moveIntoBoundsDirectionsAvailable > 1) //if other direction moves available, undo move, otherwise, just loop and move again
                                {
                                    blockTransform = preMovePosition;
                                    break;
                                }
                            }
                            else //child position fixed
                            {
                                moveSuccessful = true;
                                break;
                            }
                        }
                        checks++;
                    }
                }
            }
            else
                moveSuccessful = true;

            if (!moveSuccessful)
            {
                blockTransform = preMovePosition;
                return false;
            }                
        }

        return true;

    }

    private void DoAlternateRotation(int rotationCount, bool initialRotation)
    {
        switch (rotationCount)
        {
            default: //Debug.Log("Default alternate rotation (You shouldn't see this)");
                if (initialRotation)
                    blockTransform.Rotate(new Vector3(0, 0, rotateDistance), Space.Self);
                else
                    blockTransform.Rotate(new Vector3(0, 0, -rotateDistance), Space.Self);
                break;
            case 1: //check rotation of other direction (180 from current rotation)
                if (initialRotation)
                    blockTransform.Rotate(new Vector3(0, 0, rotateDistance * 2), Space.Self);
                else
                    blockTransform.Rotate(new Vector3(0, 0, -rotateDistance * 2), Space.Self);
                break;
        }
    }

    #endregion

    private List<Transform> GetChildrenOutOfBounds()
    {
        List<Transform> childrenOutOfBounds = new List<Transform>();

        foreach (Transform childTransform in childrenTransforms)
            if (Physics2D.BoxCast(childTransform.position, new Vector2(0.25f, 0.25f), 0f, Vector2.zero, 0f, bounds).collider != null) //if boxcast hit something (child is out of bounds)
                childrenOutOfBounds.Add(childTransform);

        if (childrenOutOfBounds.Count <= 0)
            return null;
        else
            return childrenOutOfBounds;
    }

}
