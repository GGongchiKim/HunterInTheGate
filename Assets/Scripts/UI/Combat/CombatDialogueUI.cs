using UnityEngine;
using TMPro;

public class CombatDialogueUI : MonoBehaviour
{
    public static CombatDialogueUI Instance { get; private set; }

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        HideDialogue();
    }

    /// <summary>
    /// 전투 중 대사를 출력합니다.
    /// </summary>
    public static void ShowDialogue(string speaker, string text)
    {
        if (Instance == null)
        {
            Debug.LogWarning("CombatUI Instance가 존재하지 않습니다.");
            return;
        }

        Instance.dialoguePanel.SetActive(true);
        Instance.speakerNameText.text = speaker;
        Instance.dialogueText.text = text;
    }

    /// <summary>
    /// 대사 UI를 숨깁니다.
    /// </summary>
    public static void HideDialogue()
    {
        if (Instance == null) return;
        Instance.dialoguePanel.SetActive(false);
    }
}
