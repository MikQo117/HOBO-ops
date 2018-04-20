using System.Linq;
using UnityEngine;

public class PlayerController : Character
{
    //Player Variables
    public Consumable              Item;
    public static PlayerController pl;


    //Camera Variables
    private Camera                 mainCamera;
    private float                  lenght = 1000;
    private Vector3                SprintVelocity;
    private Vector3                CameraZoffset = new Vector3(0, 0, -5);
    private float                  smoothTime = 0.3f;

    public Bounds bound;
    //Getters 
    public Inventory InventoryGetter()
    {
        return characterInventory;
    }

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

    //Methdos

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
