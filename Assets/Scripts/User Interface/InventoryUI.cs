using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    // UI variables
    private List<Transform> childObjects = new List<Transform>();
    [SerializeField]
    private List<BaseItem> AllItems;
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
        for(int i = 0; i < childObjects.Count; i++)
        {
            childObjects[i].GetComponent<Image>().sprite = AllItems[i].ObjectSprite;
            childObjects[i].GetComponentInChildren<Text>().text = PlayerController.pl.CharacterInventory.InventoryList.Count(x => x.BaseItemID == i).ToString();
        }
    }
    enum SpawnableitemList
    {
        Bottle,
        Half_ChocolateBar,
        Black_Banana,
        Old_SandWich,
        Banana,
        BeefJerky,
        Beer,
        ChocolateBar,
        Sandwich,
        Whiskey
    }
}
