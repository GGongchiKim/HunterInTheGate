using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// ���� �� ��µǴ� ��� UI�� �����մϴ�.
/// �÷��̾�� �� ������ ��� �г��� �������� ��ġ�صΰ�, �ʿ� �� Ȱ��/��Ȱ��ȭ�մϴ�.
/// </summary>
public class CombatDialogueUI : MonoBehaviour, IPointerClickHandler
{
    public static CombatDialogueUI Instance { get; private set; }

    [Header("Player Dialogue UI")]
    [SerializeField] private GameObject playerDialoguePanel;
    [SerializeField] private TextMeshProUGUI playerSpeakerNameText;
    [SerializeField] private TextMeshProUGUI playerDialogueText;

    [Header("Enemy Dialogue UI")]
    [SerializeField] private GameObject enemyDialoguePanel;
    [SerializeField] private TextMeshProUGUI enemySpeakerNameText;
    [SerializeField] private TextMeshProUGUI enemyDialogueText;

    // ���� ����
    private bool isSkippable = true;
    private Action onDialogueEndCallback = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        HideAllDialogue();
    }

    /// <summary>
    /// ��� ��� �� ���� ���
    /// </summary>
    public static void ShowDialogue(bool isPlayer, string speaker, string text, bool skippable = true, Action onEnd = null)
    {
        if (Instance == null)
        {
            Debug.LogWarning("[CombatDialogueUI] Instance�� �������� �ʽ��ϴ�.");
            return;
        }

        HideAllDialogue(); // �� �� �ϳ��� Ȱ��ȭ

        Instance.isSkippable = skippable;
        Instance.onDialogueEndCallback = onEnd;

        if (isPlayer)
        {
            Instance.playerDialoguePanel.SetActive(true);
            Instance.playerSpeakerNameText.text = speaker;
            Instance.playerDialogueText.text = text;
        }
        else
        {
            Instance.enemyDialoguePanel.SetActive(true);
            Instance.enemySpeakerNameText.text = speaker;
            Instance.enemyDialogueText.text = text;
        }

        Debug.Log($"[CombatDialogueUI] ��� ��µ�: {speaker} - {text}");
    }

    /// <summary>
    /// Ŭ�� �� ���� ���� �����ϰų� ���� �ݹ� ȣ��
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isSkippable)
        {
            Debug.Log("[CombatDialogueUI] Ŭ�� ���� (��ŵ �Ұ� ���)");
            return;
        }

        // ���� �ݹ� ���� �� ���� ť�� �����ϴ� CombatDialogueController�� ����
        CombatDialogueController.Instance?.OnClickDialogue();
    }

    /// <summary>
    /// ��� ��� �г� ��Ȱ��ȭ
    /// </summary>
    public static void HideAllDialogue()
    {
        if (Instance == null) return;

        Instance.playerDialoguePanel.SetActive(false);
        Instance.enemyDialoguePanel.SetActive(false);
        Instance.isSkippable = true;
        Instance.onDialogueEndCallback = null;
    }

    /// <summary>
    /// ���� ��簡 ��ŵ ������ �������� ����
    /// </summary>
    public static bool IsDialogueSkippable()
    {
        return Instance != null && Instance.isSkippable;
    }

    /// <summary>
    /// ���� ��ȭ ������ ����
    /// </summary>
    public static bool IsDialogueActive()
    {
        if (Instance == null) return false;
        return Instance.playerDialoguePanel.activeSelf || Instance.enemyDialoguePanel.activeSelf;
    }
}