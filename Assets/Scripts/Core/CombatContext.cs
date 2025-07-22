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
    /// 전투 씬 초기화. 이미 Instantiate된 CombatPlayer를 전달받아 세팅함.
    /// </summary>
    public void InitializeFromAcademy(CombatPlayer playerInstance, List<Enemy> enemies, CombatEventData combatEvent = null)
    {
        // 외부에서 생성된 CombatPlayer를 직접 주입
        combatPlayer = playerInstance;

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