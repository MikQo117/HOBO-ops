using UnityEngine;
using UnityEditor;

// Made by Sipi Raussi 25.4.2018
[CustomEditor(typeof(GrassTile))]
public class GrassTileEditor : Editor
{
    int defaultFontSize;

    // Custom Inspector window for Tile ScriptableObjects
    public override void OnInspectorGUI()
    {
        GrassTile selectedTile = (GrassTile)target;

        Utilitys.PrintHeader("Grass Tile");

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        Texture2D myTexture = AssetPreview.GetAssetPreview(selectedTile.Sprite);
        GUILayout.Label(myTexture);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        selectedTile.GT = (GrassTile.GrassType)EditorGUILayout.EnumPopup("Type: ", selectedTile.GT);

        string path = "Assets/Sprites/Tiles/Grass/";

        switch (selectedTile.GT)
        {
            case GrassTile.GrassType.BottomLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "GrassBL.png");
                break;
            case GrassTile.GrassType.BottomRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "GrassBR.png");
                break;
            case GrassTile.GrassType.CornerBottomLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "GrassCBL.png");
                break;
            case GrassTile.GrassType.CornerBottomRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "GrassCBR.png");
                break;
            case GrassTile.GrassType.CornerTopLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "GrassCTL.png");
                break;
            case GrassTile.GrassType.CornerTopRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "GrassCTR.png");
                break;
            case GrassTile.GrassType.Empty:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "GrassEmpty.png");
                break;
            case GrassTile.GrassType.HorizontalBottom:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "GrassHB.png");
                break;
            case GrassTile.GrassType.HorizontalTop:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "GrassHT.png");
                break;
            case GrassTile.GrassType.TopLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "GrassTL.png");
                break;
            case GrassTile.GrassType.TopRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "GrassTR.png");
                break;
            case GrassTile.GrassType.VerticalLeft:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "GrassVL.png");
                break;
            case GrassTile.GrassType.VerticalRight:
                selectedTile.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + "GrassVR.png");
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
