using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PixelData))]
public class PixelDataDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return Screen.width < 333 ? (16f + 18f) : 16f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int oldIndentLevel = EditorGUI.indentLevel;
        label = EditorGUI.BeginProperty(position, label, property);

        // Contents position
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);

        if(position.height > 16f)
        {
            position.height = 16f;
            EditorGUI.indentLevel += 1;
            contentPosition = EditorGUI.IndentedRect(position);
            contentPosition.y += 18f;
        }

        // Color takes 1/4 of space
        contentPosition.width *= 0.25f;
        EditorGUI.indentLevel = 0;
        EditorGUIUtility.labelWidth = 14f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("PixelColor"), new GUIContent("C"));

        // Tile takes 3/4 of space
        contentPosition.x += contentPosition.width;
        contentPosition.width *= 3f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("TileType"), GUIContent.none);

        EditorGUI.EndProperty();
        EditorGUI.indentLevel = oldIndentLevel;
    }
}
