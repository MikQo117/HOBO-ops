using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TempAsset", menuName = "BaseItem", order = 1)]
public class BaseItem : ScriptableObject
{
    //Consumable variables
    private string Objectname;
    [SerializeField]
    private int    healthAmount;
    [SerializeField]
    private int    sanityAmount;
    [SerializeField]
    private int    drunkAmount;
    [SerializeField]
    private int    moneyAmount;
    [SerializeField]
    private int    baseItemID;
    [SerializeField]
    private bool   consumable;
    [SerializeField]
    private Sprite  objectSprite;

    //Get & Set
    public int HealthAmount
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

    public int SanityAmount
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

    public int DrunkAmount
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

    public int MoneyAmount
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

    public int BaseItemID { get { return baseItemID; } set { value = baseItemID; } }

    public bool Consumable { get { return consumable; } set { consumable = value; } }

    public Sprite ObjectSprite
    {
        get
        {
            return objectSprite;
        }

    }
}
