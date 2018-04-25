using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Interaction variables
    public List<IInteractable> interactables = new List<IInteractable>();
    public List<Collider2D> interactablesColliders;
    private List<TrashSpawn> trashSpawns = new List<TrashSpawn>();

    //Trash management variables
    public List<TrashSpawn> GetTrashcans
    {
        get
        {
            return trashSpawns;
        }
    }

    private static GameManager instance;

    static public GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    // Use this for initialization
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        

        //interactables = GetComponents< typeof(IInteractable )>().ToList();
        foreach (IInteractable item in interactables)
        {
            interactablesColliders.Add(item.GetCollider());
        }
        foreach (TrashSpawn item in interactables)
        {
            trashSpawns.Add(item);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
