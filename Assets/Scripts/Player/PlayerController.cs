using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : Character
{
    public Consumable Item;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
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

    protected override void ConsumeItem()
    {


        health += inventory[0].HealthAmount;
        inventory.RemoveAt(0);
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
        if (Input.GetKey(KeyCode.E))
            if(inventory.Count != 0)
            ConsumeItem();
           
    }
}
