﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrashSpawn : MonoBehaviour, IInteractable
{
    [SerializeField]
    private List<BaseItem>         spawnableItems;
    [SerializeField]
    private List<BaseItem>         trashCanInventory;
    [SerializeField]
    private new Collider2D         collider;
    //variables
    private double                 diceRoll;
    private double                 cumulative;
    private int                    numberOfItems;
    //constatnts
    private const int              maxNumberOfItems = 4;
    private const double           bottleDropChance = 0.7d;

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

    //Methods

    public void Interact(Character source)
    {
        if(source.GetType() != typeof(PlayerController))
        {
        source.Gather(GiveShite());
        TrashCanInventory.Clear();
        }
        else
        {
            PlayerController.pl.Gather(GiveShite());
        }
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
                if (diceRoll < bottleDropChance)
                {
                    numberOfItems = Random.Range(0, maxNumberOfItems);

                    for (int a = 0; a < numberOfItems; a++)
                    {
                        trashCanInventory.Add(spawnableItems[i]);
                    }
                }
                else
                {
                    trashCanInventory.Add(spawnableItems[i]);
                }
                break;
            }
        }
    }

    private void Awake()
    {
        GameManager.Instance.interactables.Add(this);
    }

    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }
}
