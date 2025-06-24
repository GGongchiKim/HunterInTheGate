using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ʃ�丮�� �ܰ迡�� Ư�� UI ��ҿ� ���� ȿ��(�����̴� �ܰ���)�� �����ϴ� ���� Ŭ����
/// </summary>
public static class TutorialHighlighter
{
    private static readonly List<UIHighlighter> activeHighlights = new();


    /// <summary>
    /// ���ڿ� ID�� ���� Ư�� UI�� �����մϴ�. Ʃ�丮�� ��翡�� targetId�� ������ �� ó����.
    /// </summary>
    public static void HighlightByTargetId(string targetId)
    {
        switch (targetId)
        {
            case "HandArea":
                HighlightHandArea();
                break;
            case "TurnEnd":
                HighlightTurnEndButton();
                break;
            case "EnemyIntent":
                HighlightFirstEnemyIntent();
                break;
            default:
                // ī�� ID�� ����
                HighlightCardById(targetId);
                break;
        }
    }


    /// <summary>
    /// ù ��° ���� Intent UI�� �����մϴ�.
    /// </summary>
    public static void HighlightFirstEnemyIntent()
    {
        var allEnemies = CombatContext.Instance?.allEnemies;
        if (allEnemies == null || allEnemies.Count == 0)
        {
            Debug.LogWarning("[TutorialHighlighter] CombatContext.Instance.allEnemies is empty");
            return;
        }

        var targetEnemy = allEnemies[0]; // Ʃ�丮�󿡼� ù ��° ���� ������� ��

        var handlers = GameObject.FindObjectsOfType<EnemyHUDHandler>();
        foreach (var handler in handlers)
        {
            if (handler.GetLinkedEnemy() == targetEnemy)
            {
                var intentUI = handler.GetIntentUI();
                if (intentUI == null)
                {
                    Debug.LogWarning("[TutorialHighlighter] intentUI is null in matched EnemyHUDHandler");
                    return;
                }

                var highlighter = intentUI.GetComponentInChildren<UIHighlighter>(true);
                if (highlighter != null)
                {
                    highlighter.StartBlinking();
                    activeHighlights.Add(highlighter);
                }
                else
                {
                    Debug.LogWarning($"[TutorialHighlighter] UIHighlighter not found in {intentUI.name}");
                }

                return; // ù ��° ��Ī�� ����
            }
        }

        Debug.LogWarning("[TutorialHighlighter] No EnemyHUDHandler matched the first enemy.");
    }




    /// <summary>
    /// Ư�� ī�� ID�� ���� ī�� UI�� ���� ȿ�� ����
    /// </summary>
    public static void HighlightCardById(string cardId)
    {
        foreach (var card in GameObject.FindObjectsOfType<CardUI>())
        {
            if (card.GetCardData()?.cardId == cardId)
            {
                var highlighter = card.GetComponent<UIHighlighter>();
                if (highlighter != null)
                {
                    highlighter.StartBlinking();
                    activeHighlights.Add(highlighter);
                }
            }
        }
    }

    /// <summary>
    /// ��ü ���� ī�� ������ ���� (��: HandArea)
    /// </summary>
    public static void HighlightHandArea()
    {
        GameObject handArea = GameObject.FindWithTag("HandArea");
        if (handArea != null)
        {
            var highlighter = handArea.GetComponent<UIHighlighter>();
            if (highlighter != null)
            {
                highlighter.StartBlinking();
                activeHighlights.Add(highlighter);
            }
        }
    }

    /// <summary>
    /// �� ���� ��ư�� ����
    /// </summary>
    public static void HighlightTurnEndButton()
    {
        var button = C_HUDManager.Instance?.TurnEndbutton;
        if (button != null)
        {
            var highlighter = button.GetComponent<UIHighlighter>();
            if (highlighter != null)
            {
                highlighter.StartBlinking();
                activeHighlights.Add(highlighter);
            }
        }
    }


    /// <summary>
    /// ���� ���� ���� ��� ���� ȿ���� ����
    /// </summary>
    public static void ClearAllHighlights()
    {
        foreach (var highlighter in activeHighlights)
        {
            if (highlighter != null)
                highlighter.StopBlinking();
        }
        activeHighlights.Clear();
    }
}
