using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    //Player Variables
    public static PlayerController pl;

    protected override void Attack()
    {
    }

    protected override void Beg()
    {
    }

    public override void ConsumeItem(int itemID)
    {
        if (characterInventory.InventoryList.Exists(x => x.BaseItemID == itemID)) 
        {
            if (characterInventory.InventoryList.Find(x => x.BaseItemID == itemID).Consumable) 
            {
                var ConsumableItem = characterInventory.InventoryList.Find(x => x.BaseItemID == itemID);

                Health += ConsumableItem.HealthAmount;
                Sanity += ConsumableItem.SanityAmount;
                DrunkAmount += ConsumableItem.DrunkAmount;
                characterInventory.RemoveItemFromInventory(itemID);
            }
        }
    }

    protected override void Death()
    {

    }

    public override void Gather(List<BaseItem> items)
    {
        if (items != null)
        {
            //Some ui thing to show what we gathered
            pl.characterInventory.AddItemToInventory(items);
        }
        else
        {
            //No items found
        }
    }

    protected override void GetInput()
    {
        //Get WASD directions
        movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //Sprint
        if (Input.GetKey(KeyCode.LeftShift))
            sprinting = true;
        else
            sprinting = false;

    }

    public Inventory InventoryGetter()
    {
        return characterInventory;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        pl = this;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
