using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    // UI variables
    private List<Transform> childObjects = new List<Transform>();

    // Use this for initialization
    void Start()
    {
        foreach(Transform child in transform)
        {
            childObjects.Add(child.gameObject.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i< childObjects.Count; i++)
        {
            childObjects[i].GetComponentInChildren<Text>().text = PlayerController.pl.InventoryGetter().InventoryList.Where(x => x.BaseItemID == i).Count().ToString();
        }
    }
}
