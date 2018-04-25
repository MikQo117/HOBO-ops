using System.Drawing;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

// Made by Sipi Raussi 25.4.2018
public class CreateCavalierMapTool : EditorWindow
{
    private SerializedObject so;

    private Sprite pixelMap;
    // Store all pixel data
    private PixelData[] pixelDatas;
    [SerializeField]
    private List<PixelData> pixelVariations;

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
        if(pixelMap == null)
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

        // Create Map
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
        pixelDatas = new PixelData[pixelMap.texture.height * pixelMap.texture.width];

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
                pixelDatas[(y + 1) * x] = new PixelData();
                pixelDatas[(y + 1) * x].Position = position;
                pixelDatas[(y + 1) * x].PixelColor = color;

                // Make list of pixel color variations
                if(!tempColorVariation.Contains(pixelDatas[(y + 1) * x].PixelColor))
                {
                    tempColorVariation.Add(pixelDatas[(y + 1) * x].PixelColor);

                    PixelData pd = new PixelData();
                    pd.PixelColor = pixelDatas[(y + 1) * x].PixelColor;

                    pixelVariations.Add(pd);
                }
            }
        }
    }


    //Get tiles

    //set offset for every pixel row based on height of pixel
}
