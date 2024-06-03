using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockClearingTracker : MonoBehaviour
{
    private int childCount = 4;


    public void RemoveChild(GameObject childToRemove)
    {
        childToRemove.GetComponent<RowClearAnim>().PlayClearAnim();
        //Destroy(childToRemove);

        childCount--;

        StartCoroutine(RemoveAfterAnim(childToRemove));
    }

    private IEnumerator RemoveAfterAnim(GameObject childToRemove)
    {
        yield return new WaitForSeconds(0.6f);

        Destroy(childToRemove);

        if (childCount <= 0)
        {
            Debug.Log("All children removed from " + this.gameObject.name + ": Deleting object...");
            Destroy(this.gameObject);
        }
    }
}
