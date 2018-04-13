using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Consumable : MonoBehaviour
{

    //Consumable variables
    public List<BaseItem> ItemBase;  
    public int HealthAmount;
    public int SanityAmount;
    public int DrunkAmount;
    public string Objectname;
    public int ConsumableID;


    //Assign Scriptableobjects data to Gameobject
    void InfromationAssignToGameobject()
    {
        int r = Random.Range(0, 2);
        name = ItemBase[r].name;
        gameObject.name = ItemBase[r].name;
        HealthAmount = ItemBase[r].HealthAmount;
        SanityAmount = ItemBase[r].SanityAmount;
        DrunkAmount = ItemBase[r].DrunkAmount;
        ConsumableID = ItemBase[r].BaseItemID;
        GetComponent<SpriteRenderer>().sprite = ItemBase[r].ObjectSprite;
    }

    // Use this for initialization
    void Start()
    {
        InfromationAssignToGameobject();
    }

}
