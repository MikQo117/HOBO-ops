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
        if (source.GetType() != typeof(PlayerController))
        {
            source.ReturnBottle();
        }
        else
        {
            PlayerController.pl.InterractWithStore();
        }
    }

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        GameManager.Instance.interactables.Add(this);
    }
}
