using System.Collections.Generic;
using UnityEngine;

public static class DialogueEventLoader
{
    // 모든 이벤트를 Resources 폴더 하위의 "DialogueEvents" 폴더에서 불러온다고 가정
    private static Dictionary<string, DialogueEvent> eventCache = new();

    /// <summary>
    /// ID를 기준으로 DialogueEvent를 불러옵니다. 캐시가 있으면 캐시에서 가져옵니다.
    /// </summary>
    public static DialogueEvent LoadEventById(string eventId)
    {
        if (eventCache.ContainsKey(eventId))
        {
            return eventCache[eventId];
        }

        DialogueEvent loadedEvent = Resources.Load<DialogueEvent>($"DialogueEvents/{eventId}");

        if (loadedEvent != null)
        {
            eventCache[eventId] = loadedEvent;
            return loadedEvent;
        }

        Debug.LogError($"DialogueEvent with ID '{eventId}' not found in Resources/DialogueEvents.");
        return null;
    }

    /// <summary>
    /// 강제로 캐시 초기화 (테스트용 또는 필요 시 수동 초기화)
    /// </summary>
    public static void ClearCache()
    {
        eventCache.Clear();
    }
}