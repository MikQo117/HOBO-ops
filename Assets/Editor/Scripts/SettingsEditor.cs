using UnityEditor;

// Made by Sipi Raussi 25.4.2018
[CustomEditor(typeof(Settings))]
public class SettingsEditor : Editor
{
    //Prevent user to modifie setting from file it self
    public override void OnInspectorGUI()
    {
        Settings selectedSettings = (Settings)target;

        EditorGUILayout.LabelField("Pixel Per Unit: " + selectedSettings.PixelsPerUnit);
    }
}
