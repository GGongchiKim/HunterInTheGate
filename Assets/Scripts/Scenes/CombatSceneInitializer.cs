using Inventory;
using SaveSystem;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneInitializer : MonoBehaviour
{
    [Header("전투 매니저 프리팹 그룹")]
    [SerializeField] private GameObject managerGroupPrefab;

    [Header("전투 덱뷰어 패널 프리팹")]
    [SerializeField] private GameObject deckViewerPanelPrefab;

    [Header("전투 적 프리팹")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("전투 플레이어 프리팹")]
    [SerializeField] private GameObject combatPlayerPrefab;

    [Header("전투 적 HUD 프리팹")]
    [SerializeField] private GameObject enemyHUDPrefab;

    [Header("적 위치 배치 기준점 (중앙 기준)")]
    [SerializeField] private Transform enemySpawnCenter;

    [Header("적 간격 배수 (sprite 크기 기준)")]
    [SerializeField] private float spacingBase = 2.0f;

    private void Awake()
    {
        GameStateManager.Instance.SetPhase(GamePhase.Combat);

        if (managerGroupPrefab != null)
            Instantiate(managerGroupPrefab);
        else
            Debug.LogError("[CombatSceneInitializer] managerGroupPrefab이 설정되지 않았습니다.");

        if (deckViewerPanelPrefab != null)
        {
            Transform canvasParent = C_HUDManager.Instance.transform;
            GameObject panelInstance = Instantiate(deckViewerPanelPrefab, canvasParent);
            panelInstance.SetActive(false);
            C_HUDManager.Instance.SetDeckViewerPanel(panelInstance);
        }
        else
            Debug.LogError("[CombatSceneInitializer] deckViewerPanelPrefab이 설정되지 않았습니다.");

        string eventId = SceneDataBridge.Instance.GetString("EventId");
        if (string.IsNullOrEmpty(eventId))
        {
            Debug.LogError("[CombatSceneInitializer] EventId가 설정되지 않았습니다.");
            return;
        }

        CombatEventData evt = CombatEventLoader.GetById(eventId);
        if (evt == null)
        {
            Debug.LogError($"[CombatSceneInitializer] 전투 이벤트 데이터를 찾을 수 없습니다: {eventId}");
            return;
        }

        var academyPlayer = GameContext.Instance.academyPlayer;
        if (academyPlayer == null)
        {
            Debug.LogError("[CombatSceneInitializer] AcademyPlayer가 존재하지 않습니다.");
            return;
        }

        // 플레이어 생성 및 설정
        GameObject playerGO = Instantiate(combatPlayerPrefab);
        CombatPlayer combatPlayer = playerGO.GetComponent<CombatPlayer>();
        combatPlayer.LoadFromAcademy(academyPlayer);

        // 선택된 덱 적용 및 DeckManager 초기화
        DeckSaveData selectedDeck = SceneDataBridge.Instance.ConsumeData<DeckSaveData>("SelectedDeck");
        if (selectedDeck != null)
        {
            combatPlayer.LoadDeck(selectedDeck);

            List<CardData> cardList = new();
            foreach (string cardId in selectedDeck.cardIds)
            {
                var card = CardDatabase.Instance.GetCardById(cardId);
                if (card != null)
                    cardList.Add(card);
            }
            DeckManager.Instance.InitializeCombatDeck(cardList);
        }
        else
        {
            Debug.LogWarning("[CombatSceneInitializer] 선택된 덱이 없어 기본 덱을 사용합니다.");
            selectedDeck = GameContext.Instance.inventory.GetDefaultDeck();
        }

        // 효과 핸들러 연동
        var playerEffectHandler = combatPlayer.GetComponent<EffectHandler>();
        if (playerEffectHandler != null)
            playerEffectHandler.statusEffectPanel = C_HUDManager.Instance.playerStatusPanel;

        // === 적 배치 ===
        List<Enemy> enemyList = new();
        float currentX = 0f;
        float previousHalfWidth = 0f;

        for (int i = 0; i < evt.enemies.Count; i++)
        {
            var enemyData = evt.enemies[i];
            GameObject enemyGO = Instantiate(enemyPrefab);
            Enemy enemy = enemyGO.GetComponent<Enemy>();
            enemy.Initialize(enemyData);

            Transform spriteTF = enemyGO.transform.Find("EnemySprite");
            SpriteRenderer sr = spriteTF?.GetComponent<SpriteRenderer>();
            float currentHalfWidth = (sr != null && sr.sprite != null) ? sr.sprite.bounds.size.x / 2f : 0.5f;

            currentX += (i == 0 ? 0f : (previousHalfWidth + currentHalfWidth) * spacingBase);
            float offset = -((evt.enemies.Count - 1) / 2f) * currentX;
            Vector3 spawnPos = enemySpawnCenter != null ? enemySpawnCenter.position : Vector3.zero;
            enemyGO.transform.position = spawnPos + new Vector3(currentX + offset, 0, 0);

            previousHalfWidth = currentHalfWidth;

            // HUD 연결
            GameObject hudGO = Instantiate(enemyHUDPrefab, C_HUDManager.Instance.enemyHUDParent);
            var hudHandler = hudGO.GetComponent<EnemyHUDHandler>();
            if (hudHandler != null)
            {
                hudHandler.Initialize(enemy);
                hudHandler.UpdateHealth(enemy.health, enemy.maxHealth, enemy.currentShield);
                enemy.enemyHUD = hudHandler;

                var effectHandler = enemy.GetComponent<EffectHandler>();
                if (effectHandler != null)
                    effectHandler.statusEffectPanel = hudHandler.GetStatusPanel();
            }

            C_HUDManager.Instance.RegisterEnemyHUD(enemy, hudGO);
            enemyList.Add(enemy);
        }

        // 전투 컨텍스트 설정
        CombatContext.Instance.InitializeFromAcademy(combatPlayer, enemyList, evt);
        CombatContext.Instance.combatPlayer = combatPlayer;

        Debug.Log($"[CombatSceneInitializer] CombatContext 초기화 완료 (eventId = {eventId})");
    }

    private void Start()
    {
        var player = CombatContext.Instance.combatPlayer;

        if (player == null)
        {
            Debug.LogError("[CombatSceneInitializer] CombatContext에 전투 유닛이 설정되지 않았습니다.");
            return;
        }

        C_HUDManager.Instance.UpdatePlayerHealth(player.health, player.maxHealth);
        TurnManager.Instance.StartCombat();
        AudioManager.Instance?.PlayBGM("CombatBGM", true);
    }
}
