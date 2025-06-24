using UnityEngine;

/// <summary>
/// Ʃ�丮�� ���� �� ī�� ��� ����, UI ����, ��� �帧 ������ �����մϴ�.
/// </summary>
public static class TutorialCombatHelper
{
    private static string allowedCardId = null;
    private static bool requireCardUseToProceed = false;
    private static System.Action onTutorialStepCompleted;

    /// <summary>
    /// ���� ī�� ����� ���ѵ� �������� �ܺο��� Ȯ��
    /// </summary>
    public static bool RequireCardUseToProceed => requireCardUseToProceed;

    /// <summary>
    /// ���� ī�尡 ��� ������ ī������ �Ǵ�
    /// </summary>
    public static bool IsCardUsable(string cardId)
    {
        return string.IsNullOrEmpty(allowedCardId) || cardId == allowedCardId;
    }

    /// <summary>
    /// ���� ī�尡 Ʃ�丮�� ���� ������ �������� �Ǵ�
    /// </summary>
    public static bool IsCardCurrentlyRestricted(string cardId)
    {
        return !string.IsNullOrEmpty(allowedCardId) && cardId != allowedCardId;
    }

    /// <summary>
    /// Ư�� ī�常 ����ϵ��� ����
    /// </summary>
    public static void AllowOnlyCard(string cardId)
    {
        allowedCardId = cardId;
        requireCardUseToProceed = true;
    }

    /// <summary>
    /// Ʃ�丮�� �帧 �ݹ� ���
    /// </summary>
    public static void RegisterOnStepComplete(System.Action callback)
    {
        onTutorialStepCompleted = callback;
    }

    /// <summary>
    /// ī�尡 ���Ǿ��� �� ȣ���
    /// </summary>
    public static void OnCardUsed(string usedCardId)
    {
        if (!requireCardUseToProceed) return;

        if (usedCardId == allowedCardId)
        {
            Debug.Log($"[Tutorial] ���� ī��({usedCardId}) ���� �� ���� ��� �ڵ� ����");
            TryInvokeStepComplete();
        }
    }

    /// <summary>
    /// �ݹ� ���� �� ���� �ʱ�ȭ
    /// </summary>
    private static void TryInvokeStepComplete()
    {
        requireCardUseToProceed = false;
        allowedCardId = null;
        DisableAllCardRaycast();

        if (onTutorialStepCompleted != null)
        {
            onTutorialStepCompleted.Invoke();
            onTutorialStepCompleted = null;
        }
    }

    /// <summary>
    /// ī�� ��ü�� Raycast�� ����
    /// </summary>
    public static void DisableAllCardRaycast()
    {
        foreach (var cardUI in GameObject.FindObjectsOfType<CardUI>())
        {
            cardUI.SetRaycastBlock(true);
        }
    }

    /// <summary>
    /// ���� ī�常 Raycast�� ����ϰ� �������� ����
    /// </summary>
    public static void EnableRaycastForAllowedCard()
    {
        foreach (var cardUI in GameObject.FindObjectsOfType<CardUI>())
        {
            var card = cardUI.GetCardData();
            bool isAllowed = card != null && card.cardId == allowedCardId;
            cardUI.SetRaycastBlock(!isAllowed);
        }
    }

    // UI ���� ���
    public static void HighlightPlayerHand()
    {
        TutorialHighlighter.HighlightHandArea();
    }

    public static void HighlightEnemyIntendUI()
    {
        if (CombatContext.Instance.allEnemies.Count > 0)
        {
            TutorialHighlighter.HighlightFirstEnemyIntent();
        }
    }

    public static void HighlightCardById(string cardId)
    {
        TutorialHighlighter.HighlightCardById(cardId);
    }

    public static void HighlightTurnEndButton()
    {
        TutorialHighlighter.HighlightTurnEndButton();
    }

    public static void HighlightTargetById(string targetId)
    {
        TutorialHighlighter.HighlightByTargetId(targetId);
    }

    public static void ClearHighlights()
    {
        TutorialHighlighter.ClearAllHighlights();
    }

    /// <summary>
    /// ��Ʈ ���� ����: ī�� ���� + UI ����
    /// </summary>
    public static void ApplyHint(TutorialTurnHint hint, System.Action onStepComplete = null)
    {
        if (hint.highlightCard) HighlightPlayerHand();
        if (hint.highlightEnemy) HighlightEnemyIntendUI();

        if (!string.IsNullOrEmpty(hint.highlightTargetId))
        {
            HighlightTargetById(hint.highlightTargetId);
        }

        if (!string.IsNullOrEmpty(hint.forceCardId))
        {
            AllowOnlyCard(hint.forceCardId);
            RegisterOnStepComplete(onStepComplete);
            EnableRaycastForAllowedCard();
        }
    }

    /// <summary>
    /// Ʃ�丮�� ���� ���� �ʱ�ȭ
    /// </summary>
    public static void ClearAllTutorialState()
    {
        ClearHighlights();
        allowedCardId = null;
        requireCardUseToProceed = false;
        onTutorialStepCompleted = null;
        DisableAllCardRaycast();
    }
}
