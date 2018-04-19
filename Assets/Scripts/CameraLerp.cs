using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    private Camera mainCamera;
    private float offSet = 1;
    private float maxOffset = 1;
    private const float maxMovement = 20;
    public float lenght = 10;
    public Vector3 Normalvelocity;
    public Vector3 SprintVelocity;
    public Vector3 PreviousLocation;
    public Vector3 CameraZoffset = new Vector3(0, 0, -5);
    private float smoothTime = 0.1f;

    public float OffSet
    {
        get
        {
            return offSet;
        }

        set
        {
            Mathf.Clamp01(offSet);
        }
    }

    // Use this for initialization


    void Start()
    {
        mainCamera = Camera.main;
        PreviousLocation = PlayerController.pl.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentVelocity = PlayerController.pl.transform.TransformPoint(new Vector3(PlayerController.pl.movementDirection.x, PlayerController.pl.movementDirection.y, 0));
        Debug.Log(currentVelocity);
        if (PlayerController.pl.Sprinting)
        {
            mainCamera.transform.position = (CameraZoffset + Vector3.SmoothDamp(PreviousLocation,currentVelocity,ref SprintVelocity,smoothTime,10 ,Time.deltaTime));
        }
        else
        {
            mainCamera.transform.position = (CameraZoffset + Vector3.SmoothDamp(PlayerController.pl.transform.position, transform.InverseTransformDirection(PlayerController.pl.transform.position - PreviousLocation), ref Normalvelocity, smoothTime, 10, Time.deltaTime));
        }

        PreviousLocation =PlayerController.pl.transform.position;
    }
}
