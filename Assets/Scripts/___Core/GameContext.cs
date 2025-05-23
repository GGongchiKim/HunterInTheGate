using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    public static GameContext Instance { get; private set; }

    [Header("게임 진행 상태")]
    public int currentWeek = 1;
    public int currentDay = 1;

    [Header("플레이어")]
    public AcademyPlayer academyPlayer; // 리팩터링된 플레이어 클래스

    public enum SceneType { Academy, Combat, Inventory, Event }
    public SceneType currentScene = SceneType.Academy;

    private void Awake()
    {
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
        if (academyPlayer == null)
            academyPlayer = new AcademyPlayer(); // 실제로는 MonoBehaviour가 아님 → DI로 주입해야 함
    }

    public void AdvanceWeek()
    {
        currentDay++;
        if (currentDay > 7)
        {
            currentDay = 1;
            currentWeek++;
        }

        AcademyContext.Instance?.CommitResults(academyPlayer);
    }

    public void EnterCombatScene(List<Enemy> enemies)
    {
        currentScene = SceneType.Combat;
        CombatContext.Instance.InitializeFromAcademy(academyPlayer, enemies);
    }

    public void ExitCombatScene()
    {
        currentScene = SceneType.Academy;
    }

    public void ChangeScene(SceneType nextScene)
    {
        currentScene = nextScene;
    }

    public void InitializeGame()
    {
        academyPlayer = new AcademyPlayer();
        currentWeek = 1;
        currentDay = 1;
        currentScene = SceneType.Academy;
    }
}
