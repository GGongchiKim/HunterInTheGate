using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ʃ�丮�󿡼� ������ �� �ִ� UI ������Ʈ���� ����صΰ�, ���ڿ� ID�� ������ �� �ֵ��� ���ִ� ���� ���
/// </summary>
public class TutorialUIRegistry : MonoBehaviour
{
    public static TutorialUIRegistry Instance { get; private set; }

    [System.Serializable]
    public class UIEntry
    {
        public string id;                // highlightTargetId (��: "TurnEndButton")
        public GameObject targetObject; // ���� UI ������Ʈ
    }

    [SerializeField]
    private List<UIEntry> uiEntries = new List<UIEntry>();

    private Dictionary<string, GameObject> uiMap = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // ���� �ʱ�ȭ
        foreach (var entry in uiEntries)
        {
            if (!uiMap.ContainsKey(entry.id) && entry.targetObject != null)
            {
                uiMap.Add(entry.id, entry.targetObject);
            }
        }
    }

    public GameObject GetTargetById(string id)
    {
        if (uiMap.TryGetValue(id, out var obj))
        {
            return obj;
        }

        Debug.LogWarning($"[TutorialUIRegistry] Unknown highlightTargetId: {id}");
        return null;
    }
}