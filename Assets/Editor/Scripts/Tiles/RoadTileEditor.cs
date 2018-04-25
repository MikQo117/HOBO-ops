using UnityEngine;
using UnityEditor;

// Made by Sipi Raussi 25.4.2018
[CustomEditor(typeof(RoadTile))]
public class RoadTileEditor : Editor
{
    int defaultFontSize;

    // Custom Inspector window for Tile ScriptableObjects
    public override void OnInspectorGUI()
    {
        RoadTile selectedTile = (RoadTile)target;

        Utilitys.PrintHeader("Road Tile");

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        Texture2D myTexture = AssetPreview.GetAssetPreview(selectedTile.Sprite);
        GUILayout.Label(myTexture);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        selectedTile.RT = (RoadTile.RoadType)EditorGUILayout.EnumPopup("Type: ", selectedTile.RT);

        string path = "Assets/Sprites/Tiles/Road/";

        switch (selectedTile.RT)
        {
            case RoadTile.RoadType.Empty:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadEmpty.png");
                break;
            case RoadTile.RoadType.HorizontalBottom:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadHB.png");
                break;
            case RoadTile.RoadType.HorizontalBottomLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadHBL.png");
                break;
            case RoadTile.RoadType.HorizontalTop:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadHT.png");
                break;
            case RoadTile.RoadType.HorizontalTopRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadHTR.png");
                break;
            case RoadTile.RoadType.InnerTopLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadITL.png");
                break;
            case RoadTile.RoadType.OuterBottomLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadOBL.png");
                break;
            case RoadTile.RoadType.OuterBottomRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadOBR.png");
                break;
            case RoadTile.RoadType.OuterTopLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadOTL.png");
                break;
            case RoadTile.RoadType.OuterTopRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadOTR.png");
                break;
            case RoadTile.RoadType.VerticalBottomLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadVBL.png");
                break;
            case RoadTile.RoadType.VerticalBottomRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadVBR.png");
                break;
            case RoadTile.RoadType.VerticalLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadVL.png");
                break;
            case RoadTile.RoadType.VerticalRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadVR.png");
                break;
            case RoadTile.RoadType.VerticalTopRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "RoadVTR.png");
                break;
            default:
                selectedTile.Sprite = null;
                break;
        }

        GUILayout.EndVertical();

        if (GUI.changed)
        {
            Undo.RecordObject(selectedTile, "Tile Editor Modify");
            EditorUtility.SetDirty(selectedTile);
        }
    }
}
