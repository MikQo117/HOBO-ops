using System.Collections;

using UnityEngine;


public class Building : MonoBehaviour
{
    private Material currentMaterial;
    private Material[] materials = new Material[2];
    private SpriteRenderer sr;

    public float DistanceThreshold;

    IEnumerator LoadMaterials()
    {
        AssetBundle ab = AssetManager.Instance.AssetBundlesList[0];

        AssetBundleRequest abr = ab.LoadAllAssetsAsync<Material>();
        yield return abr;

        for (int i = 0; i < abr.allAssets.Length; i++)
        {
            if (abr.allAssets[i].name == "BuildingDefault")
            {
                Material defaultMaterial = (Material)abr.allAssets[i];
                materials[0] = defaultMaterial;
            }
            else if (abr.allAssets[i].name == "PlayerIsNear")
            {
                Material playerIsNear = (Material)abr.allAssets[i];
                materials[1] = playerIsNear;
            }
        }

        sr.materials = materials;
        sr.materials[1].color = Vector4.zero;
        sr.materials[0].color = Vector4.one;
        yield return null;
    }
        
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        currentMaterial = GetComponent<Material>();

        StartCoroutine(LoadMaterials());
    }

    private void Update()
    {
        if (Time.frameCount % 20 == 0)
        {
            if (InputManager.Instance.AxisDown("Horizontal") || InputManager.Instance.AxisDown("Vertical"))
            {
                float distance = Vector2.Distance(sr.bounds.ClosestPoint(GameManager.Instance.PlayerTransform.position), GameManager.Instance.PlayerTransform.position);

                if (distance <= DistanceThreshold)
                {
                    if (GameManager.Instance.PlayerOrderInLayer < sr.sortingOrder)
                    {
                        if (transform.position.y < GameManager.Instance.PlayerTransform.position.y)
                        {
                            sr.materials[0].color = Vector4.zero;
                            sr.materials[1].color = Vector4.one;
                        }
                    }
                }

                else if(sr.materials[1].color.a != 0)
                {
                    sr.materials[1].color = Vector4.zero;
                    sr.materials[0].color = Vector4.one;
                }
            }
        }
    }

}
