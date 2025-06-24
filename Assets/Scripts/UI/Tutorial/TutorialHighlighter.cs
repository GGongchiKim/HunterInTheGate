using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 튜토리얼 단계에서 특정 UI 요소에 강조 효과(깜빡이는 외곽선)를 적용하는 헬퍼 클래스
/// </summary>
public static class TutorialHighlighter
{
    private static readonly List<UIHighlighter> activeHighlights = new();


    /// <summary>
    /// 문자열 ID에 따라 특정 UI를 강조합니다. 튜토리얼 대사에서 targetId로 지정된 값 처리용.
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
                // 카드 ID로 간주
                HighlightCardById(targetId);
                break;
        }
    }


    /// <summary>
    /// 첫 번째 적의 Intent UI를 강조합니다.
    /// </summary>
    public static void HighlightFirstEnemyIntent()
    {
        var allEnemies = CombatContext.Instance?.allEnemies;
        if (allEnemies == null || allEnemies.Count == 0)
        {
            Debug.LogWarning("[TutorialHighlighter] CombatContext.Instance.allEnemies is empty");
            return;
        }

        var targetEnemy = allEnemies[0]; // 튜토리얼에서 첫 번째 적만 대상으로 함

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

                return; // 첫 번째 매칭만 강조
            }
        }

        Debug.LogWarning("[TutorialHighlighter] No EnemyHUDHandler matched the first enemy.");
    }




    /// <summary>
    /// 특정 카드 ID를 가진 카드 UI에 강조 효과 적용
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
    /// 전체 손패 카드 영역을 강조 (예: HandArea)
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
    /// 턴 종료 버튼을 강조
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
    /// 현재 적용 중인 모든 강조 효과를 제거
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
