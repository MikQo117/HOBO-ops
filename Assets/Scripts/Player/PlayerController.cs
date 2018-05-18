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
    private bool paused;

    //Camera Variables
    public Camera mainCamera;
    private Vector2 SprintVelocity;
    private Vector3 CameraZoffset = new Vector3(0, 0, -5);
    private float smoothTime = 0.3f;
    public Bounds bound;

    //Get & Set
    public bool Paused
    {
        get { return paused; }
    }

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
        mainCamera.transform.position = Vector2.SmoothDamp(mainCamera.transform.position, transform.TransformPoint(movementDirection * 3), ref SprintVelocity, smoothTime, Mathf.Infinity, Time.deltaTime);
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, CameraZoffset.z);
    }

    private void PauseMethod()
    {

        if (paused)
        {
            //UIManager.Instance.PauseMenu(paused);
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

        if (InputManager.Instance.AxisDown("Pause"))
        {
            paused = !paused;
        }
    }

    public override void ConsumeItem(BaseItem item)
    {
        
            if (Inventory.InventoryList.Exists(x => item) && item.Consumable)
            {


                if (item.BaseItemID == 0)
                {
                    GameManager.Instance.BeersConsumed++;
                }
                else if (item.BaseItemID == 1)
                {
                    GameManager.Instance.WhiskeyConsumed++;
                }
                else
                {
                    GameManager.Instance.FoodConsumed++;
                }

                base.Health += item.HealthAmount;
                base.Sanity += item.SanityAmount;
                Inventory.RemoveItemFromInventory(item);
            Debug.Log("removed");
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
        else
        {
            if (InputManager.Instance.AxisPressed("Use"))
            {
                StartCoroutine(UIManager.Instance.SleepTextActivator());
            }
        }

    }

    protected override void Collision()
    {
        base.Collision();
        
    }

    public override void Sleep()
    {
    }

    public override void Sleep(int hours)
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
                items.Clear();
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
        if (MoneyAmount >= item.ItemCost)
        {
            if (item.BaseItemID == 0)
            {
                GameManager.Instance.BeersBought++;
            }
            else if (item.BaseItemID == 1)
            {
                GameManager.Instance.WhiskeyBought++;
            }
            else
            {
                GameManager.Instance.FoodBought++;
            }
            Inventory.AddItemToInventory(item);
            moneyAmount -= item.ItemCost;
        }
    }

    protected override void Death()
    {
        UIManager.Instance.DeathScreen();
    }

    protected override void SpriteFlip(bool inverted)
    {
        base.SpriteFlip(false);
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
        PauseMethod();
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
