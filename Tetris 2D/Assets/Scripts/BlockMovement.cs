using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    private Transform objectTransform;
    private float moveDistance = 0.5f;
    private bool canMove = true;
    private float moveSpeed;

    private void Start()
    {
        objectTransform = this.transform;
        moveSpeed = FindObjectOfType<GameManager>().currentSpeed;
    }

    private void Update()
    {
        /*
        if (Input.GetKey(KeyCode.D))
        {
            objectTransform.position += new Vector3(moveDistance * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            objectTransform.position += new Vector3(-moveDistance * Time.deltaTime, 0, 0);
        }
        */
    }

    public void StartAutoMoving()
    {
        StartCoroutine(AutoMoveDown());
    }

    IEnumerator AutoMoveDown()
    {
        yield return new WaitForSeconds(moveSpeed);
        while (canMove)
        {
            yield return new WaitForSeconds(moveSpeed);
            objectTransform.position += new Vector3(0, -moveDistance, 0);
        }
    }

    private void CheckIfBlockCanMove()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bounds"/* || collision.gameObject.tag == "Block"*/)
            Debug.Log("I am touching " + collision.gameObject.tag);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bounds")
            Debug.Log("I am no longer touching " + collision.gameObject.tag);
    }
}
