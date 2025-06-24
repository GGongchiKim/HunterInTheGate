using UnityEngine;

/// <summary>
/// 튜토리얼 전투 중 카드 사용 제한, UI 강조, 대사 흐름 제한을 관리합니다.
/// </summary>
public static class TutorialCombatHelper
{
    private static string allowedCardId = null;
    private static bool requireCardUseToProceed = false;
    private static System.Action onTutorialStepCompleted;

    /// <summary>
    /// 현재 카드 사용이 제한된 상태인지 외부에서 확인
    /// </summary>
    public static bool RequireCardUseToProceed => requireCardUseToProceed;

    /// <summary>
    /// 현재 카드가 사용 가능한 카드인지 판단
    /// </summary>
    public static bool IsCardUsable(string cardId)
    {
        return string.IsNullOrEmpty(allowedCardId) || cardId == allowedCardId;
    }

    /// <summary>
    /// 현재 카드가 튜토리얼에 의해 금지된 상태인지 판단
    /// </summary>
    public static bool IsCardCurrentlyRestricted(string cardId)
    {
        return !string.IsNullOrEmpty(allowedCardId) && cardId != allowedCardId;
    }

    /// <summary>
    /// 특정 카드만 사용하도록 제한
    /// </summary>
    public static void AllowOnlyCard(string cardId)
    {
        allowedCardId = cardId;
        requireCardUseToProceed = true;
    }

    /// <summary>
    /// 튜토리얼 흐름 콜백 등록
    /// </summary>
    public static void RegisterOnStepComplete(System.Action callback)
    {
        onTutorialStepCompleted = callback;
    }

    /// <summary>
    /// 카드가 사용되었을 때 호출됨
    /// </summary>
    public static void OnCardUsed(string usedCardId)
    {
        if (!requireCardUseToProceed) return;

        if (usedCardId == allowedCardId)
        {
            Debug.Log($"[Tutorial] 허용된 카드({usedCardId}) 사용됨 → 다음 대사 자동 진행");
            TryInvokeStepComplete();
        }
    }

    /// <summary>
    /// 콜백 실행 및 상태 초기화
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
    /// 카드 전체의 Raycast를 차단
    /// </summary>
    public static void DisableAllCardRaycast()
    {
        foreach (var cardUI in GameObject.FindObjectsOfType<CardUI>())
        {
            cardUI.SetRaycastBlock(true);
        }
    }

    /// <summary>
    /// 허용된 카드만 Raycast를 허용하고 나머지는 차단
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

    // UI 강조 기능
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
    /// 힌트 정보 적용: 카드 제한 + UI 강조
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
    /// 튜토리얼 상태 완전 초기화
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
