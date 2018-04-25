using System.IO;

using UnityEditor;
using UnityEngine;

// Made by Sipi Raussi 25.4.2018
// Make create command in Assets/Create/Tile for creating tiles
public class MakeTile
{
    [MenuItem("Assets/Create/Tile/Road")]
    public static void CreateRoadTile()
    {
        RoadTile tile = ScriptableObject.CreateInstance<RoadTile>();

        if (!Selection.activeObject)
        {
            AssetDatabase.CreateAsset(tile, "Assets/NewRoadTile.asset");
        }
        else
        {
            AssetDatabase.CreateAsset(tile, GetFolderPath() + "/NewRoadTile.asset");
        }
        AssetDatabase.SaveAssets();

        // Select created tile
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = tile;
    }

    [MenuItem("Assets/Create/Tile/Sidewalk")]
    public static void CreateSidewalkTile()
    {
        SidewalkTile tile = ScriptableObject.CreateInstance<SidewalkTile>();

        if (!Selection.activeObject)
        {
            AssetDatabase.CreateAsset(tile, "Assets/NewSidewalkTile.asset");
        }
        else
        {
            AssetDatabase.CreateAsset(tile, GetFolderPath() + "/NewSidewalkTile.asset");
        }
        AssetDatabase.SaveAssets();

        // Select created tile
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = tile;
    }

    public static string GetFolderPath()
    {
        string path = "Assets";

        foreach (Object obj in Selection.GetFiltered<Object>(SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if(!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }
        }

        return path;
    }
}