using System.Collections.Generic;
using UnityEngine;

public static class DialogueEventLoader
{
    // ��� �̺�Ʈ�� Resources ���� ������ "DialogueEvents" �������� �ҷ��´ٰ� ����
    private static Dictionary<string, DialogueEvent> eventCache = new();

    /// <summary>
    /// ID�� �������� DialogueEvent�� �ҷ��ɴϴ�. ĳ�ð� ������ ĳ�ÿ��� �����ɴϴ�.
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
    /// ������ ĳ�� �ʱ�ȭ (�׽�Ʈ�� �Ǵ� �ʿ� �� ���� �ʱ�ȭ)
    /// </summary>
    public static void ClearCache()
    {
        eventCache.Clear();
    }
}