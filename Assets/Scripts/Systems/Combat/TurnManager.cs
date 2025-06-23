using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [Header("매니저 참조")]
    public DeckManager deckManager;
    public HandManager handManager;

    [Header("전투 설정")]
    public int initialDrawCount = 5;
    public int drawCountPerTurn = 2;
    public int baseActionPoints = 5;

    private int currentTurn = 1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartCombat()
    {
        if (CombatContext.Instance.combatPlayer == null)
        {
            Debug.LogError("GameContext가 초기화되지 않았습니다.");
            return;
        }

        BeginCombat();
    }

    private void BeginCombat()
    {
        deckManager.InitializeCombatDeck();

        C_HUDManager.Instance.playerSpriteTransform = CombatContext.Instance.combatPlayer.transform;

        currentTurn = 1;
        SetActionPoints(baseActionPoints);

        DrawCards(initialDrawCount);
        DrawCards(drawCountPerTurn);

        GameStateManager.Instance.SetPhase(GamePhase.Combat);
        C_HUDManager.Instance.UpdateTurn(currentTurn);
        CombatContext.Instance.combatPlayer.ResetShield();

        UpdatePlayerEffects();

        PlayDialogueForTurn(currentTurn, true);

        C_DeckViewerManager.Instance.ForceRefreshAll();
        C_DeckViewerManager.Instance.ForceUpdateCounts();
    }

    public void EndTurn()
    {
        SetActionPoints(0);
        currentTurn++;
        StartCoroutine(EnemyTurnPhase());
    }

    private IEnumerator EnemyTurnPhase()
    {
        Debug.Log("=== 적 턴 시작 ===");

        PlayDialogueForTurn(currentTurn, false);

        foreach (Enemy enemy in CombatContext.Instance.allEnemies)
        {
            if (enemy == null) continue;

            enemy.GetComponent<EffectHandler>()?.UpdateEffects();
            enemy.PerformTurn();
            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("=== 플레이어 턴 시작 ===");

        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        SetActionPoints(baseActionPoints);
        DrawCards(drawCountPerTurn);
        C_HUDManager.Instance.UpdateTurn(currentTurn);
        CombatContext.Instance.combatPlayer.ResetShield();

        UpdatePlayerEffects();

        PlayDialogueForTurn(currentTurn, true);
    }

    private void PlayDialogueForTurn(int turn, bool isPlayerTurn)
    {
        var evt = CombatContext.Instance.currentCombatEvent;

        CombatDialogueTiming timing = isPlayerTurn
            ? CombatDialogueTiming.PlayerTurn
            : CombatDialogueTiming.EnemyTurn;

        if (evt is TutorialCombatEventData tutorialEvent)
        {
            var entries = DialogueEntryConverter.FromTutorialHints(tutorialEvent.tutorialHints, turn, timing);
            if (entries.Count > 0)
            {
                CombatDialogueController.Instance.EnqueueDialogues(entries);
                return;
            }
        }

        var normalEntries = DialogueEntryConverter.FromCombatDialogues(evt.turnDialogues, turn, timing);
        if (normalEntries.Count > 0)
        {
            CombatDialogueController.Instance.EnqueueDialogues(normalEntries);
        }
    }

    private void DrawCards(int count)
    {
        List<CardData> drawn = deckManager.DrawCards(count);
        handManager.AddCardsToHand(drawn);
    }

    public void DrawExtraCards(int count)
    {
        if (deckManager == null || handManager == null)
        {
            Debug.LogWarning("데크 또는 핸드 매니저가 설정되지 않았습니다.");
            return;
        }

        List<CardData> extraDrawn = deckManager.DrawCards(count);
        handManager.AddCardsToHand(extraDrawn);
        Debug.Log($"[전술카드] 추가 드로우 {count}장");
    }

    private void SetActionPoints(int ap)
    {
        if (CombatContext.Instance.combatPlayer == null)
        {
            Debug.LogWarning("플레이어가 null입니다.");
            return;
        }

        CombatContext.Instance.combatPlayer.actionPoints = ap;
        C_HUDManager.Instance.UpdateActionPoints(ap);
    }

    private void UpdatePlayerEffects()
    {
        CombatContext.Instance.combatPlayer.GetComponent<EffectHandler>()?.UpdateEffects();
    }

    private void HandleCombatEnd(bool playerWon)
    {
        var eventData = CombatContext.Instance.currentCombatEvent;

        if (playerWon)
        {
            string nextId = eventData.onWinDialogueId;
            if (!string.IsNullOrEmpty(nextId))
            {
                Debug.Log($"[CombatEnd] 승리 → 다음 대화 이벤트로 이동: {nextId}");
                SceneTransitionManager.Instance.LoadSceneWithFade("DialogueScene", GamePhase.Event, nextId);
            }
            else
            {
                Debug.LogWarning("[CombatEnd] 승리했지만 다음 대화 이벤트 ID가 비어 있습니다.");
            }
        }
        else
        {
            if (eventData.onLoseGameOver)
            {
                Debug.Log("[CombatEnd] 패배 → GameOver 패널 표시");
                // GameOverUI.Instance.Show();
            }
            else
            {
                Debug.LogWarning("[CombatEnd] 패배했지만 GameOver 처리 설정이 없습니다.");
            }
        }
    }

    public void CheckVictoryCondition()
    {
        if (CombatContext.Instance.allEnemies.Count == 0)
        {
            Debug.Log("[TurnManager] 모든 적 처치됨 → 즉시 승리 판정");
            HandleCombatEnd(playerWon: true);
        }
    }
}
