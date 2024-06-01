using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeComponent : MonoBehaviour
{
    Vector3 cameraOriginalPosition;

    private void Start()
    {
        cameraOriginalPosition = this.transform.localPosition;
    }

    public void Shake(float duration, float roughness, float x, float y, float z)
    {
        StartCoroutine(DoShake(duration, roughness, x, y, z));
    }

    private IEnumerator DoShake(float duration, float roughness, float x, float y, float z)
    {
        float elapsed = 0.0f;
        float interval = duration / roughness;
        Vector3 targetPosition;

        while (elapsed < duration)
        {
            float xShake = cameraOriginalPosition.x + Random.Range(-x, x);
            float yShake = cameraOriginalPosition.y + Random.Range(-y, y);
            float zShake = cameraOriginalPosition.z + Random.Range(-z, z);

            targetPosition = new Vector3(xShake, yShake, zShake);

            iTween.MoveTo(this.gameObject, iTween.Hash("position", targetPosition, "time", interval, "easetype", iTween.EaseType.easeInOutQuart));

            yield return new WaitForSeconds(interval);

            elapsed += interval;
        }

        iTween.MoveTo(this.gameObject, iTween.Hash("position", cameraOriginalPosition, "time", interval, "easetype", iTween.EaseType.easeInOutQuart));
    }
}
