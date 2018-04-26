using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrashSpawn : MonoBehaviour, IInteractable
{
    [SerializeField]
    private List<BaseItem> spawnableItems;
    [SerializeField]
    private List<BaseItem> trashCanInventory;
    [SerializeField]
    private new Collider2D collider;
    double                 diceRoll;
    double                 cumulative = 0.0f;



    //GET & SET
    public List<BaseItem> Spawnableitems
    {
        get
        {
            return spawnableItems;
        }

        set
        {
            spawnableItems = value;
        }
    }

    public List<BaseItem> TrashCanInventory
    {
        get
        {
            return trashCanInventory;
        }

        set
        {
            trashCanInventory = value;
        }
    }

    public Collider2D GetCollider()
    {
        return collider;
    }

    public static TrashSpawn Instance;

    //Methods

    public void Interact(Character source)
    {
        //Implementation is the same for player and ai
        source.Gather(GiveShite());
        TrashCanInventory.Clear();
    }

    private List<BaseItem> GiveShite()
    {
        if (TrashCanInventory.Count > 0)
        {
            return TrashCanInventory;
        }
        else
        {
            return null;
        }
    }

    public void SpawnItems()
    {
        cumulative = 0.0f;
        diceRoll = Random.value;
        for (int i = 0; i < spawnableItems.Count; i++)
        {
            cumulative += spawnableItems[i].DropProBability;

            if (diceRoll < cumulative)
            {
                trashCanInventory.Add(spawnableItems[i]);
                break;
            }
        }

    }

    private void Awake()
    {
        Instance = this;
        collider = GetComponent<Collider2D>();
        GameManager.Instance.interactables.Add(this);
    }
}
