using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSceneController : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueEvent dialogueData;

    [Header("대화 타이핑 효과 처리")]
    private Coroutine typingCoroutine;               // 현재 실행 중인 코루틴 참조
    private bool isTyping = false;                   // 타이핑 중 여부

    [Header("UI")]
    [SerializeField] private Image playerScg;
    [SerializeField] private Image npcScg;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private Button choiceButtonPrefab;
    [SerializeField] private Image backgroundImage;

    private string currentNodeId;
    private List<Button> activeChoiceButtons = new();

    void Start()
    {
        string eventId = SceneDataBridge.Instance.ConsumeData<string>("EventId");

        if (!string.IsNullOrEmpty(eventId))
        {
            DialogueEvent loadedEvent = DialogueEventLoader.LoadEventById(eventId);
            if (loadedEvent != null)
            {
                dialogueData = loadedEvent;
            }
            else
            {
                Debug.LogError("[DialogueSceneController] 이벤트 로드 실패: " + eventId);
            }
        }

        if (dialogueData == null || dialogueData.nodes == null || dialogueData.nodes.Count == 0)
        {
            Debug.LogWarning("Dialogue data is empty or not assigned.");
            return;
        }

        // SCG 기본값: 완전 투명으로 초기화
        if (playerScg != null) playerScg.color = new Color(1f, 1f, 1f, 0f);
        if (npcScg != null) npcScg.color = new Color(1f, 1f, 1f, 0f);

        currentNodeId = dialogueData.nodes[0].nodeId;
        ShowCurrentNode();
    }

    void ShowCurrentNode()
    {
        if (!string.IsNullOrEmpty(dialogueData.backgroundSpriteId))
        {
            Sprite bgSprite = Resources.Load<Sprite>("BG/" + dialogueData.backgroundSpriteId);
            if (bgSprite != null)
                backgroundImage.sprite = bgSprite;
        }

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
            // 스프라이트 없을 경우 이미지도 null + 투명화 처리
            activeImage.sprite = null;
            activeImage.color = new Color(1f, 1f, 1f, 0f);
        }

        // 상대방 이미지 흐리게 (스프라이트 없으면 그냥 투명)
        if (inactiveImage.sprite != null)
        {
            inactiveImage.color = new Color(0.4f, 0.4f, 0.4f, 1f);
        }
        else
        {
            inactiveImage.color = new Color(1f, 1f, 1f, 0f);
        }

        //  텍스트 타이핑 처리
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(node.text));
    }


    IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            // 클릭 시 전체 출력
            if (Input.GetMouseButtonDown(0))
            {
                dialogueText.text = fullText;
                break;
            }

            dialogueText.text += fullText[i];
            yield return new WaitForSeconds(0.02f); // 타이핑 속도
        }

        isTyping = false;
    }


    IEnumerator WaitForClickThenAdvance(string nextNodeId)
    {
        // 출력 완료 전엔 클릭 무시
        yield return new WaitUntil(() =>
            Input.GetMouseButtonDown(0) && !isTyping
        );

        // 클릭 후 잠깐 딜레이
        yield return new WaitForSeconds(0.1f);

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
                SceneTransitionManager.Instance.LoadSceneWithFade(
                    "AcademyScene",
                    GamePhase.Management,
                    choice.jumpTargetIdOrScene
                );

                break;

            case DialogueJumpType.Battle:
                Debug.Log($"[Choice] 튜토리얼 전투 진입: {choice.jumpTargetIdOrScene}");

                SceneTransitionManager.Instance.LoadSceneWithFade(
                    "CombatScene",
                    GamePhase.Combat,
                    choice.jumpTargetIdOrScene
                );
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
                    SceneTransitionManager.Instance.LoadSceneWithFade(
                    "CombatScene",
                    GamePhase.Combat,
                    node.action.parameter)
;

                    return;

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
