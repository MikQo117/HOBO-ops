using System.Drawing;
using System.IO;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

// Made by Sipi Raussi 25.4.2018
public class CreateCavalierMapTool : EditorWindow
{
    private SerializedObject so;

    private Sprite pixelMap;

    // Store all pixel data
    private PixelData[,] pixelDatas;
    [SerializeField]
    private List<PixelData> pixelVariations;

    // Tile information
    private SidewalkTile[] sidewalkTiles;
    private RoadTile[] roadTiles;

    [MenuItem("Custom Tools/Create Cavalier Map")]
    private static void ShowWindow()
    {
        CreateCavalierMapTool window = GetWindow<CreateCavalierMapTool>("Create Cavalier Map");
        window.position = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 400, 200);
        window.Show();
    }

    private void OnEnable()
    {
        ScriptableObject target = this;
        so = new SerializedObject(target);
    }

    private void OnGUI()
    {
        Utilitys.PrintHeader("Map Creation Tool");

        // Enable GUI if there is pixelmap
        pixelMap = (Sprite)EditorGUILayout.ObjectField("Pixel Map:", pixelMap, typeof(Sprite), false);
        if (pixelMap == null)
        {
            GUI.enabled = false;
        }
        else
        {
            GUI.enabled = true;

            EditorGUILayout.LabelField("Dimensions (W x H): " + pixelMap.texture.width.ToString() + "px " + "x " + pixelMap.texture.height.ToString() + "px");
        }

        // Read pixels button
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Read Pixels", GUILayout.MaxWidth(100)))
        {
            ReadPixels();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        // Show pixel properties
        if (pixelDatas != null && pixelMap != null)
        {
            so.Update();
            EditorList.Show(so.FindProperty("pixelVariations"), false);
            so.ApplyModifiedProperties();
        }

        // Get tiles from project
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Get Tiles", GUILayout.MaxWidth(100)))
        {
            GetTiles();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Place Tiles", GUILayout.MaxWidth(100)))
        {
            PlaceTiles();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void CreateCavalierMap()
    {

    }



    //Read pixelmap
    private void ReadPixels()
    {
        pixelDatas = new PixelData[pixelMap.texture.height, pixelMap.texture.width];

        List<UnityEngine.Color> tempColorVariation = new List<UnityEngine.Color>();

        pixelVariations = new List<PixelData>();

        // Read through Every pixel in the Pixel Map
        for (int y = 0; y < pixelMap.texture.height; y++)
        {
            for (int x = 0; x < pixelMap.texture.width; x++)
            {
                Vector2 position = new Vector2(x, y);
                UnityEngine.Color color = pixelMap.texture.GetPixel(x, y);

                // Add new pixel
                pixelDatas[y, x] = new PixelData();
                pixelDatas[y, x].Position = position;
                pixelDatas[y, x].PixelColor = color;

                // Make list of pixel color variations
                if (!tempColorVariation.Contains(pixelDatas[y, x].PixelColor))
                {
                    tempColorVariation.Add(pixelDatas[y, x].PixelColor);

                    PixelData pd = new PixelData();
                    pd.PixelColor = pixelDatas[y, x].PixelColor;

                    pixelVariations.Add(pd);
                }
            }
        }
    }

    /// <summary>
    /// Load tile from right directory
    /// </summary>
    private void GetTiles()
    {
        // Root Directory
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/Tiles");
        string[] paths = new string[di.GetDirectories().Length];

        string[] roadFiles = new string[0];
        string[] sidewalkFiles = new string[0];

        // Get the paths
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i] = di.GetDirectories()[i].Name;
            switch (paths[i])
            {
                case "Road":
                    roadFiles = Directory.GetFiles(di.FullName + "/" + paths[i] + "/", "*.asset");
                    roadTiles = new RoadTile[roadFiles.Length];
                    break;
                case "Sidewalk":
                    sidewalkFiles = Directory.GetFiles(di.FullName + "/" + paths[i] + "/", "*.asset");
                    sidewalkTiles = new SidewalkTile[sidewalkFiles.Length];
                    break;
                default:
                    break;
            }
        }

        for (int i = 0; i < roadFiles.Length; i++)
        {
            string file = Path.GetFileName(roadFiles[i]);
            roadTiles[i] = AssetDatabase.LoadAssetAtPath<RoadTile>("Assets/Tiles/Road/" + file);
        }

        for (int i = 0; i < sidewalkFiles.Length; i++)
        {
            string file = Path.GetFileName(sidewalkFiles[i]);
            sidewalkTiles[i] = AssetDatabase.LoadAssetAtPath<SidewalkTile>("Assets/Tiles/Sidewalk/" + file);
        }
    }

    private void PlaceTiles()
    {
        float offsetX = pixelMap.texture.width;
        GameObject map = new GameObject("Map");

        for (int y = 0; y < pixelMap.texture.height; y++)
        {
            offsetX--;
            for (int x = 0; x < pixelMap.texture.width; x++)
            {
                PixelData currentPixel = pixelDatas[y, x];

                for (int j = 0; j < pixelVariations.Count; j++)
                {
                    if (currentPixel.PixelColor == pixelVariations[j].PixelColor)
                    {
                        currentPixel.TileType = pixelVariations[j].TileType;
                    }
                }
                if (currentPixel.TileType != PixelData.Tile.Null)
                {
                    GameObject tileObject = new GameObject("Tile");

                    tileObject.AddComponent<TileObject>();
                    TileObject to = tileObject.GetComponent<TileObject>();

                    switch (currentPixel.TileType)
                    {
                        case PixelData.Tile.Road:
                            to.Tile = roadTiles[0];
                            tileObject.name = "Road";
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = sidewalkTiles[0];
                            tileObject.name = "Sidewalks";
                            break;
                        default:
                            break;
                    }
                    tileObject.transform.parent = map.transform;
                    tileObject.transform.position = currentPixel.Position;
                    tileObject.transform.position = new Vector2(tileObject.transform.position.x - offsetX * 0.5f, tileObject.transform.position.y * 0.5f);
                }
            }
        }
    }

    //set offset for every pixel row based on height of pixel
}
