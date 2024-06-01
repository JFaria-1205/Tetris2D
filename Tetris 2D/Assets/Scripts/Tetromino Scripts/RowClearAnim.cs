using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowClearAnim : MonoBehaviour
{
    Transform blockTransform;
    Vector3 targetPointFirst;
    float waitTimeFirst = 0.1f;

    Vector3 targetPointFinal;
    float waitTimeFinal = 0.35f;

    float randRotation;
    float randMovement;

    public void PlayClearAnim()
    {
        Debug.Log("Clear me!");
        blockTransform = this.transform;

        randMovement = Random.Range(-0.3f, 0.3f);
        randRotation = Random.Range(-0.3f, 0.3f);

        targetPointFirst = new Vector3(blockTransform.position.x+randMovement, blockTransform.position.y+0.25f, blockTransform.position.z);
        targetPointFinal = new Vector3(blockTransform.position.x+(randMovement*2), -7f, blockTransform.position.z);

        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", targetPointFirst, "time", waitTimeFirst, "easetype", iTween.EaseType.easeOutSine));
        iTween.RotateBy(this.gameObject, iTween.Hash("z", randRotation, "easeType", iTween.EaseType.spring));
        yield return new WaitForSeconds(waitTimeFirst);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", targetPointFinal, "time", waitTimeFinal, "easetype", iTween.EaseType.easeInSine));
    }
    
}
