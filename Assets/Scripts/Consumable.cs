using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class Consumable : MonoBehaviour
{
    public BaseItem[] ItemBase;
    public int HealthAmount;
    public int SanityAmount;
    public int DrunkAmount;
    
    void GiveItemInfo()
    {

    }

    // Use this for initialization
    void Start()
    {
        int r = Random.Range(0, 1);
            HealthAmount = ItemBase[r].HealthAmount;
            SanityAmount = ItemBase[r].SanityAmount;
            DrunkAmount = ItemBase[r].DrunkAmount;
            GetComponent<SpriteRenderer>().sprite = ItemBase[r].sprite;
        

    }

    enum ItemNameList
    {
        Bottle,
        SandWich
    }
}
