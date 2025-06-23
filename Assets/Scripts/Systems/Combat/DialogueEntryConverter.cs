using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CombatDialogue 및 TutorialTurnHint를 DialogueEntry로 변환하는 유틸리티.
/// 전투 시나리오 흐름을 제어하기 위한 일관된 포맷으로 정리합니다.
/// </summary>
public static class DialogueEntryConverter
{
    /// <summary>
    /// 일반 전투 대사(CombatDialogue)를 DialogueEntry로 변환합니다.
    /// </summary>
    /// <param name="dialogues">CombatEventData.turnDialogues</param>
    /// <param name="turn">현재 턴 인덱스</param>
    /// <param name="timing">대사 타이밍 (PlayerTurn, EnemyTurn 등)</param>
    /// <returns>DialogueEntry 리스트</returns>
    public static List<DialogueEntry> FromCombatDialogues(List<CombatDialogue> dialogues, int turn, CombatDialogueTiming timing)
    {
        List<DialogueEntry> result = new();

        if (dialogues == null) return result;

        foreach (var d in dialogues)
        {
            if (d.turnIndex == turn && d.timing == timing)
            {
                result.Add(new DialogueEntry
                {
                    isPlayer = timing == CombatDialogueTiming.PlayerTurn,
                    speakerName = d.speakerName,
                    text = d.text,
                    forceCardId = null,
                    hint = null
                });
            }
        }

        return result;
    }

    /// <summary>
    /// 튜토리얼 힌트 대사(TutorialTurnHint)를 DialogueEntry로 변환합니다.
    /// </summary>
    /// <param name="hints">TutorialCombatEventData.tutorialHints</param>
    /// <param name="turn">현재 턴 인덱스</param>
    /// <param name="timing">대사 타이밍 (PlayerTurn, EnemyTurn 등)</param>
    /// <returns>DialogueEntry 리스트</returns>
    public static List<DialogueEntry> FromTutorialHints(List<TutorialTurnHint> hints, int turn, CombatDialogueTiming timing)
    {
        List<DialogueEntry> result = new();

        if (hints == null) return result;

        foreach (var h in hints)
        {
            if (h.turnIndex == turn && h.timing == timing)
            {
                result.Add(new DialogueEntry
                {
                    isPlayer = timing == CombatDialogueTiming.PlayerTurn,
                    speakerName = h.speakerName,
                    text = h.text,
                    forceCardId = h.forceCardId,
                    hint = h
                });
            }
        }

        return result;
    }
}