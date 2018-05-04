﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Character
{
    //Player Variables
    public static PlayerController pl;
    private bool shopping;


    //Camera Variables
    public  Camera  mainCamera;
    private float   lenght = 1000;
    private Vector3 SprintVelocity;
    private Vector3 CameraZoffset = new Vector3(0, 0, -5);
    private float   smoothTime = 0.3f;

    public Bounds bound;

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
        if (Inventory.InventoryList.Exists(x => x.BaseItemID == itemID))
        {
            if (Inventory.InventoryList.Find(x => x.BaseItemID == itemID).Consumable)
            {
                BaseItem ConsumableItem = Inventory.InventoryList.Find(x => x.BaseItemID == itemID);

                base.Health += ConsumableItem.HealthAmount;
                base.Sanity += ConsumableItem.SanityAmount;
                DrunkAmount += ConsumableItem.DrunkAmount;
                Inventory.RemoveItemFromInventory(itemID);
            }
        }
    }

    public void InterractWithStore()
    {
        if (InputManager.Instance.AxisPressed("Use"))
        {
            shopping = !shopping;
        }

        
        UIManager.Instance.ShopWindow(shopping);
    }

    public void InterractWithLiqourStore()
    {
        if(InputManager.Instance.AxisPressed("Use"))
        {
            shopping = !shopping;
        }
        UIManager.Instance.LiqourStoreWindow(shopping);
    }

    public override void ReturnBottle()
    {
        
        List<BaseItem> items = Inventory.InventoryList.FindAll(x => x.BaseItemID == 8);
        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                moneyAmount += items.First().MoneyAmount;
                Inventory.RemoveItemFromInventory(items.First());
            }
        }
    }

    public override void Buy(BaseItem item)
    {
            Inventory.AddItemToInventory(item);
            moneyAmount -= item.ItemCost;
    }

    protected override void Death()
    {

    }

    public override void Gather(List<BaseItem> items)
    {
        if (items != null)
        {
            UIManager.Instance.PickupIndicator(items);
            pl.Inventory.AddItemToInventory(items);
        }
        else
        {
            //No items found
        }
    }

    protected override void GetInput()
    {
        if (!shopping)
        {
            if (InputManager.Instance.AxisDown("Horizontal") || InputManager.Instance.AxisDown("Vertical"))
            {
                //Determines wanted direction
                Vector2 direction = Vector2.right * InputManager.Instance.XAxis + Vector2.up * InputManager.Instance.YAxis;

                float directionMagnitude = direction.magnitude;

                if (directionMagnitude > 1)
                {
                    directionMagnitude = 1;
                }

                //Destination is unit vector * Speed and directions magnitude effects on how much speed is used;
                movementDirection = direction.normalized * directionMagnitude;
            }
            else
            {
                movementDirection = Vector3.zero;
            }

            //Sprint
            if (InputManager.Instance.AxisDown("Fire3"))
                sprinting = true;
            else
                sprinting = false;
        }
        else
        {
            movementDirection = Vector2.zero;
            sprinting = false;
        }
    }

    protected void CameraMovement()
    {

        Vector3 temp = Vector3.SmoothDamp(mainCamera.transform.position, transform.TransformPoint(movementDirection * 3), ref SprintVelocity, smoothTime);

        if (Sprinting && !exhausted)
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
        shopping = false;
        mainCamera = Camera.main;
        UIManager.Instance.PickupObject.SetActive(false);
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
        if (InputManager.Instance == null)
        {
            GameObject inputManager = new GameObject();
            inputManager.AddComponent<InputManager>();
            inputManager.name = "InputManager";
        }
    }
}
