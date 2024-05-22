using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockClearingTracker : MonoBehaviour
{
    private int childCount = 4;

    public void RemoveChild(GameObject childToRemove)
    {
        Destroy(childToRemove);

        childCount--;

        if (childCount <= 0)
        {
            Debug.Log("All children removed from " + this.gameObject.name + ": Deleting object...");
            Destroy(this.gameObject);
        }
    }
}
