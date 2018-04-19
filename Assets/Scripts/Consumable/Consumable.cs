using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Consumable : MonoBehaviour
{

    //Consumable variables
    [SerializeField]
    private List<BaseItem> itemBase;
    private int            healthAmount;
    private int            sanityAmount;
    private int            drunkAmount;
    private string         objectname;
    private int            consumableID;

    //Consumable Getters
    public int HealthAmount
    {
        get
        {
            return healthAmount;
        }
    }

    public int SanityAmount
    {
        get
        {
            return sanityAmount;
        }

    }

    public int DrunkAmount
    {
        get
        {
            return drunkAmount;
        }

    }

    public int ConsumableID
    {
        get
        {
            return consumableID;
        }

    }

    public List<BaseItem> ItemBase
    {
        get
        {
            return itemBase;
        }
    }

    //Assign Scriptableobjects data to monobehaviour script
    private void InfromationAssignToGameobject()
    {
        int r                                 = Random.Range(0, 2);
        name                                  = ItemBase[r].Objectname;
        gameObject.name                       = ItemBase[r].Objectname;
        healthAmount                          = ItemBase[r].HealthAmount;
        sanityAmount                          = ItemBase[r].SanityAmount;
        drunkAmount                           = ItemBase[r].DrunkAmount;
        consumableID                          = ItemBase[r].BaseItemID;
        GetComponent<SpriteRenderer>().sprite = ItemBase[r].ObjectSprite;
    }

    // Use this for initialization
    private void Start()
    {
        InfromationAssignToGameobject();
    }

}
