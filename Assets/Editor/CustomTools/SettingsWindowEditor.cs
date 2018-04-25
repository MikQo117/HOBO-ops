using System.IO;

using UnityEngine;
using UnityEditor;

// Made by Sipi Raussi 25.4.2018
public class SettingsWindowsEditor : EditorWindow
{
    // Instance of settings
    private Settings settings;

    // Pixels per unit
    private int ppu;

    [MenuItem("Custom Tools/Settings")]
    private static void ShowWindow()
    {
        GetWindow<SettingsWindowsEditor>("Settings");
    }

    private void OnEnable()
    {
        settings = AssetDatabase.LoadAssetAtPath<Settings>("Assets/Settings/Settings.asset");
        ppu = settings.PixelsPerUnit;
    }

    private void OnGUI()
    {
        Utilitys.PrintHeader("Settings");

        // Show create button if there is no settings
        if (settings == null)
        { 
            if (GUILayout.Button("Create Settings"))
            {
                CreateSettings();
            }
        }
        // Give settings to modifie
        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Pixels Per Unit: ", GUILayout.MaxWidth(100));
            GUILayout.FlexibleSpace();
            ppu = EditorGUILayout.IntField(ppu, GUILayout.MaxWidth(40), GUILayout.MinWidth(20));
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Update Settings"))
            {
                UpdateSettings();
            }
        }
    }

    /// <summary>
    /// Create sttings file if it does not exist
    /// </summary>
    private void CreateSettings()
    {
        settings = CreateInstance<Settings>();

        // Create path if it does not exist
        string path = Application.dataPath + "/Settings/";
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // Create Settings asset
        AssetDatabase.CreateAsset(settings, "Assets/Settings/Settings.asset");
        AssetDatabase.SaveAssets();

        // Focus on project window and select Settings asset
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = settings;
    }

    /// <summary>
    /// Update settings
    /// </summary>
    private void UpdateSettings()
    {
        // Make settings serializable
        SerializedObject serializedSettings = new SerializedObject(settings);
        // Select proper to that you want to modifie
        SerializedProperty serializedPpu = serializedSettings.FindProperty("PixelsPerUnit");
        // Set Settings dirty, set values and apply changes
        serializedPpu.intValue = ppu;
        serializedSettings.ApplyModifiedProperties();
    }
}
