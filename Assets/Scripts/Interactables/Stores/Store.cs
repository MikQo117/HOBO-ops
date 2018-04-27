﻿using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Store : MonoBehaviour, IInteractable
{

    private new Collider2D collider;

    public Collider2D GetCollider()
    {
        return collider;
    }

    public void Interact(Character source)
    {
        source.ReturnBottle();
    }

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        GameManager.Instance.interactables.Add(this);
    }
}