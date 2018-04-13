using System.Linq;
using UnityEngine;

public class PlayerController : Character
{
    public Consumable              Item;

    public static PlayerController pl;

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
    protected override void Attack()
    {
    }

    protected override void Beg()
    {
    }

    public override void ConsumeItem(int index)
    {
       Health      += characterInventory.InventoryList[index].HealthAmount;
       Sanity      += characterInventory.InventoryList[index].SanityAmount;
       DrunkAmount += characterInventory.InventoryList[index].DrunkAmount;
       characterInventory.InventoryList.Remove(characterInventory.InventoryList.Where(x => x.BaseItemID == index).First());
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
}
