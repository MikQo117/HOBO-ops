using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using UnityEditor;
using UnityEngine;

// Made by Sipi Raussi 25.4.2018
public class CreateCavalierMapTool : EditorWindow
{
    // Pixel unit text
    private const string pixelUnit = "px";

    // Serialized object of this class
    private SerializedObject so;

    // Users pixel map
    private Sprite pixelMap;

    // Store all pixel data
    private PixelData[,] pixelDatas;
    [SerializeField]
    private List<PixelData> pixelVariations;

    // Tile information
    private SidewalkTile[] sidewalkTiles;
    private RoadTile[] roadTiles;
    private GrassTile[] grassTiles;

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

        // Enable GUI if there is pixel map
        pixelMap = (Sprite)EditorGUILayout.ObjectField("Pixel Map:", pixelMap, typeof(Sprite), false);
        if (pixelMap == null)
        {
            GUI.enabled = false;
        }
        else
        {
            GUI.enabled = true;

            EditorGUILayout.LabelField("Dimensions (W x H): " + pixelMap.texture.width.ToString() + pixelUnit + "x " + pixelMap.texture.height.ToString() + pixelUnit);
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

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Create map", GUILayout.MaxWidth(100)))
        {
            CreateCavalierMap();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Create map
    /// </summary>
    private void CreateCavalierMap()
    {
        FindMapFromScene();
        GetTiles();
        SetTileData();
        UpdateTiles();
    }

    /// <summary>
    /// Read pixel map information
    /// </summary>
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
                pixelDatas[y, x] = new PixelData
                {
                    Position = position,
                    PixelColor = color
                };

                // Make list of pixel color variations
                if (!tempColorVariation.Contains(pixelDatas[y, x].PixelColor))
                {
                    tempColorVariation.Add(pixelDatas[y, x].PixelColor);

                    PixelData pd = new PixelData
                    {
                        PixelColor = pixelDatas[y, x].PixelColor
                    };

                    pixelVariations.Add(pd);
                }
            }
        }
    }

    private void FindMapFromScene()
    {
        GameObject map = GameObject.Find("Map");
        if (map)
        {
            DestroyImmediate(map);
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

        // Tile file paths
        string[] roadFiles = new string[0];
        string[] sidewalkFiles = new string[0];
        string[] grassFiles = new string[0];

        // Get the paths of files that has *.asset file extension
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
                case "Grass":
                    grassFiles = Directory.GetFiles(di.FullName + "/" + paths[i] + "/", "*.asset");
                    grassTiles = new GrassTile[grassFiles.Length];
                    break;
                default:
                    break;
            }
        }

        // Add files
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

        for (int i = 0; i < grassFiles.Length; i++)
        {
            string file = Path.GetFileName(grassFiles[i]);
            grassTiles[i] = AssetDatabase.LoadAssetAtPath<GrassTile>("Assets/Tiles/Grass/" + file);
        }
    }

    /// <summary>
    /// Place all tiles to the scene
    /// </summary>
    private void SetTileData()
    {
        // Offset for X axis
        float offsetX = pixelMap.texture.width;

        // Start at top left corner
        for (int y = 0; y < pixelMap.texture.height; y++)
        {
            // When row changes reduce offset by 1
            offsetX--;
            for (int x = 0; x < pixelMap.texture.width; x++)
            {
                // Get current pixel
                PixelData currentPixel = pixelDatas[y, x];

                // Set type for a tile based on its pixel color
                for (int i = 0; i < pixelVariations.Count; i++)
                {
                    if (currentPixel.PixelColor == pixelVariations[i].PixelColor)
                    {
                        currentPixel.TileType = pixelVariations[i].TileType;
                        break;
                    }
                }
            }
        }
    }

    private void UpdateTiles()
    {
        // Offset for X axis
        float offsetX = pixelMap.texture.width;

        GameObject map = new GameObject("Map");
        for (int y = 0; y < pixelMap.texture.height; y++)
        {
            offsetX--;
            for (int x = 0; x < pixelMap.texture.width; x++)
            {
                PixelData currentPixel = pixelDatas[y, x];

                byte bitMask = (byte)CalculateBitMask(x, y, currentPixel);

                GameObject tileObject = new GameObject("Tile");

                TileObject to = tileObject.AddComponent<TileObject>();

                // create tile type based on bit mask

                if ((bitMask & 231) == 231)             // 1 1 1
                {                                       // 0 x 0 = 231
                    switch (currentPixel.TileType)      // 1 1 1
                    {
                        case PixelData.Tile.Road:
                            Debug.LogError("Not possible for Road tile!");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkH");
                            break;
                        case PixelData.Tile.Grass:
                            Debug.LogError("Not possible for Grass tile!");
                            break;
                        case PixelData.Tile.ParkingLot:
                            Debug.LogError("Not possible for Parking Lot tile");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 189) == 189)        // 1 0 1
                {                                       // 1 x 1 = 189
                    switch (currentPixel.TileType)      // 1 0 1
                    {
                        case PixelData.Tile.Road:
                            Debug.LogError("Not possible for Road tile!");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkV");
                            break;
                        case PixelData.Tile.Grass:
                            Debug.LogError("Not possible for Grass tile!");
                            break;
                        case PixelData.Tile.ParkingLot:
                            Debug.LogError("Not possible for Parking Lot tile");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 244) == 244)        // 1 0 0
                {                                       // 1 x 0 = 244
                    switch (currentPixel.TileType)      // 1 1 1
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadOBL");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkOBL");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassBL");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadOBL");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 233) == 233)        // 0 0 1
                {                                       // 0 x 1 = 233
                    switch (currentPixel.TileType)      // 1 1 1
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadOBR");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkV");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassBR");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadOBR");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 151) == 151)        // 1 1 1
                {                                       // 1 x 0 = 151
                    switch (currentPixel.TileType)      // 1 0 0
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadOTL");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkOTL");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassTL");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadOTL");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 47) == 47)          // 1 1 1
                {                                       // 0 x 1 = 47
                    switch (currentPixel.TileType)      // 0 0 1
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadOTR");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkIBL");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassTR");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadOTR");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 224) == 224)        // 0 0 0            
                {                                       // 0 x 0 = 244
                    switch (currentPixel.TileType)      // 1 1 1
                    {
                        case PixelData.Tile.Road:

                                to.Tile = FindTileByName(roadTiles, "RoadHB");
                            
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkH");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassHB");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadHBCR");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 7) == 7)            // 1 1 1            
                {                                       // 0 x 0 = 7
                    switch (currentPixel.TileType)      // 0 0 0
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadHT");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkH");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassHT");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadHT");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 148) == 148)        // 1 0 0            
                {                                       // 1 x 0 = 148
                    switch (currentPixel.TileType)      // 1 0 0
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadVL");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkV");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassVL");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadVL");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 41) == 41)          // 0 0 1            
                {                                       // 0 x 1 = 41
                    switch (currentPixel.TileType)      // 0 0 1
                    {
                        case PixelData.Tile.Road:
                            if ((bitMask & 2) == 2)
                            {
                                to.Tile = FindTileByName(roadTiles, "RoadOTR");
                            }
                            else if ((bitMask & 64) == 64)
                            {
                                to.Tile = FindTileByName(roadTiles, "RoadOBR");
                            }
                            else
                            {
                                to.Tile = FindTileByName(roadTiles, "RoadVR");
                            }
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkV");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassVR");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadVRCR");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 192) == 192)        // 0 0 0            
                {                                       // 0 x 0 = 192
                    switch (currentPixel.TileType)      // 1 1 0
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadHBL");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkH");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassHB");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadHBCR");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 96) == 96)          // 0 0 0            
                {                                       // 0 x 0 = 96
                    switch (currentPixel.TileType)      // 0 1 1
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadHB");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkH");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassHB");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadHBCR");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 40) == 40)          // 0 0 0
                {                                       // 0 x 1 = 40
                    switch (currentPixel.TileType)      // 0 0 1
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadVBR");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkV");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassVR");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadVRCR");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 9) == 9)            // 0 0 1
                {                                       // 0 x 1 = 9
                    switch (currentPixel.TileType)      // 0 0 0
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadVTR");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkV");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassVR");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadVRCR");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 3) == 3)            // 0 1 1
                {                                       // 0 x 0 = 3
                    switch (currentPixel.TileType)      // 0 0 0
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadHTR");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkH");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassHT");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadHTR");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 6) == 6)            // 1 1 0
                {                                       // 0 x 0 = 6
                    switch (currentPixel.TileType)      // 0 0 0
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadHT");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkH");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassHT");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadHT");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 20) == 20)          // 1 0 0            
                {                                       // 1 x 0 = 20
                    switch (currentPixel.TileType)      // 0 0 0
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadVL");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkV");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassVL");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadVL");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 144) == 144)        // 0 0 0            
                {                                       // 1 x 0 = 144
                    switch (currentPixel.TileType)      // 1 0 0
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadVBL");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkV");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassVL");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadVL");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 128) == 128)        // 0 0 0
                {                                       // 0 x 0 = 128
                    switch (currentPixel.TileType)      // 1 0 0
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadEmpty");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkIBL");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassCBL");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadEmpty");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 32) == 32)          // 0 0 0
                {                                       // 0 x 0 = 32
                    switch (currentPixel.TileType)      // 0 0 1
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadEmpty");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkIBR");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassCBR");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadEmpty");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 1) == 1)            // 0 0 1
                {                                       // 0 x 0 = 1
                    switch (currentPixel.TileType)      // 0 0 0
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadEmpty");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkITR");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassCTR");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadEmpty");
                            break;
                        default:
                            break;
                    }
                }
                else if ((bitMask & 4) == 4)            // 1 0 0
                {                                       // 0 x 0 = 4
                    switch (currentPixel.TileType)      // 0 0 0
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadITL");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkITL");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassCTL");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadITL");
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (currentPixel.TileType)
                    {
                        case PixelData.Tile.Road:
                            to.Tile = FindTileByName(roadTiles, "RoadEmpty");
                            break;
                        case PixelData.Tile.Sidewalk:
                            to.Tile = FindTileByName(sidewalkTiles, "SidewalkH");
                            break;
                        case PixelData.Tile.Grass:
                            to.Tile = FindTileByName(grassTiles, "GrassEmpty");
                            break;
                        case PixelData.Tile.ParkingLot:
                            to.Tile = FindTileByName(roadTiles, "RoadEmpty");
                            break;
                        default:
                            break;
                    }
                }


                tileObject.transform.parent = map.transform;
                tileObject.transform.position = new Vector2(currentPixel.Position.x - offsetX * 0.5f, currentPixel.Position.y * 0.5f);
            }
        }
    }

    /// <summary>
    /// Finds tile from array list
    /// </summary>
    /// <param name="tileList">List where to search</param>
    /// <param name="tileName">Name of the tile</param>
    /// <returns></returns>
    private Tile FindTileByName(Tile[] tileList, string tileName)
    {
        for (int i = 0; i < tileList.Length; i++)
        {
            if (tileList[i].name == tileName)
            {
                return tileList[i];
            }
        }
        Debug.LogError("Couldn't find " + tileName + " from " + tileList.ToString() + "!");
        return null;
    }

    private int CalculateBitMask(int x, int y, PixelData currentPixel)
    {
        int bitMask = 0;
        // Left top corner
        if (y == 0 && x == 0)
        {
            bitMask = 244;

            if (pixelDatas[y, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 8;
            }
            if (pixelDatas[y + 1, x].TileType != currentPixel.TileType)
            {
                bitMask |= 2;
            }
            if (pixelDatas[y + 1, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 1;
            }

            return bitMask;
        }
        // Right top corner
        else if (y == 0 && x == pixelMap.texture.width - 1)
        {
            bitMask = 233;
            if (pixelDatas[y, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 16;
            }
            if (pixelDatas[y + 1, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 4;
            }
            if (pixelDatas[y + 1, x].TileType != currentPixel.TileType)
            {
                bitMask |= 2;
            }
            return bitMask;
        }
        // Left bottom corner
        else if (y == pixelMap.texture.height - 1 && x == 0)
        {
            bitMask = 151;
            if (pixelDatas[y - 1, x].TileType != currentPixel.TileType)
            {
                bitMask |= 64;
            }
            if (pixelDatas[y - 1, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 32;
            }
            if (pixelDatas[y, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 8;
            }
            return bitMask;
        }
        // Right bottom corner
        else if (y == pixelMap.texture.height - 1 && x == pixelMap.texture.width - 1)
        {
            bitMask = 47;
            if (pixelDatas[y - 1, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 128;
            }
            if (pixelDatas[y - 1, x].TileType != currentPixel.TileType)
            {
                bitMask |= 64;
            }
            if (pixelDatas[y, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 16;
            }
            return bitMask;
        }
        // Top bound
        else if (y == 0 && x < pixelMap.texture.width - 1)
        {
            bitMask = 224;
            if (pixelDatas[y, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 16;
            }
            if (pixelDatas[y, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 8;
            }
            if (pixelDatas[y + 1, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 4;
            }
            if (pixelDatas[y + 1, x].TileType != currentPixel.TileType)
            {
                bitMask |= 2;
            }
            if (pixelDatas[y + 1, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 1;
            }
            return bitMask;
        }
        // Bottom bound
        else if (y == pixelMap.texture.height - 1 && x < pixelMap.texture.width - 1)
        {
            bitMask = 7;
            if (pixelDatas[y - 1, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 128;
            }
            if (pixelDatas[y - 1, x].TileType != currentPixel.TileType)
            {
                bitMask |= 64;
            }
            if (pixelDatas[y - 1, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 32;
            }
            if (pixelDatas[y, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 16;
            }
            if (pixelDatas[y, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 8;
            }
            return bitMask;
        }
        // Left bound
        else if (x == 0 && y < pixelMap.texture.height - 1)
        {
            bitMask = 148;
            if (pixelDatas[y - 1, x].TileType != currentPixel.TileType)
            {
                bitMask |= 64;
            }
            if (pixelDatas[y - 1, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 32;
            }
            if (pixelDatas[y, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 8;
            }
            if (pixelDatas[y + 1, x].TileType != currentPixel.TileType)
            {
                bitMask |= 2;
            }
            if (pixelDatas[y + 1, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 1;
            }
            return bitMask;
        }
        // Right bound
        else if (x == pixelMap.texture.width - 1 && y < pixelMap.texture.height - 1)
        {
            bitMask = 41;
            if (pixelDatas[y - 1, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 128;
            }
            if (pixelDatas[y - 1, x].TileType != currentPixel.TileType)
            {
                bitMask |= 64;
            }
            if (pixelDatas[y, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 16;
            }
            if (pixelDatas[y + 1, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 4;
            }
            if (pixelDatas[y + 1, x].TileType != currentPixel.TileType)
            {
                bitMask |= 2;
            }
            return bitMask;
        }
        // Check inner pixels
        else
        {
            if (pixelDatas[y - 1, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 128;
            }
            if (pixelDatas[y - 1, x].TileType != currentPixel.TileType)
            {
                bitMask |= 64;
            }
            if (pixelDatas[y - 1, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 32;
            }
            if (pixelDatas[y, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 16;
            }
            if (pixelDatas[y, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 8;
            }
            if (pixelDatas[y + 1, x - 1].TileType != currentPixel.TileType)
            {
                bitMask |= 4;
            }
            if (pixelDatas[y + 1, x].TileType != currentPixel.TileType)
            {
                bitMask |= 2;
            }
            if (pixelDatas[y + 1, x + 1].TileType != currentPixel.TileType)
            {
                bitMask |= 1;
            }
            return bitMask;
        }
    }
}
