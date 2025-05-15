using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    public static GameContext Instance { get; private set; }

    [Header("게임 상태")]
    public int currentWeek = 1;
    public int currentDay = 1;
    public bool isCombatScene = false; // 현재 전투 씬인지 여부

    [Header("플레이어 데이터")]
    public Player player;

    [Header("전역 관리")]
    public AcademyContext academyContext; // 아카데미 씬의 상태
    public CombatContext combatContext;   // 전투 씬의 상태

    private void Awake()
    {
        // 싱글톤 패턴으로 GameContext 관리
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // 기본 값으로 플레이어 생성 (게임 시작 시 기본 데이터 설정)
        player = new Player("Player1", 100, 10, 10, 10, 10, 10, 10, 10, 10);
        academyContext = AcademyContext.Instance;
        combatContext = CombatContext.Instance;
    }

    // 주간 스케줄 업데이트
    public void AdvanceWeek()
    {
        currentDay++;
        if (currentDay > 7)
        {
            currentDay = 1;
            currentWeek++;
        }

        // 아카데미 씬에서 주간 활동 후 능력치 변화 처리
        if (!isCombatScene)
        {
            academyContext.AdvanceWeek();
        }
    }

    // 전투 씬 상태 업데이트
    public void EnterCombatScene()
    {
        isCombatScene = true;

        // 새로운 적 생성 및 적 리스트 준비
        Enemy enemy = new Enemy(); // 기본 적 하나 생성
        List<Enemy> enemies = new List<Enemy> { enemy }; // 적 리스트 생성

        combatContext.Initialize(player, enemy, enemies); // 초기화
    }

    // 전투 씬을 벗어나면 아카데미 씬으로 돌아가기
    public void ExitCombatScene()
    {
        isCombatScene = false;
        academyContext.ApplyWeeklyActivities(); // 전투 후 아카데미 활동 반영
    }

    // 능력치 변경
    public void ChangePlayerStat(string statName, float value)
    {
        switch (statName)
        {
            case "Strength":
                player.strength += (int)value;
                break;
            case "Agility":
                player.agility += (int)value;
                break;
            case "Insight":
                player.insight += (int)value;
                break;
            case "Magic":
                player.magic += (int)value;
                break;
            case "WillPower":
                player.willPower += (int)value;
                break;
            case "Wit":
                player.wit += (int)value;
                break;
            case "Charisma":
                player.charisma += (int)value;
                break;
            case "Luck":
                player.luck += (int)value;
                break;
        }

        // 능력치 변화 후 UI 업데이트
        ArcademyUIManager.Instance.UpdateStatsUI();
    }

    // 게임 초기화
    public void InitializeGame()
    {
        // 전역 데이터를 초기화하고 플레이어 생성
        player = new Player("Player1", 100, 10, 10, 10, 10, 10, 10, 10, 10);
        currentWeek = 1;
        currentDay = 1;
        isCombatScene = false;
    }
}