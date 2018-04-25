using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TempAsset", menuName = "BaseItem", order = 1)]
public class BaseItem : ScriptableObject
{
    //Consumable variables
    [SerializeField]
    private string   objectname;
    [SerializeField]
    private float    healthAmount;
    [SerializeField]
    private float    sanityAmount;
    [SerializeField]
    private float    drunkAmount;
    [SerializeField]
    private float    moneyAmount;
    [SerializeField]
    private int      baseItemID;
    [SerializeField]
    private bool     consumable;
    [SerializeField]
    private Sprite   objectSprite;
    [SerializeField]
    private float    itemCost;
    //Get & Set
    public float HealthAmount
    {
        get
        { 
        
            return healthAmount;
        }

        private set
        {
            healthAmount = value;
        }
    }

    public float SanityAmount
    {
        get
        {
            return sanityAmount;
        }

        private set
        {
            sanityAmount = value;
        }
    }

    public float DrunkAmount
    {
        get
        {
                return drunkAmount;
        }

        private set
        {
            drunkAmount = value;
        }
    }

    public float MoneyAmount
    {
        get
        {
            if (!Consumable)
                return moneyAmount;
            else
                return 0;
        }

        set
        {
            moneyAmount = value;
        }

    }

    public float ItemCost {get { return itemCost; }}

    public int BaseItemID { get { return baseItemID; } set { value = baseItemID; } }

    public bool Consumable { get { return consumable; } set { consumable = value; } }

    public Sprite ObjectSprite
    {
        get
        {
            return objectSprite;
        }

    }

    public string Objectname
    {
        get
        {
            return objectname;
        }
    }
}
