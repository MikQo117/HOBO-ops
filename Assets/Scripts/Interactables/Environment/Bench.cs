using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bench : MonoBehaviour, IInteractable
{
    private new Collider2D collider;

    public Collider2D GetCollider()
    {
        return collider;
    }

    public void Interact(Character source)
    {
        if(source.GetType() != typeof(PlayerController))
        {
            source.Sleep();
        }
        else
        {
            PlayerController.pl.InterractionWithBench();
        }
    }


    void Start()
    {
        collider = GetComponent<Collider2D>();
        GameManager.Instance.interactables.Add(this);
    }
}
