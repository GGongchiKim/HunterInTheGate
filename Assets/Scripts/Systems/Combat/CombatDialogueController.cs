using System.Collections.Generic;
using UnityEngine;

public class CombatDialogueController : MonoBehaviour
{
    public static CombatDialogueController Instance { get; private set; }

    private Queue<DialogueEntry> dialogueQueue = new();
    private bool isActive = false; // 현재 대화가 진행 중인지 상태 플래그
    public bool IsDialogueActive() => isActive;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EnqueueDialogues(List<DialogueEntry> entries)
    {
        if (entries == null || entries.Count == 0)
        {
            Debug.LogWarning("[CombatDialogueController] 빈 대사 목록이 전달됨");
            return;
        }

        Debug.Log($"[CombatDialogueController] 전달된 대사 수: {entries.Count}");
        foreach (var e in entries)
        {
            Debug.Log($"→ Speaker: {e.speakerName}, Text: {e.text}");
        }

        dialogueQueue.Clear();
        foreach (var entry in entries)
            dialogueQueue.Enqueue(entry);

        isActive = true;
        SetCardRaycastState(false);
        TryDisplayNext();
    }

    private void TryDisplayNext()
    {
        Debug.Log($"[CombatDialogueController] TryDisplayNext() 호출됨. 큐 개수: {dialogueQueue.Count}");

        if (dialogueQueue.Count == 0)
        {
            Debug.Log("[CombatDialogueController] 대사 큐 종료 → 패널 숨김 + 전투 재개 예정");
            isActive = false;
            CombatDialogueUI.HideAllDialogue();
            SetCardRaycastState(true);
            TutorialCombatHelper.ClearCardRestriction();
            return;
        }

        DialogueEntry next = dialogueQueue.Dequeue();
        Debug.Log($"[CombatDialogueController] 다음 대사: {next.speakerName} - {next.text}");

        CombatDialogueUI.ShowDialogue(next.isPlayer, next.speakerName, next.text);

        if (!string.IsNullOrEmpty(next.forceCardId))
        {
            TutorialCombatHelper.ApplyHint(next.hint, OnTutorialStepComplete);
            Debug.Log("[CombatDialogueController] 튜토리얼 조건 대사 → 카드 사용 대기");
        }
    }

    public void OnClickDialogue()
    {
        if (!isActive)
        {
            Debug.Log("[CombatDialogueController] 클릭 무시됨 - 대사 진행 중 아님");
            return;
        }

        if (TutorialCombatHelper.RequireCardUseToProceed)
        {
            Debug.Log("[CombatDialogueController] 강제 카드 사용 필요. 클릭 무시됨.");
            return;
        }

        TryDisplayNext();
    }

    private void OnTutorialStepComplete()
    {
        if (!isActive) return;

        Debug.Log("[CombatDialogueController] 튜토리얼 조건 완료됨 → 다음 대사로 진행");
        TryDisplayNext();
    }


    private void SetCardRaycastState(bool enabled)
    {
        foreach (var cardUI in FindObjectsOfType<CardUI>())
        {
            cardUI.SetRaycastBlock(!enabled);
        }
    }
}
