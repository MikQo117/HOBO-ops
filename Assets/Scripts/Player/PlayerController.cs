using System.Linq;
using UnityEngine;

public class PlayerController : Character
{
    //Player Variables
    public Consumable              Item;
    public static PlayerController pl;
    protected Inventory            PlayerInventory;

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

    protected override void Gather()
    {
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
