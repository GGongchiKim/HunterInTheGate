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
    /// ��� ���� ���� ���� ����
    /// </summary>
    public static bool CanProceedToNextDialogue()
    {
        return !requireCardUseToProceed;
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
        if (requireCardUseToProceed && usedCardId == allowedCardId)
        {
            Debug.Log($"[Tutorial] ���� ī��({usedCardId}) ���� �� ���� ��� ����");
            ClearCardRestriction();
            DisableAllCardRaycast();
            onTutorialStepCompleted?.Invoke();
        }
    }

    /// <summary>
    /// ī�� ���� ���� �� �ݹ� �ʱ�ȭ
    /// </summary>
    public static void ClearCardRestriction()
    {
        allowedCardId = null;
        requireCardUseToProceed = false;
        onTutorialStepCompleted = null;
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
        Debug.Log("[TutorialHighlight] ���� ī�� ���� ����");
        // TODO: ���� ī�忡 Outline �� ���� ȿ�� �߰�
    }

    public static void HighlightEnemy()
    {
        Debug.Log("[TutorialHighlight] �� ���� ����");
        // TODO: �� HUD�� ���� �׵θ� �Ǵ� �ִϸ��̼� ǥ��
    }

    public static void ClearHighlights()
    {
        Debug.Log("[TutorialHighlight] ���� ȿ�� ����");
        // TODO: ��� ���̶���Ʈ ���� ���� ����
    }

    /// <summary>
    /// ��Ʈ ���� ����: ī�� ���� + UI ����
    /// </summary>
    public static void ApplyHint(TutorialTurnHint hint, System.Action onStepComplete = null)
    {
        if (hint.highlightCard) HighlightPlayerHand();
        if (hint.highlightEnemy) HighlightEnemy();

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
        ClearCardRestriction();
        DisableAllCardRaycast();
    }
}
