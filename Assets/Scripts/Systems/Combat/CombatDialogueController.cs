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
        //  �̱��� �ߺ� ����
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
    /// ��� ť ���
    /// </summary>
    public void EnqueueDialogues(List<DialogueEntry> entries)
    {
        if (entries == null || entries.Count == 0)
        {
            Debug.LogWarning("[CombatDialogueController] ���޵� ��� ����� ��� ����");
            return;
        }

        Debug.Log($"[CombatDialogueController] ��� {entries.Count}�� ��ϵ�");

        dialogueQueue.Clear();
        foreach (var entry in entries)
        {
            Debug.Log($"�� {entry.speakerName}: {entry.text}");
            dialogueQueue.Enqueue(entry);
        }

        isActive = true;
        SetCardRaycastState(false); // ī�� ��ȣ�ۿ� ��Ȱ��ȭ
        TryDisplayNext();
    }

    /// <summary>
    /// ���� ��� ��� �õ�
    /// </summary>
    private void TryDisplayNext()
    {
        Debug.Log($"[CombatDialogueController] TryDisplayNext(): ���� ��� {dialogueQueue.Count}��");

        // ���� ȿ�� �ʱ�ȭ
        TutorialCombatHelper.ClearHighlights();

        if (dialogueQueue.Count == 0)
        {
            Debug.Log("[CombatDialogueController] ��� ���� �� �г� ����, ���� �簳");
            isActive = false;
            CombatDialogueUI.HideAllDialogue();
            SetCardRaycastState(true);
            return;
        }

        DialogueEntry next = dialogueQueue.Dequeue();
        Debug.Log($"[CombatDialogueController] ���� ��� �� {next.speakerName}: {next.text}");

        CombatDialogueUI.ShowDialogue(next.isPlayer, next.speakerName, next.text);

        // ��Ʈ ���� ���� �� �ݹ� ���
        TutorialCombatHelper.ApplyHint(next.hint, OnTutorialStepComplete);
    }

    /// <summary>
    /// ���� Ŭ�� �� ���� ��� ����
    /// </summary>
    public void OnClickDialogue()
    {
        if (!isActive)
        {
            Debug.Log("[CombatDialogueController] ���õ�: ��� ���� �� �ƴ�");
            return;
        }

        if (TutorialCombatHelper.RequireCardUseToProceed)
        {
            Debug.Log("[CombatDialogueController] ī�� ��� �ʿ� ����. Ŭ�� ���õ�.");
            return;
        }

        TryDisplayNext();
    }

    /// <summary>
    /// Ʃ�丮�� ���� ���� �� �ݹ�
    /// </summary>
    private void OnTutorialStepComplete()
    {
        if (!isActive) return;

        Debug.Log("[CombatDialogueController] Ʃ�丮�� ���� �Ϸ� �� ���� ���");
        TryDisplayNext();
    }

    /// <summary>
    /// ���� �� ī�� ��ȣ�ۿ� ��� ���� ����
    /// </summary>
    private void SetCardRaycastState(bool enabled)
    {
        foreach (var cardUI in FindObjectsOfType<CardUI>())
        {
            cardUI.SetRaycastBlock(!enabled);
        }
    }
}