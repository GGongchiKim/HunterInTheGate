using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    public static GameContext Instance { get; private set; }

    [Header("게임 진행 상태")]
    public int currentWeek = 1;
    public int currentDay = 1;

    [Header("플레이어")]
    public AcademyPlayer academyPlayer;
    public GameObject academyPlayerPrefab;

    public enum SceneType { Academy, Combat, Inventory, Event }
    public SceneType currentScene = SceneType.Academy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (academyPlayer == null && academyPlayerPrefab != null)
            {
                GameObject playerInstance = Instantiate(academyPlayerPrefab);
                academyPlayer = playerInstance.GetComponent<AcademyPlayer>();
                academyPlayer.InitializeStats(); // ✅ 초기 능력치 설정 호출
                DontDestroyOnLoad(playerInstance);
            }
            Debug.Log($"[GameContext] AcademyPlayer 생성 완료: {academyPlayer}");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AdvanceDay()
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
        currentWeek = 1;
        currentDay = 1;
        currentScene = SceneType.Academy;
    }
}