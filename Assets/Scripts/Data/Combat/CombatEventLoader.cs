using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatEventLoader
{
    private static Dictionary<string, CombatEvent> cache = new();

    public static CombatEvent LoadEventById(string eventId)
    {
        if (cache.TryGetValue(eventId, out var ce)) return ce;

        var loaded = Resources.Load<CombatEvent>($"CombatEvents/{eventId}");
        if (loaded != null)
        {
            cache[eventId] = loaded;
            return loaded;
        }

        Debug.LogError($"CombatEvent with ID '{eventId}' not found.");
        return null;
    }

    public static void ClearCache()
    {
        cache.Clear();
    }
}