using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CombatDialogue �� TutorialTurnHint�� DialogueEntry�� ��ȯ�ϴ� ��ƿ��Ƽ.
/// ���� �ó����� �帧�� �����ϱ� ���� �ϰ��� �������� �����մϴ�.
/// </summary>
public static class DialogueEntryConverter
{
    /// <summary>
    /// �Ϲ� ���� ���(CombatDialogue)�� DialogueEntry�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="dialogues">CombatEventData.turnDialogues</param>
    /// <param name="turn">���� �� �ε���</param>
    /// <param name="timing">��� Ÿ�̹� (PlayerTurn, EnemyTurn ��)</param>
    /// <returns>DialogueEntry ����Ʈ</returns>
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
    /// Ʃ�丮�� ��Ʈ ���(TutorialTurnHint)�� DialogueEntry�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="hints">TutorialCombatEventData.tutorialHints</param>
    /// <param name="turn">���� �� �ε���</param>
    /// <param name="timing">��� Ÿ�̹� (PlayerTurn, EnemyTurn ��)</param>
    /// <returns>DialogueEntry ����Ʈ</returns>
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