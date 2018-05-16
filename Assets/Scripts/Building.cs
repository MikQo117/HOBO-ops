using UnityEngine;

public class Building : MonoBehaviour
{
    public Material DefaultMaterial;
    public Material PlayerIsNear;
    private Material currentMaterial;
    private SpriteRenderer sr;

    public float DistanceThreshold;

    private int currentLayer;
    private int previousLayer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        currentMaterial = GetComponent<Material>();
        sr.material = DefaultMaterial;
    }

    private void Update()
    {
        if(Vector2.Distance(sr.bounds.ClosestPoint(GameManager.Instance.PlayerTransform.position), GameManager.Instance.PlayerTransform.position) < DistanceThreshold)
        {
            if (GameManager.Instance.PlayerOrderInLayer < sr.sortingOrder)
            {
                if (transform.position.y < GameManager.Instance.PlayerTransform.position.y)
                {
                    sr.material = PlayerIsNear;
                }
            }
            else if(sr.material != DefaultMaterial)
            {
                sr.material = DefaultMaterial;
            }
        }
        else if (sr.material != DefaultMaterial)
        {
            sr.material = DefaultMaterial;
        }
        
             

        previousLayer = currentLayer;
        currentLayer = sr.sortingOrder;
    }
}
