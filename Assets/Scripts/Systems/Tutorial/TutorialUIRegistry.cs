using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 튜토리얼에서 강조할 수 있는 UI 오브젝트들을 등록해두고, 문자열 ID로 접근할 수 있도록 해주는 매핑 허브
/// </summary>
public class TutorialUIRegistry : MonoBehaviour
{
    public static TutorialUIRegistry Instance { get; private set; }

    [System.Serializable]
    public class UIEntry
    {
        public string id;                // highlightTargetId (예: "TurnEndButton")
        public GameObject targetObject; // 실제 UI 오브젝트
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

        // 사전 초기화
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