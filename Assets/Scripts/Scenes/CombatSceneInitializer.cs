using System.Collections.Generic;
using UnityEngine;

public class CombatSceneInitializer : MonoBehaviour
{
    [Header("전투 적 프리팹")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("전투 플레이어 프리팹")]
    [SerializeField] private GameObject combatPlayerPrefab;

    [Header("전투 적 HUD 프리팹")]
    [SerializeField] private GameObject enemyHUDPrefab;

    private void Awake()
    {
        GameStateManager.Instance.SetPhase(GamePhase.Combat);

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

        // AcademyPlayer → CombatPlayer 변환
        var academyPlayer = GameContext.Instance.academyPlayer;
        if (academyPlayer == null)
        {
            Debug.LogError("[CombatSceneInitializer] AcademyPlayer가 존재하지 않습니다.");
            return;
        }

        GameObject playerGO = Instantiate(combatPlayerPrefab);
        CombatPlayer combatPlayer = playerGO.GetComponent<CombatPlayer>();
        combatPlayer.LoadFromAcademy(academyPlayer);
        DontDestroyOnLoad(playerGO);

        var playerEffectHandler = combatPlayer.GetComponent<EffectHandler>();
        if (playerEffectHandler != null)
        {
            playerEffectHandler.statusEffectPanel = C_HUDManager.Instance.playerStatusPanel;
        }

        // 적 생성 및 개별 HUD 생성
        List<Enemy> enemyList = new();
        foreach (var enemyData in evt.enemies)
        {
            GameObject enemyGO = Instantiate(enemyPrefab);
            Enemy enemy = enemyGO.GetComponent<Enemy>();
            enemy.Initialize(enemyData);

            GameObject hudGO = Instantiate(enemyHUDPrefab, C_HUDManager.Instance.enemyHUDParent);
            var hudHandler = hudGO.GetComponent<EnemyHUDHandler>();
            if (hudHandler != null)
            {
                hudHandler.Initialize(enemy);
                hudHandler.UpdateHealth(enemy.health, enemy.maxHealth, enemy.currentShield);

                // 🔥 Enemy.cs의 enemyHUD 필드에 명시적으로 연결
                enemy.enemyHUD = hudHandler;

                var effectHandler = enemy.GetComponent<EffectHandler>();
                if (effectHandler != null)
                {
                    effectHandler.statusEffectPanel = hudHandler.GetStatusPanel();
                }
            }

            C_HUDManager.Instance.RegisterEnemyHUD(enemy, hudGO);
            enemyList.Add(enemy);
        }

        CombatContext.Instance.InitializeFromAcademy(academyPlayer, enemyList, evt);
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
    }
}