using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowMoveAnim : MonoBehaviour
{
    private float moveTime = 0.5f;

    public void PlayMoveDownAnim(Vector3 newPosition)
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", newPosition, "time", moveTime, "easetype", iTween.EaseType.easeOutBounce));
        StartCoroutine(DoCameraShakeAfterMoveDown());
    }

    private IEnumerator DoCameraShakeAfterMoveDown()
    {
        yield return new WaitForSeconds(0.2f);
        CameraShake.Shake(0.3f, 5f, 0.05f, 0.1f);
    }
}
