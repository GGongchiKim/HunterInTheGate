using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [Header("매니저 참조")]
    public DeckManager deckManager;
    public HandManager handManager;

    [Header("적 인텐트 UI")]
    public GameObject enemyIntentPrefab;

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

        currentTurn = 1;
        SetActionPoints(baseActionPoints);

        DrawCards(initialDrawCount);
        DrawCards(drawCountPerTurn);

        GameStateManager.Instance.SetPhase(GamePhase.Combat);
        C_HUDManager.Instance.UpdateTurn(currentTurn);
        CombatContext.Instance.combatPlayer.ResetShield();

        InitializeEnemyIntents();
        C_DeckViewerManager.Instance.ForceRefreshAll();
        C_DeckViewerManager.Instance.ForceUpdateCounts();

        // 🔹 전투 시작 직후, 플레이어 상태이상 갱신
        UpdatePlayerEffects();
    }

    private void InitializeEnemyIntents()
    {
        foreach (Enemy enemy in CombatContext.Instance.allEnemies)
        {
            if (enemy == null || enemy.intentUI != null) continue;

            GameObject uiObj = Instantiate(enemyIntentPrefab);
            uiObj.transform.SetParent(enemy.transform, worldPositionStays: true);
            uiObj.transform.position = enemy.transform.position + new Vector3(0, 2f, 0);
            uiObj.transform.localScale = Vector3.one;

            EnemyIntentUI intentUI = uiObj.GetComponent<EnemyIntentUI>();
            if (intentUI == null)
            {
                Debug.LogError("EnemyIntentUI 컴포넌트가 없습니다!");
                continue;
            }

            SpriteRenderer sr = enemy.GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                intentUI.targetEnemySprite = sr.transform;
            }
            else
            {
                Debug.LogWarning($"[{enemy.name}]에게 SpriteRenderer가 없습니다.");
            }

            enemy.SetIntentUI(intentUI);
        }
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

        // 🔹 적들 상태이상 갱신
        foreach (Enemy enemy in CombatContext.Instance.allEnemies)
        {
            if (enemy == null) continue;

            enemy.GetComponent<EffectHandler>()?.UpdateEffects(); // DOT 적용!
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

        // 🔹 플레이어 상태이상 갱신
        UpdatePlayerEffects();
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
            Debug.LogWarning("덱 또는 핸드 매니저가 설정되지 않았습니다.");
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
}