using UnityEngine;
using UnityEditor;
using System.Text;

public class HierarchyToText : EditorWindow
{
    [MenuItem("Tools/Export Hierarchy as Text")]
    public static void ShowWindow()
    {
        StringBuilder sb = new StringBuilder();

        foreach (GameObject obj in Selection.gameObjects)
        {
            PrintHierarchy(obj.transform, 0, sb);
        }

        string output = sb.ToString();
        EditorGUIUtility.systemCopyBuffer = output;
        Debug.Log("Hierarchy copied to clipboard:\n" + output);
    }

    private static void PrintHierarchy(Transform t, int indent, StringBuilder sb)
    {
        sb.AppendLine($"{new string(' ', indent * 2)}└── {t.name}");
        foreach (Transform child in t)
        {
            PrintHierarchy(child, indent + 1, sb);
        }
    }
}