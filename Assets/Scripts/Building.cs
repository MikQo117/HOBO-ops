using UnityEngine;
using System;
using System.Diagnostics;

public class Building : MonoBehaviour
{
    public Material DefaultMaterial;
    public Material PlayerIsNear;
    private Material currentMaterial;
    private SpriteRenderer sr;
    Stopwatch sw;
    int numFrames;
    int joo;

    public float DistanceThreshold;

    private int currentLayer;
    private int previousLayer;

    /*private void Awake()
    {
        currentMaterial = GetComponent<Material>();
        sr.material = DefaultMaterial;
        sw = new Stopwatch();
        sw.Start();
        numFrames = 0;
        joo = 0;
    }

    private void Update()
    {
        numFrames++;
        if (sw.ElapsedMilliseconds > 1000)
        {
            ++joo;
            print(joo + " FPS in building.cs: " + (float)numFrames / ((float)sw.ElapsedMilliseconds / 1000.0f));
            sw.Reset();
            numFrames = 0;
        }

        // sw.Stop();




        sw.Start();
        if (InputManager.Instance.AxisDown("Horizontal") || InputManager.Instance.AxisDown("Vertical"))
        {
            float distance = Vector2.Distance(sr.bounds.ClosestPoint(GameManager.Instance.PlayerTransform.position), GameManager.Instance.PlayerTransform.position);

            if (distance < DistanceThreshold)
            {
                if (GameManager.Instance.PlayerOrderInLayer < sr.sortingOrder)
                {
                    if (transform.position.y < GameManager.Instance.PlayerTransform.position.y)
                    {
                        if (sr.material != PlayerIsNear)
                        {
                            sr.material = PlayerIsNear;
                            return;
                        }
                    }
                }
                else if (sr.material != DefaultMaterial)
                {
                    sr.material = DefaultMaterial;
                    return;
                }
            }
            else if (sr.material != DefaultMaterial)
            {
                sr.material = DefaultMaterial;
            }
            previousLayer = currentLayer;
            currentLayer = sr.sortingOrder;
        }
    }*/
    
}
