using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made by Sipi Raussi 25.4.2018
[RequireComponent(typeof(Camera))]
public class PixelPerfect : MonoBehaviour
{
    private int pixelsPerUnit;

    private void SetCameraSize(int pixelsPerUnit)
    {
        GetComponent<Camera>().orthographicSize = Screen.height * 0.5f / pixelsPerUnit;
    }

    private void LateUpdate()
    {
        if (SettingsManager.Instance.Settings)
        {
            pixelsPerUnit = SettingsManager.Instance.Settings.PixelsPerUnit;
            SetCameraSize(pixelsPerUnit);
            Destroy(this);
        }
    }
}
