using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
        if (shopInventory.ElementAt(0).ItemCost < source.MoneyAmount && Input.GetKeyDown(KeyCode.F))
        {
            source.Buy(shopInventory.ElementAt(0));
        }
        else if(shopInventory.ElementAt(1).ItemCost < source.MoneyAmount && Input.GetKeyDown(KeyCode.R))
        {
            source.Buy(shopInventory.ElementAt(1));
        }
    }
    public void Start()
    {
        collider = GetComponent<Collider2D>();
        GameManager.Instance.interactables.Add(this);
    }
}
