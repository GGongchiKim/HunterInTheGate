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
    /// 대사 진행 조건 충족 여부
    /// </summary>
    public static bool CanProceedToNextDialogue()
    {
        return !requireCardUseToProceed;
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
        if (requireCardUseToProceed && usedCardId == allowedCardId)
        {
            Debug.Log($"[Tutorial] 허용된 카드({usedCardId}) 사용됨 → 다음 대사 진행");
            ClearCardRestriction();
            DisableAllCardRaycast();
            onTutorialStepCompleted?.Invoke();
        }
    }

    /// <summary>
    /// 카드 제한 해제 및 콜백 초기화
    /// </summary>
    public static void ClearCardRestriction()
    {
        allowedCardId = null;
        requireCardUseToProceed = false;
        onTutorialStepCompleted = null;
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
        Debug.Log("[TutorialHighlight] 손패 카드 강조 예정");
        // TODO: 실제 카드에 Outline 등 강조 효과 추가
    }

    public static void HighlightEnemy()
    {
        Debug.Log("[TutorialHighlight] 적 강조 예정");
        // TODO: 적 HUD에 강조 테두리 또는 애니메이션 표시
    }

    public static void ClearHighlights()
    {
        Debug.Log("[TutorialHighlight] 강조 효과 제거");
        // TODO: 모든 하이라이트 제거 로직 구현
    }

    /// <summary>
    /// 힌트 정보 적용: 카드 제한 + UI 강조
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
    /// 튜토리얼 상태 완전 초기화
    /// </summary>
    public static void ClearAllTutorialState()
    {
        ClearHighlights();
        ClearCardRestriction();
        DisableAllCardRaycast();
    }
}
