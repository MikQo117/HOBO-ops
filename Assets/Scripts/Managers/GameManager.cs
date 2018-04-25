using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Trash management variables
    public List<IInteractable> interactables = new List<IInteractable>();
    private List<TrashSpawn>   trashCans = new List<TrashSpawn>();
    public List<Collider2D>    interactablesColliders;
    private float              spawnableItemIndex;
    private const float        originalSpawnTimer = 1.0f;
    float                      spawntimer;

    //Others
    private static GameManager instance;

    //Getters
    static public GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public List<TrashSpawn>   GetTrashCans
    {
        get
        {
            return trashCans;
        }
    }

    // Use this for initialization

    private void AddItemToTrashCans()
    {
        spawntimer -= Time.deltaTime;
        if (spawntimer <= 0)
        {
            for (int i = 0; i < trashCans.Count; i++)
            {
                spawnableItemIndex = Random.Range(0.0f, 10.1f);
                if (spawnableItemIndex <= 7)
                {
                    int amountOfBottlesSpawned = Random.Range(2, 5);

                    for (int a = 0; a < amountOfBottlesSpawned; a++)
                    {
                        trashCans.ElementAt(i).TrashCanInventory.Add(TrashSpawn.Instance.Spawnableitems.ElementAt((int)SpawnableitemList.Bottle));
                    }
                }
                else if (spawnableItemIndex > 7 && spawnableItemIndex <= 8.5f)
                {
                    trashCans.ElementAt(i).TrashCanInventory.Add(TrashSpawn.Instance.Spawnableitems.ElementAt((int)SpawnableitemList.Black_Banana));

                }
                else if (spawnableItemIndex > 8.5f && spawnableItemIndex < 9.5f)
                {
                    trashCans.ElementAt(i).TrashCanInventory.Add(TrashSpawn.Instance.Spawnableitems.ElementAt((int)SpawnableitemList.Half_ChocolateBar));
                }
                else
                {
                    trashCans.ElementAt(i).TrashCanInventory.Add(TrashSpawn.Instance.Spawnableitems.ElementAt((int)SpawnableitemList.Old_SandWich));
                }
            }
        ResetTimer();
        }

    }

    void ResetTimer()
    {
        spawntimer = originalSpawnTimer;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        foreach (IInteractable item in interactables)
        {
            interactablesColliders.Add(item.GetCollider());
            if (item.GetType() == typeof(TrashSpawn))
            {
                trashCans.Add((TrashSpawn)item);
            }
        }
        ResetTimer();
    }

    private void Update()
    {
        AddItemToTrashCans();
    }

    enum SpawnableitemList
    {
        Bottle,
        Half_ChocolateBar,
        Black_Banana,
        Old_SandWich
    }
}
