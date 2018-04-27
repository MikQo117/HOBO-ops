using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Interaction variables
    public List<IInteractable> interactables = new List<IInteractable>();
    private List<TrashSpawn>   trashSpawns = new List<TrashSpawn>();
    public List<Collider2D>    interactablesColliders;
    private float              spawnableItemIndex;
    private const float        originalSpawnTimer = 30.0f;
    float                      spawntimer;
    public List<TrashSpawn> GetTrashSpawns
    {
        get
        {
            return trashSpawns;
        }
    }

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

    // Use this for initialization

    private void AddItemToTrashCans()
    {
        spawntimer -= Time.deltaTime;
        if (spawntimer <= 0)
        {
            for (int i = 0; i < trashSpawns.Count; i++)
            {
                trashSpawns[i].SpawnItems();
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
        }
        foreach (IInteractable item in interactables)
        {
            if (item is TrashSpawn)
            {
                trashSpawns.Add((TrashSpawn)item);
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
