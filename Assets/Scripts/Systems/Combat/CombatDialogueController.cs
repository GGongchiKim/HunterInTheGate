using System.Collections.Generic;
using UnityEngine;

public class CombatDialogueController : MonoBehaviour
{
    public static CombatDialogueController Instance { get; private set; }

    private Queue<DialogueEntry> dialogueQueue = new();
    private bool isActive = false; // ���� ��ȭ�� ���� ������ ���� �÷���
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
            Debug.LogWarning("[CombatDialogueController] �� ��� ����� ���޵�");
            return;
        }

        Debug.Log($"[CombatDialogueController] ���޵� ��� ��: {entries.Count}");
        foreach (var e in entries)
        {
            Debug.Log($"�� Speaker: {e.speakerName}, Text: {e.text}");
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
        Debug.Log($"[CombatDialogueController] TryDisplayNext() ȣ���. ť ����: {dialogueQueue.Count}");

        if (dialogueQueue.Count == 0)
        {
            Debug.Log("[CombatDialogueController] ��� ť ���� �� �г� ���� + ���� �簳 ����");
            isActive = false;
            CombatDialogueUI.HideAllDialogue();
            SetCardRaycastState(true);
            TutorialCombatHelper.ClearCardRestriction();
            return;
        }

        DialogueEntry next = dialogueQueue.Dequeue();
        Debug.Log($"[CombatDialogueController] ���� ���: {next.speakerName} - {next.text}");

        CombatDialogueUI.ShowDialogue(next.isPlayer, next.speakerName, next.text);

        if (!string.IsNullOrEmpty(next.forceCardId))
        {
            TutorialCombatHelper.ApplyHint(next.hint, OnTutorialStepComplete);
            Debug.Log("[CombatDialogueController] Ʃ�丮�� ���� ��� �� ī�� ��� ���");
        }
    }

    public void OnClickDialogue()
    {
        if (!isActive)
        {
            Debug.Log("[CombatDialogueController] Ŭ�� ���õ� - ��� ���� �� �ƴ�");
            return;
        }

        if (TutorialCombatHelper.RequireCardUseToProceed)
        {
            Debug.Log("[CombatDialogueController] ���� ī�� ��� �ʿ�. Ŭ�� ���õ�.");
            return;
        }

        TryDisplayNext();
    }

    private void OnTutorialStepComplete()
    {
        if (!isActive) return;

        Debug.Log("[CombatDialogueController] Ʃ�丮�� ���� �Ϸ�� �� ���� ���� ����");
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
