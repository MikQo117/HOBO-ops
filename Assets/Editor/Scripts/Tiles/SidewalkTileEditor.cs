using UnityEngine;
using UnityEditor;

// Made by Sipi Raussi 25.4.2018
[CustomEditor(typeof(SidewalkTile))]
public class SideWalkTileEditor : Editor
{
    // Custom Inspector window for Tile ScriptableObjects
    public override void OnInspectorGUI()
    {
        SidewalkTile selectedTile = (SidewalkTile)target;

        Utilitys.PrintHeader("Sidewalk Tile");

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        Texture2D myTexture = AssetPreview.GetAssetPreview(selectedTile.Sprite);
        GUILayout.Label(myTexture);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        selectedTile.ST = (SidewalkTile.SidewalkType)EditorGUILayout.EnumPopup("Type: ", selectedTile.ST);

        string path = "Assets/Sprites/Tiles/Sidewalk/";

        switch (selectedTile.ST)
        {
            case SidewalkTile.SidewalkType.Horizontal:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "SidewalkH.png");
                break;
            case SidewalkTile.SidewalkType.InnerBottomLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "SidewalkIBL.png");
                break;
            case SidewalkTile.SidewalkType.InnerBottomRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "SidewalkIBR.png");
                break;
            case SidewalkTile.SidewalkType.InnerTopLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "SidewalkITL.png");
                break;
            case SidewalkTile.SidewalkType.InnerTopRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "SidewalkITR.png");
                break;
            case SidewalkTile.SidewalkType.OuterBottomLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "SidewalkOBL.png");
                break;
            case SidewalkTile.SidewalkType.OuterBottomRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "SidewalkOBR.png");
                break;
            case SidewalkTile.SidewalkType.OuterTopLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "SidewalkOTL.png");
                break;
            case SidewalkTile.SidewalkType.Vertical:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "SidewalkV.png");
                break;
            default:
                selectedTile.Sprite = null;
                break;
        }

        if (GUI.changed)
        {
            Undo.RecordObject(selectedTile, "Tile Editor Modify");
            EditorUtility.SetDirty(selectedTile);
        }
    }
}
