using System.Collections.Generic;
using UnityEngine;

public class CombatDialogueController : MonoBehaviour
{
    public static CombatDialogueController Instance { get; private set; }

    private Queue<DialogueEntry> dialogueQueue = new();
    private bool isActive = false;

    public bool IsDialogueActive() => isActive;

    private void Awake()
    {
        //  싱글톤 중복 방지
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// 대사 큐 등록
    /// </summary>
    public void EnqueueDialogues(List<DialogueEntry> entries)
    {
        if (entries == null || entries.Count == 0)
        {
            Debug.LogWarning("[CombatDialogueController] 전달된 대사 목록이 비어 있음");
            return;
        }

        Debug.Log($"[CombatDialogueController] 대사 {entries.Count}개 등록됨");

        dialogueQueue.Clear();
        foreach (var entry in entries)
        {
            Debug.Log($"→ {entry.speakerName}: {entry.text}");
            dialogueQueue.Enqueue(entry);
        }

        isActive = true;
        SetCardRaycastState(false); // 카드 상호작용 비활성화
        TryDisplayNext();
    }

    /// <summary>
    /// 다음 대사 출력 시도
    /// </summary>
    private void TryDisplayNext()
    {
        Debug.Log($"[CombatDialogueController] TryDisplayNext(): 남은 대사 {dialogueQueue.Count}개");

        // 강조 효과 초기화
        TutorialCombatHelper.ClearHighlights();

        if (dialogueQueue.Count == 0)
        {
            Debug.Log("[CombatDialogueController] 대사 종료 → 패널 숨김, 전투 재개");
            isActive = false;
            CombatDialogueUI.HideAllDialogue();
            SetCardRaycastState(true);
            return;
        }

        DialogueEntry next = dialogueQueue.Dequeue();
        Debug.Log($"[CombatDialogueController] 다음 대사 → {next.speakerName}: {next.text}");

        CombatDialogueUI.ShowDialogue(next.isPlayer, next.speakerName, next.text);

        // 힌트 조건 적용 및 콜백 등록
        TutorialCombatHelper.ApplyHint(next.hint, OnTutorialStepComplete);
    }

    /// <summary>
    /// 유저 클릭 → 다음 대사 진행
    /// </summary>
    public void OnClickDialogue()
    {
        if (!isActive)
        {
            Debug.Log("[CombatDialogueController] 무시됨: 대사 진행 중 아님");
            return;
        }

        if (TutorialCombatHelper.RequireCardUseToProceed)
        {
            Debug.Log("[CombatDialogueController] 카드 사용 필요 상태. 클릭 무시됨.");
            return;
        }

        TryDisplayNext();
    }

    /// <summary>
    /// 튜토리얼 조건 충족 시 콜백
    /// </summary>
    private void OnTutorialStepComplete()
    {
        if (!isActive) return;

        Debug.Log("[CombatDialogueController] 튜토리얼 조건 완료 → 다음 대사");
        TryDisplayNext();
    }

    /// <summary>
    /// 전투 중 카드 상호작용 허용 여부 설정
    /// </summary>
    private void SetCardRaycastState(bool enabled)
    {
        foreach (var cardUI in FindObjectsOfType<CardUI>())
        {
            cardUI.SetRaycastBlock(!enabled);
        }
    }
}