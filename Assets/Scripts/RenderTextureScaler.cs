using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureScaler : MonoBehaviour
{
    public Camera Cam;

    void Update()
    {
        float verticalSize = Cam.orthographicSize * 2.0f;
        float horizontalSize = verticalSize * Screen.width / Screen.height;


        transform.localScale = new Vector3(verticalSize, horizontalSize, 1);
    }

}
