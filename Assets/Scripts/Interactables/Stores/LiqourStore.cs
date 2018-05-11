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
        if(source.GetType() != typeof(PlayerController))
        {
            source.Buy(shopInventory.First());
        }
        else
        {
            PlayerController.pl.InterractWithLiqourStore();
        }
    }

    public void Start()
    {
        collider = GetComponent<Collider2D>();
        GameManager.Instance.interactables.Add(this);
    }
}
