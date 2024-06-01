using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraShake
{
    public static ShakeComponent shakeComponent;

    static CameraShake()
    {
        shakeComponent = GetCameraShakeComponent();
    }

    private static ShakeComponent GetCameraShakeComponent()
    {
        ShakeComponent component = GameObject.Find("Main Camera").GetComponent<ShakeComponent>();
        if (component == null) Debug.LogError("Camera shake component not found in scene.");
        return component;
    }

    public static void Shake(float shake_Duration, float shake_Roughness, float x_Magnitude, float y_Magnitude, float z_Magnitude = 0)
    {
        if (shakeComponent == null)
            shakeComponent = GetCameraShakeComponent();
        shakeComponent.Shake(shake_Duration, shake_Roughness, x_Magnitude, y_Magnitude, z_Magnitude);
    }
}
