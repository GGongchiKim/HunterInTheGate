using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투 이벤트 데이터를 ID로 로딩하는 로더 클래스
/// Resources/CombatEvents/{eventId}.asset 경로 기준으로 로드됨
/// </summary>
public static class CombatEventLoader
{
    private static Dictionary<string, CombatEventData> cache = new();

    /// <summary>
    /// 전투 이벤트 ID를 통해 CombatEventData를 로딩
    /// </summary>
    public static CombatEventData GetById(string eventId)
    {
        if (cache.TryGetValue(eventId, out var ce))
            return ce;

        var loaded = Resources.Load<CombatEventData>($"CombatEvents/{eventId}");
        if (loaded != null)
        {
            cache[eventId] = loaded;
            return loaded;
        }

        Debug.LogError($"[CombatEventLoader] CombatEvent with ID '{eventId}' not found.");
        return null;
    }

    public static void ClearCache()
    {
        cache.Clear();
    }
}