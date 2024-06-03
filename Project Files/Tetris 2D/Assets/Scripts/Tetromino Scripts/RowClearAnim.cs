using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowClearAnim : MonoBehaviour
{
    Transform blockTransform;
    Vector3 targetPointFirstMovement;
    float waitTimeFirst = 0.1f;

    Vector3 targetPointSecondMovement;
    float waitTimeFinal = 0.35f;

    float randRotation;
    float randMovement;

    public void PlayClearAnim()
    {
        blockTransform = this.transform;

        randMovement = Random.Range(-0.4f, 0.4f);
        randRotation = Random.Range(-0.4f, 0.4f);

        targetPointFirstMovement = new Vector3(blockTransform.position.x+randMovement, blockTransform.position.y+0.25f, blockTransform.position.z);
        targetPointSecondMovement = new Vector3(blockTransform.position.x+(randMovement*2), -7f, blockTransform.position.z);

        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", targetPointFirstMovement, "time", waitTimeFirst, "easetype", iTween.EaseType.easeOutSine));
        iTween.RotateBy(this.gameObject, iTween.Hash("z", randRotation, "easeType", iTween.EaseType.spring));
        yield return new WaitForSeconds(waitTimeFirst);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", targetPointSecondMovement, "time", waitTimeFinal, "easetype", iTween.EaseType.easeInSine));
    }
    
}
