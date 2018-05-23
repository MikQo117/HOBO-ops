using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureScaler : MonoBehaviour
{
    public Camera Cam;

    void Update()
    {


        float pos = (Cam.nearClipPlane + 0.01f);

        transform.position = Cam.transform.position + Cam.transform.forward * pos;

        float h = Mathf.Tan(Cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * pos * 2f;

        transform.localScale = new Vector3(h * Cam.aspect, h, 0f);
    }

}
