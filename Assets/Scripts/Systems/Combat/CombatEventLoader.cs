using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �̺�Ʈ �����͸� ID�� �ε��ϴ� �δ� Ŭ����
/// Resources/CombatEvents/{eventId}.asset ��� �������� �ε��
/// </summary>
public static class CombatEventLoader
{
    private static Dictionary<string, CombatEventData> cache = new();

    /// <summary>
    /// ���� �̺�Ʈ ID�� ���� CombatEventData�� �ε�
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