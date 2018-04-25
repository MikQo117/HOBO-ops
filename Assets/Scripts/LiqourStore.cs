using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class LiqourStore : MonoBehaviour, IInteractable
{

    private new Collider2D collider;

    [SerializeField]
    private List<BaseItem> shopInventory;

    public Collider2D GetCollider()
    {
        return collider;
    }

    public void Interact(Character source)
    {
        if (shopInventory.Exists(x => x.ItemCost < source.MoneyAmount))
        {
            source.Buy(shopInventory);
        }
    }

    public void Start()
    {
        collider = GetComponent<Collider2D>();
        GameManager.Instance.interactables.Add(this);
    }
}
