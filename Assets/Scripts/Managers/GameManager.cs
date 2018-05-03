using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Interaction variables
    public List<IInteractable> interactables = new List<IInteractable>();
    private List<TrashSpawn> trashSpawns = new List<TrashSpawn>();
    public List<Collider2D> interactablesColliders;
    private float spawnableItemIndex;
    private float originalSpawnTimer = 30.0f;
    float spawntimer;

    //Daysystem variables
    private float[] intervals = { 0.0f, 150.0f, 300.0f, 375.0f, 430.5f, 456.0f };
    private float dayTimer = 450.0f;
    private const float regulartimeDrop = 30.0f;
    private const float rushHourTimeDrop = 15.0f;
    private bool rushHour;

    private static GameManager instance;

    //Getters
    public List<TrashSpawn> GetTrashSpawns
    {
        get { return trashSpawns; }
    }

    static public GameManager Instance
    {
        get { return instance; }
    }

    public float DayTimer
    {
        get { return dayTimer; }
    }

    //Methods

    private void AddItemToTrashCans()
    {

        if (spawntimer <= 0)
        {
            for (int i = 0; i < trashSpawns.Count; i++)
            {
                trashSpawns[i].SpawnItems();
            }
            ResetTimer();
        }
    }

    private void IntervalChecker()
    {
        RushHourChecker();
        if (!rushHour)
        {
            if (dayTimer > intervals[1])
            {
                originalSpawnTimer = regulartimeDrop;
            }

            if (dayTimer >= intervals[5])
            {
                dayTimer = 0.0f;
            }

            if (dayTimer > intervals[4] || DayTimer < intervals[1])
            {
                ResetTimer();
            }
        }

    }

    private void RushHourChecker()
    {
        if (DayTimer > intervals[2] && DayTimer < intervals[3])
        {
            rushHour = true;
            originalSpawnTimer = rushHourTimeDrop;
        }
        else
        {
            rushHour = false;
        }
    }

    private void ResetTimer()
    {
        spawntimer = originalSpawnTimer;
    }

    private void TimeChanger()
    {
        spawntimer -= Time.deltaTime;
        dayTimer   += Time.deltaTime * 10;
    }

    //Unity methods

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
        TimeChanger();
        IntervalChecker();
        AddItemToTrashCans();
    }
}
