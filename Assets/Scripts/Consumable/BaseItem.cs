using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TempAsset", menuName = "BaseItem", order = 1)]
public class BaseItem : ScriptableObject
{

    public string name;

    public Sprite sprite;

    //Consumable variables

    public int healthAmount;

    public int sanityAmount;

    public int drunkAmount;


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

    public string ID { get { return name; } set { value = name; } }
}
