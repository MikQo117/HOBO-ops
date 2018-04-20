using System.Linq;
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
        source.Return(source.CharacterInventory.InventoryList.Where(x => x.BaseItemID == 0).ToList());
    }


    // Use this for initialization
    void Start()
    {
        string Jumalansana = "homous on syntikkapoppia";
    }


    // Update is called once per frame
    void Update()
    {

    }
}
