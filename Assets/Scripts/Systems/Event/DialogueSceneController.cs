using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSceneController : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueEvent dialogueData;

    [Header("UI")]
    [SerializeField] private Image playerScg;
    [SerializeField] private Image npcScg;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private Button choiceButtonPrefab;

    private string currentNodeId;
    private List<Button> activeChoiceButtons = new();

    void Start()
    {
        if (dialogueData.nodes == null || dialogueData.nodes.Count == 0)
        {
            Debug.LogWarning("Dialogue data is empty.");
            return;
        }

        currentNodeId = dialogueData.nodes[0].nodeId;
        ShowCurrentNode();
    }

    void ShowCurrentNode()
    {
        DialogueNode node = FindNodeById(currentNodeId);
        if (node == null)
        {
            Debug.LogError($"DialogueNode '{currentNodeId}' not found.");
            return;
        }

        ShowDialogueBase(node);

        switch (node.nodeType)
        {
            case DialogueNodeType.Dialogue:
                StartCoroutine(WaitForClickThenAdvance(node.nextNodeId));
                break;

            case DialogueNodeType.Choice:
                ShowChoices(node);
                break;

            case DialogueNodeType.Action:
                HandleAction(node);
                break;
        }
    }

    void ShowDialogueBase(DialogueNode node)
    {
        speakerNameText.text = node.speakerName;
        dialogueText.text = node.text;

        bool isPlayer = IsPlayer(node.speakerName);
        Sprite sprite = null;

        if (!string.IsNullOrEmpty(node.portraitSpriteId))
            sprite = Resources.Load<Sprite>("Scg/" + node.portraitSpriteId);

        Image activeImage = isPlayer ? playerScg : npcScg;
        Image inactiveImage = isPlayer ? npcScg : playerScg;

        if (sprite != null)
        {
            activeImage.sprite = sprite;
            activeImage.color = Color.white;
        }
        else
        {
            activeImage.sprite = null;
            activeImage.color = new Color(1, 1, 1, 0);
        }

        if (inactiveImage.sprite != null)
            inactiveImage.color = new Color(0.4f, 0.4f, 0.4f, 1f);
        else
            inactiveImage.color = new Color(1, 1, 1, 0);
    }

    System.Collections.IEnumerator WaitForClickThenAdvance(string nextNodeId)
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        currentNodeId = nextNodeId;
        ShowCurrentNode();
    }

    void ShowChoices(DialogueNode node)
    {
        choicesPanel.SetActive(true);
        ClearOldChoices();

        foreach (var choice in node.choices)
        {
            if (!AreConditionsMet(choice.conditions))
                continue;

            var btn = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
            btn.onClick.AddListener(() =>
            {
                ApplyEffects(choice.effects);
                HandleJump(choice);
                choicesPanel.SetActive(false);
            });

            activeChoiceButtons.Add(btn);
        }
    }

    void HandleJump(DialogueChoice choice)
    {
        switch (choice.jumpType)
        {
            case DialogueJumpType.Converge:
                currentNodeId = choice.jumpTargetIdOrScene;
                ShowCurrentNode();
                break;

            case DialogueJumpType.Branch:
                DialogueEvent newEvent = DialogueEventLoader.LoadEventById(choice.jumpTargetIdOrScene);
                if (newEvent != null)
                {
                    dialogueData = newEvent;
                    currentNodeId = dialogueData.nodes[0].nodeId;
                    ShowCurrentNode();
                }
                else
                {
                    Debug.LogError("Branch target event not found: " + choice.jumpTargetIdOrScene);
                }
                break;

            case DialogueJumpType.Scene:
                Debug.Log($"Scene jump: {choice.jumpTargetIdOrScene}");
                // GameStateManager.Instance.LoadScene(choice.jumpTargetIdOrScene);
                break;
        }
    }

    void HandleAction(DialogueNode node)
    {
        if (node.action != null)
        {
            switch (node.action.actionType)
            {
                case DialogueActionType.StartCombat:
                    Debug.Log($"[Action] 전투 시작: {node.action.parameter}");
                   // GameStateTransfer.combatEventId = node.action.parameter;
                    //GameStateManager.Instance.LoadScene("CombatScene"); // 전투 씬 이름은 필요시 수정
                    return; // 씬 이동이므로 이후 로직 실행하지 않음

                case DialogueActionType.BranchDialogue:
                    var newEvent = DialogueEventLoader.LoadEventById(node.action.parameter);
                    if (newEvent != null)
                    {
                        dialogueData = newEvent;
                        currentNodeId = newEvent.nodes[0].nodeId;
                        ShowCurrentNode();
                    }
                    else
                    {
                        Debug.LogError("[Action] 대화 이벤트 분기 실패: ID = " + node.action.parameter);
                    }
                    break;

                case DialogueActionType.ModifyStat:
                    Debug.Log($"[Action] 능력치 변화 (미구현): {node.action.parameter}");
                    break;

                case DialogueActionType.GainItem:
                    Debug.Log($"[Action] 아이템 획득 (미구현): {node.action.parameter}");
                    break;
            }
        }

        if (!string.IsNullOrEmpty(node.nextNodeId))
        {
            StartCoroutine(WaitForClickThenAdvance(node.nextNodeId));
        }
    }

    void ApplyEffects(List<DialogueEffect> effects)
    {
        foreach (var effect in effects)
        {
            switch (effect.mode)
            {
                case EffectMode.Set:
                    GameVariables.Set(effect.targetVariable, effect.amount);
                    break;

                case EffectMode.Add:
                    int currentAdd = GameVariables.GetInt(effect.targetVariable);
                    GameVariables.Set(effect.targetVariable, currentAdd + effect.amount);
                    break;

                case EffectMode.Subtract:
                    int currentSub = GameVariables.GetInt(effect.targetVariable);
                    GameVariables.Set(effect.targetVariable, currentSub - effect.amount);
                    break;
            }
        }
    }

    bool AreConditionsMet(List<DialogueCondition> conditions)
    {
        if (conditions == null || conditions.Count == 0) return true;

        foreach (var condition in conditions)
        {
            int actual = GameVariables.GetInt(condition.variableName);
            switch (condition.op)
            {
                case ConditionOperator.Equal:
                    if (actual != condition.value) return false;
                    break;
                case ConditionOperator.GreaterThan:
                    if (actual <= condition.value) return false;
                    break;
                case ConditionOperator.LessThan:
                    if (actual >= condition.value) return false;
                    break;
            }
        }
        return true;
    }

    DialogueNode FindNodeById(string nodeId)
    {
        return dialogueData.nodes.Find(n => n.nodeId == nodeId);
    }

    void ClearOldChoices()
    {
        foreach (var btn in activeChoiceButtons)
            Destroy(btn.gameObject);
        activeChoiceButtons.Clear();
    }

    bool IsPlayer(string name)
    {
        return name == "Player" || name == "서예린";
    }
}
