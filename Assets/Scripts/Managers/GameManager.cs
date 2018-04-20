using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Trash management variables
    public List<IInteractable> interactables = new List<IInteractable>();
    public List<Collider2D> interactablesColliders;

    private  static GameManager instance;

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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
