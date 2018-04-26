using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : Character
{
    //Player Variables
    public static PlayerController pl;


    //Camera Variables
    private Camera  mainCamera;
    private float   lenght = 1000;
    private Vector3 SprintVelocity;
    private Vector3 CameraZoffset = new Vector3(0, 0, -5);
    private float   smoothTime = 0.3f;

    public Bounds bound;
    //Getters 

    public float HealthGetter()
    {
        return Health;
    }

    public float SanityGetter()
    {
        return Sanity;
    }

    public float StaminaGetter()
    {
        return stamina;
    }


    //Minigame methods
    protected override void Attack()
    {
    }

    protected override void Beg()
    {
    }

    //Interaction Methods
    public override void ConsumeItem(int itemID)
    {
        if (CharacterInventory.InventoryList.Exists(x => x.BaseItemID == itemID))
        {
            if (CharacterInventory.InventoryList.Find(x => x.BaseItemID == itemID).Consumable)
            {
                BaseItem ConsumableItem = CharacterInventory.InventoryList.Find(x => x.BaseItemID == itemID);

                Health += ConsumableItem.HealthAmount;
                Sanity += ConsumableItem.SanityAmount;
                DrunkAmount += ConsumableItem.DrunkAmount;
                CharacterInventory.RemoveItemFromInventory(itemID);
            }
        }
    }

    public override void ReturnBottle()
    {
        List<BaseItem> items = CharacterInventory.InventoryList.FindAll(x => x.BaseItemID == 0);
        if (items != null && returningBottles)
        {
            for (int i = 0; i < items.Count; i++)
            {
                moneyAmount += items.First().MoneyAmount;
                CharacterInventory.RemoveItemFromInventory(items.First());
            }

        }
        else
        {
            //Display UI stuff that inventory is empty of bottles
            return;
        }
    }

    public override void Buy(BaseItem item)
    {
        CharacterInventory.AddItemToInventory(item);
        moneyAmount -= item.ItemCost;
    }

    protected override void Death()
    {

    }

    public override void Gather(List<BaseItem> items)
    {
        if (items != null)
        {
            //Some ui thing to show what we gathered
            pl.CharacterInventory.AddItemToInventory(items);
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
        returningBottles = Input.GetKeyDown(KeyCode.E) ? true : false;

    }


    protected void CameraMovement()
    {

        Vector3 temp = Vector3.SmoothDamp(mainCamera.transform.position, transform.TransformPoint(movementDirection * 5), ref SprintVelocity, smoothTime);

        if (Sprinting)
        {
            mainCamera.transform.position = temp + CameraZoffset;
        }
        else
        {
            mainCamera.transform.position = Vector3.MoveTowards(temp, transform.position, smoothTime * Time.deltaTime * lenght) + CameraZoffset;
        }
    }

    //Unity Methods
    protected override void Start()
    {
        base.Start();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        CameraMovement();
    }

    protected override void Awake()
    {
        pl = this;
    }


}
