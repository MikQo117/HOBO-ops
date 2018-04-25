using UnityEngine;
using UnityEditor;

public static class EditorList
{
    public static void Show (SerializedProperty list, bool showListSize = true)
    {
        EditorGUILayout.PropertyField(list);
        EditorGUI.indentLevel += 1;
        if (list.isExpanded)
        {
            if(showListSize)
            {
                EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
            }
            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
        }
        EditorGUI.indentLevel -= 1;
    }

}
