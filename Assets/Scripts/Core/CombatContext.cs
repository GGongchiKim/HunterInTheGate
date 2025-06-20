using System.Collections.Generic;
using UnityEngine;
using CutsceneSystem;

/// <summary>
/// 전투 씬 컨텍스트. CombatPlayer와 적 목록, 전투 이벤트, 임시 보상 데이터 포함.
/// </summary>
public class CombatContext : MonoBehaviour
{
    public static CombatContext Instance { get; private set; }

    public CombatPlayer combatPlayer;
    public List<Enemy> allEnemies = new List<Enemy>();
    public Enemy selectedEnemy;

    [Header("전투 이벤트")]
    public CombatEventData currentCombatEvent;

    [Header("전투 보상")]
    public int rewardGold;
    public int rewardExp;
    public List<string> rewardCards = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Academy 씬에서 넘어온 데이터를 기반으로 전투 준비
    /// </summary>
    public void InitializeFromAcademy(AcademyPlayer source, List<Enemy> enemies, CombatEventData combatEvent = null)
    {
        // 플레이어 설정
        combatPlayer = new CombatPlayer();
        combatPlayer.LoadFromAcademy(source);

        // 적 리스트 설정
        allEnemies = new List<Enemy>(enemies);
        selectedEnemy = allEnemies.Count > 0 ? allEnemies[0] : null;

        // 이벤트 정보 저장
        currentCombatEvent = combatEvent;

        // 보상 초기화
        ResetRewards();
    }

    public void ResetRewards()
    {
        rewardGold = 0;
        rewardExp = 0;
        rewardCards.Clear();
    }
}