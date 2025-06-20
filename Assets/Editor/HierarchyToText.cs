using UnityEngine;
using UnityEditor;
using System.Text;
using System.Collections.Generic;

public class HierarchyToText : EditorWindow
{
    [MenuItem("Tools/Export Hierarchy as Text")]
    public static void ShowWindow()
    {
        StringBuilder sb = new StringBuilder();

        // 1. 중복 제거를 위한 최상위 오브젝트만 필터링
        List<GameObject> rootsOnly = new List<GameObject>();
        foreach (var obj in Selection.gameObjects)
        {
            bool isChild = false;
            foreach (var other in Selection.gameObjects)
            {
                if (other == obj) continue;
                if (obj.transform.IsChildOf(other.transform))
                {
                    isChild = true;
                    break;
                }
            }
            if (!isChild)
                rootsOnly.Add(obj);
        }

        // 2. 계층 출력
        foreach (GameObject obj in rootsOnly)
        {
            PrintHierarchy(obj.transform, 0, sb);
        }

        // 3. 복사
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