using System.Collections;

using UnityEngine;


public class Building : MonoBehaviour
{
    private Material[] materials = new Material[2];
    private SpriteRenderer sr;

    public float DistanceThreshold;

    IEnumerator LoadMaterials()
    {
        AssetBundle ab = null;

        for (int i = 0; i < AssetManager.Instance.AssetBundlesList.Count; i++)
        {
            if(AssetManager.Instance.AssetBundlesList[i].name == "materials")
            {
                ab = AssetManager.Instance.AssetBundlesList[i];
            }
        }

        if(ab == null)
        {
            Debug.LogError("Cannot find materials AssetBundle");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        AssetBundleRequest abr = ab.LoadAllAssetsAsync<Material>();
        yield return abr;

        for (int i = 0; i < abr.allAssets.Length; i++)
        {
            if (abr.allAssets[i].name == "BuildingDefault")
            {
                materials[0] = (Material)abr.allAssets[i];
            }
            else if (abr.allAssets[i].name == "PlayerIsNear")
            {
                materials[1] = (Material)abr.allAssets[i];
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
                            sr.materials[1].color = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
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
