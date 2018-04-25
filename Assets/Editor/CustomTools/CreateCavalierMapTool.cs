using System.Drawing;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

// Made by Sipi Raussi 25.4.2018
public class CreateCavalierMapTool : EditorWindow
{
    private SerializedObject so;

    private Sprite pixelMap;
    // Store every pixel data
    private UnityEngine.Color[,]pixelColors;
    // Store all color variations
    private List<UnityEngine.Color> colorVariations;
    [SerializeField]
    private Texture2D[] colorVariationsTextures;

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

        //Input field
        pixelMap = (Sprite)EditorGUILayout.ObjectField("Pixel Map:", pixelMap, typeof(Sprite), false);

        if(pixelMap == null)
        {
            GUI.enabled = false;
        }
        else
        {
            GUI.enabled = true;

            EditorGUILayout.LabelField("Dimensions (W x H): " + pixelMap.texture.width.ToString() + "px " + "x " + pixelMap.texture.height.ToString() + "px");

        }

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Read Pixels", GUILayout.MaxWidth(100)))
        {
            ReadPixels();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (colorVariationsTextures != null && pixelMap != null)
        {
            SerializedProperty pixelPreview = so.FindProperty("colorVariationsTextures");
            so.Update();
            EditorGUILayout.PropertyField(pixelPreview, true);
            so.ApplyModifiedProperties();
        }

        GUI.enabled = false;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Create Map", GUILayout.MaxWidth(100)))
        {
            CreateCavalierMap();
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
        pixelColors = new UnityEngine.Color[pixelMap.texture.height, pixelMap.texture.width];
        colorVariations = new List<UnityEngine.Color>();

        for (int y = 0; y < pixelMap.texture.height; y++)
        {
            for (int x = 0; x < pixelMap.texture.width; x++)
            {
                pixelColors[y, x] = pixelMap.texture.GetPixel(x, y);

                if (!colorVariations.Contains(pixelColors[y, x]))
                {
                    colorVariations.Add(pixelColors[y, x]);
                }
            }
        }

        colorVariationsTextures = new Texture2D[colorVariations.Count];

        for (int i = 0; i < colorVariations.Count; i++)
        {
            // Color preview image
            colorVariationsTextures[i] = new Texture2D(64, 64);
            // Set images color
            for (int y = 0; y < colorVariationsTextures[i].height; y++)
            {
                for (int x = 0; x < colorVariationsTextures[i].width; x++)
                {
                    colorVariationsTextures[i].SetPixel(x, y, colorVariations[i]);
                }
            }
            colorVariationsTextures[i].Apply();
        }

    }

    //Define pixel colors to corresponding tile

    //Get tiles

    //set offset for every pixel row based on height of pixel
}
