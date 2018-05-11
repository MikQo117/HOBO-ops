using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : Character
{
    //Player Variables
    public static PlayerController pl;
    private bool shopping;
    public bool Gathered;

    //Camera Variables
    public Camera mainCamera;
    private float lenght = 1000;
    private Vector3 SprintVelocity;
    private Vector3 CameraZoffset = new Vector3(0, 0, -5);
    private float smoothTime = 0.3f;
    public Bounds bound;

    //Minigame methods
    protected override void Attack()
    {
    }

    protected override void Beg()
    {
    }

    //Player actions 
    protected void CameraMovement()
    {
        Vector3 temp = Vector3.SmoothDamp(mainCamera.transform.position, transform.TransformPoint(movementDirection * 3), ref SprintVelocity, smoothTime);
        temp.z = -5f;

        if (Sprinting && !exhausted)
        {
            mainCamera.transform.position = temp + CameraZoffset;
        }
        else
        {
            mainCamera.transform.position = Vector3.MoveTowards(temp, transform.position + new Vector3(0, 0.5f), smoothTime * Time.deltaTime * lenght) + CameraZoffset;
        }
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -5f);
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
                inputDirection = direction.normalized * directionMagnitude;
            }
            else
            {
                inputDirection = Vector3.zero;
            }
            movementDirection = inputDirection;
            //Sprint
            if (InputManager.Instance.AxisDown("Fire3"))
                sprinting = true;
            else
                sprinting = false;
        }
        else
        {
            movementDirection = Vector3.zero;
            sprinting = false;
        }
    }

    public override void ConsumeItem(int itemID)
    {
        if (Inventory.InventoryList.Exists(x => x.BaseItemID == itemID))
        {
            if (Inventory.InventoryList.Find(x => x.BaseItemID == itemID).Consumable)
            {
                BaseItem ConsumableItem = Inventory.InventoryList.Find(x => x.BaseItemID == itemID);

                base.Health += ConsumableItem.HealthAmount;
                base.Sanity += ConsumableItem.SanityAmount;
                Inventory.RemoveItemFromInventory(itemID);
            }
        }
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

    public void InterractionWithBench()
    {
        if (canSleep)
        {
            if (InputManager.Instance.AxisPressed("Use"))
            {
                shopping = !shopping;
                UIManager.Instance.SleepWindow(shopping);
            }
        }
    }

    public override void Sleep()
    {
    }
    public void Sleep(int hours)
    {
        if (hours > 0)
        {
            canSleep = false;
            sleeping = true;

            for (int i = 0; i < hours; i++)
            {
                Health += healthGain;
                Sanity += sanityGain;
            }
            float temp = GameManager.Instance.DayTimer;
            GameManager.Instance.DayTimer += hours * GameManager.Instance.Hour;
            sleepTimer = 180.0f;

            if (GameManager.Instance.DayTimer > 456.0f)
            {
                GameManager.Instance.DayTimeIncreaser(hours * GameManager.Instance.Hour - (456 - temp));
            }
            GameManager.Instance.TimesSlept++;
            sleeping = false;
        }
    }

    //Interact Methods
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
        if (InputManager.Instance.AxisPressed("Use"))
        {
            shopping = !shopping;
        }
        UIManager.Instance.LiqourStoreWindow(shopping);
    }

    public override void Gather(List<BaseItem> items)
    {
        if (InputManager.Instance.AxisPressed("Use"))
        {
            if (items != null)
            {
                if (!UIManager.Instance.CRisRunning)
                {
                    StartCoroutine(UIManager.Instance.PickupIndicator(items));
                }
                Inventory.AddItemToInventory(items);
                Gathered = true;
            }
            else
            {
                UIManager.Instance.NoItemIndicator();
            }
        }
        else
        {
            Gathered = false;
        }

    }

    public override void Buy(BaseItem item)
    {
        if (item.BaseItemID == 2 || item.BaseItemID == 3 || item.BaseItemID == 6)
        {
            GameManager.Instance.FoodBought++;
        }
        else if (item.BaseItemID == 0)
        {
            GameManager.Instance.BeersBought++;
        }
        else if (item.BaseItemID == 1)
        {
            GameManager.Instance.WhiskeyBought++;
        }
        Inventory.AddItemToInventory(item);
        moneyAmount -= item.ItemCost;
    }

    protected override void Death()
    {

    }

    private void EpromtChecker()
    {
        foreach (Collider2D item in GameManager.Instance.interactablesColliders)
        {

            if (transform.GetComponent<Collider2D>().bounds.Contains(item.transform.position))
            {
                UIManager.Instance.Eprompt(true);
                break;
            }
            else
            {
                UIManager.Instance.Eprompt(false);
            }
        }
    }


    //Unity Methods
    protected override void Start()
    {
        base.Start();
        shopping = false;
        mainCamera = Camera.main;
    }

    protected override void Update()
    {
        base.Update();
        CameraMovement();
        EpromtChecker();
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
