using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 전투 중 출력되는 대사 UI를 관리합니다.
/// 플레이어와 적 각각의 대사 패널을 정적으로 배치해두고, 필요 시 활성/비활성화합니다.
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

    // 현재 상태
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
    /// 대사 출력 및 제어 등록
    /// </summary>
    public static void ShowDialogue(bool isPlayer, string speaker, string text, bool skippable = true, Action onEnd = null)
    {
        if (Instance == null)
        {
            Debug.LogWarning("[CombatDialogueUI] Instance가 존재하지 않습니다.");
            return;
        }

        HideAllDialogue(); // 둘 중 하나만 활성화

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

        Debug.Log($"[CombatDialogueUI] 대사 출력됨: {speaker} - {text}");
    }

    /// <summary>
    /// 클릭 시 다음 대사로 진행하거나 종료 콜백 호출
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isSkippable)
        {
            Debug.Log("[CombatDialogueUI] 클릭 무시 (스킵 불가 대사)");
            return;
        }

        // 기존 콜백 제거 → 실제 큐를 제어하는 CombatDialogueController로 위임
        CombatDialogueController.Instance?.OnClickDialogue();
    }

    /// <summary>
    /// 모든 대사 패널 비활성화
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
    /// 현재 대사가 스킵 가능한 상태인지 여부
    /// </summary>
    public static bool IsDialogueSkippable()
    {
        return Instance != null && Instance.isSkippable;
    }

    /// <summary>
    /// 현재 대화 중인지 여부
    /// </summary>
    public static bool IsDialogueActive()
    {
        if (Instance == null) return false;
        return Instance.playerDialoguePanel.activeSelf || Instance.enemyDialoguePanel.activeSelf;
    }
}