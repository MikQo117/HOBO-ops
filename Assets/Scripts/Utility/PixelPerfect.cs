using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made by Sipi Raussi 25.4.2018
[RequireComponent(typeof(Camera))]
public class PixelPerfect : MonoBehaviour
{
    private int pixelsPerUnit;

    private Camera c;

    private void SetCameraSize(int pixelsPerUnit)
    {
        c.orthographicSize = Screen.height * 0.5f / pixelsPerUnit;
    }

    private void Start()
    {
        c = GetComponent<Camera>();
        pixelsPerUnit = SettingsManager.Instance.Settings.PixelsPerUnit;
        SetCameraSize(pixelsPerUnit);
    }
}
