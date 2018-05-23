using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : Character
{
    //Player Variables
    public static PlayerController pl;
    private bool interaction;
    public bool Gathered;
    private bool shopping;
    private bool paused;
    private bool submit;
    private bool inventoryAccess;

    //Camera Variables
    public Camera mainCamera;
    private Vector2 SprintVelocity;
    private Vector3 CameraZoffset = new Vector3(0, 0, -5);
    private float smoothTime = 0.3f;
    public Bounds bound;

    //Get & Set
    public bool Paused
    {
        get
        {
            return paused;
        }
        set
        {
            paused = value;
        }
    }

    public bool Interaction
    {
        get
        {
            return interaction;
        }

        set
        {
            interaction = value;
        }
    }

    public bool Submit
    {
        get
        {
            return submit;
        }

        set
        {
            submit = value;
        }
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
        if (!paused)
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

                //Apply inputvector to overall movementvector which is calculated in character class
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

            if (InputManager.Instance.AxisPressed("Submit") && interaction)
            {
                Submit = !Submit;
            }

            if (canInteract)
            {
                if (InputManager.Instance.AxisPressed("Use"))
                {
                    Interaction = !Interaction;
                }
            }
            else
            {
                Interaction = false;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                inventoryAccess = !inventoryAccess;
            }


        }
        else
        {
            inputDirection = Vector3.zero;
            movementDirection = Vector3.zero;
        }

        UIManager.Instance.Inventory(inventoryAccess);

        if (InputManager.Instance.AxisPressed("Pause"))
        {
            if (!interaction && !inventoryAccess)
            {
                paused = !paused;

                Time.timeScale = paused ? 0 : 1;

                UIManager.Instance.PausemenuActive(paused);
            }
            else
            {
                interaction = false;
            }
        }

        if (UIManager.Instance.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            if (InputManager.Instance.AxisPressed("Pause"))
            {
                inventoryAccess = !inventoryAccess;
            }
        }
    }

    public override void ConsumeItem(BaseItem item)
    {

        if (Inventory.InventoryList.Exists(x => x == item) && item.Consumable)
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
        }

    }

    public override void ReturnBottle()
    {
        List<BaseItem> items = Inventory.InventoryList.FindAll(x => x.BaseItemID == 8);
        if (items != null)
        {
            StartCoroutine(UIManager.Instance.PickupIndicator(items, true));
            for (int i = 0; i < items.Count; i++)
            {
                moneyAmount += items.First().MoneyAmount;
                Inventory.RemoveItemFromInventory(items.First());
            }
        }
    }

    public override void Gather(List<BaseItem> items)
    {
        if (Interaction)
        {
            canInteract = false;
            interaction = false;
            if (items != null)
            {
                if (!UIManager.Instance.CRisRunning)
                {
                    StartCoroutine(UIManager.Instance.PickupIndicator(items, false));
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
        canInteract = true;
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

    //Interact Methods
    public void InterractionWithBench()
    {
        if (canSleep)
        {
            if (Interaction)
            {
                UIManager.Instance.SleepWindow(Interaction);
            }
        }
        else
        {
            if (Interaction)
            {
                StartCoroutine(UIManager.Instance.SleepTextActivator());
                Interaction = !Interaction;
            }
        }

    }

    public void InterractWithStore()
    {
        if (Interaction)
        {
            shopping = true;
        }
        else
        {
            shopping = false;
        }
        UIManager.Instance.ShopWindow(shopping);
    }

    public void InterractWithLiqourStore()
    {
        if (Interaction)
        {
            shopping = true;
        }
        else
        {
            shopping = false;
        }
        UIManager.Instance.LiqourStoreWindow(shopping);
    }

    //Character overrides
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
            UIManager.Instance.SleepWindow(false);

            for (int i = 0; i < hours; i++)
            {
                Health += healthGain;
                Sanity += sanityGain;
                UIManager.Instance.FadetoBlack(true);
            }

            float temp = GameManager.Instance.DayTimer;
            GameManager.Instance.DayTimer += hours * GameManager.Instance.Hour;
            sleepTimer = 180.0f;

            if (GameManager.Instance.DayTimer > 456.0f)
            {
                GameManager.Instance.DayTimeIncreaser(hours * GameManager.Instance.Hour - (456 - temp));
            }

            GameManager.Instance.TimesSlept++;
            interaction = !interaction;
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

    protected override void StatsDecay()
    {
        if (!interaction)
        {
            base.StatsDecay();
        }

    }

    //Indicator methods
    private void StatusChecker()
    {
        if (Health <= 20.0f)
        {
            UIManager.Instance.StatusBarLowIndicator(1);
        }
        if (Sanity <= 15.0f)
        {
            UIManager.Instance.StatusBarLowIndicator(2);
        }

        if (Stamina <= 20.0f)
        {
            UIManager.Instance.StatusBarLowIndicator(3);
        }
    }

    //Unity Methods
    protected override void Start()
    {
        base.Start();
        mainCamera = Camera.main;
    }

    protected override void Update()
    {
            base.Update();
            CameraMovement();
            PauseMethod();
            StatusChecker();
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
