using System.Collections;
using System.Collections.Generic;
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

    public  override  void ConsumeItem(int index)
    {
        Health      += Inventory.Inv.InventoryList[index].HealthAmount;
        Sanity      += Inventory.Inv.InventoryList[index].SanityAmount;
        DrunkAmount += Inventory.Inv.InventoryList[index].DrunkAmount;
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
