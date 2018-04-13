using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    // UI variables
    private List<Transform> childObjects;

    // Use this for initialization
    void Start()
    {
        for(int i= 0; i < transform.childCount; i++)
        {
            childObjects.Add(gameObject.transform.GetChild(i).transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
