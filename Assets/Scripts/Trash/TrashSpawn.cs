using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrashSpawn : MonoBehaviour, IInteractable
{
    [SerializeField]
    private List<BaseItem> items;
    [SerializeField]
    private new Collider2D collider;

    public Collider2D GetCollider()
    {
        return collider;
    }

    public void Interact(Character source)
    {
        //Implementation is the same for player and ai
        source.Gather(GiveShite());
        items.Clear();
    }

    private List<BaseItem> GiveShite()
    {
        if (items.Count > 0)
        {
            return items;
        }
        else
        {
            return null;
        }
    }

    public void SpawnItems()
    {
        //Populates items list
    }
    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        GameManager.Instance.interactables.Add(this);
    }
}
