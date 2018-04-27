using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

public static class Utilitys
{
    public static void PrintHeader(string header)
    {
        int defaultFontSize;
        Font defaultFont = GUI.skin.label.font;

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        defaultFontSize = GUI.skin.label.fontSize;
        GUI.skin.label.font = AssetDatabase.LoadAssetAtPath<Font>("Asset/Fonts/PermanentMarker-Regular.tff");
        GUI.skin.label.fontSize = 25;
        GUILayout.Label(header);
        GUI.skin.label.font = defaultFont;
        GUI.skin.label.fontSize = defaultFontSize;

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }
}
