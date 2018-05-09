using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Made by Sipi Raussi 25.4.2018
[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class TileObject : MonoBehaviour
{
    // Tile information
    public Tile Tile;

    private SpriteRenderer sr;

    private AssetBundle ab;

    private void LateUpdate()
    {
        if(!Application.isPlaying)
        {
            if (!sr)
            {
                sr = GetComponent<SpriteRenderer>();
                if(Tile)
                {
                    sr.sprite = Tile.Sprite;
                }
            }
            else
            {
                Sprite previousSprite = Tile.Sprite;
                if (sr.sprite == previousSprite)
                {
                    return;
                }
                else
                {
                    sr.sprite = previousSprite;
                }
            }
        }
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            sr = GetComponent<SpriteRenderer>();

            // Set tile information to game object
            if (Tile == null)
            {
                Debug.LogError("Tile doesn't have Tile information!");
                return;
            }
            else
            {
                sr.sprite = Tile.Sprite;

                List<AssetBundle> abl = AssetManager.Instance.AssetBundlesList;
                AssetBundle ab;

                for (int i = 0; i < AssetManager.Instance.AssetBundlesList.Count; i++)
                {
                    if (AssetManager.Instance.AssetBundlesList[i].name == "materials")
                    {
                        ab = abl[i];
                        sr.material = ab.LoadAsset<Material>("PixelSnap.mat");
                        break;
                    }
                }
            }
            Destroy(this);
        }
    }

#if UNITY_EDITOR

    private Color SidewalkBeige = new Color(0.65f, 0.61f, 0.53f);
    private Color RoadBlack = new Color(0.27f, 0.27f, 0.27f);

    private void OnDrawGizmos()
    {
        if (!Tile)
        {
            // Don't display Gizmo's on play
            if (!EditorApplication.isPlaying)
            {
                // Show details of the tile
                if (Tile != null)
                {
                    if (SceneView.lastActiveSceneView.camera.orthographicSize <= 5f)
                    {
                        Handles.Label(transform.position + new Vector3(-0.2f, 0.3f, 0), Tile.name);
                    }

                    if (Tile.GetType() == typeof(RoadTile))
                    {
                        Gizmos.color = RoadBlack;
                    }
                    else if (Tile.GetType() == typeof(SidewalkTile))
                    {
                        Gizmos.color = SidewalkBeige;
                    }
                    else
                    {
                        Gizmos.color = Color.blue;
                    }

                    Gizmos.DrawSphere(transform.position, 0.1f);
                }
                // Give me tile!
                else
                {
                    if (SceneView.lastActiveSceneView.camera.orthographicSize <= 5f)
                    {
                        Handles.Label(transform.position + new Vector3(-0.2f, 0.3f, 0), "I need tile!");
                    }

                    Gizmos.color = Color.blue;

                    Gizmos.DrawSphere(transform.position, 0.1f);
                }
            }
        }
    }
#endif
}
