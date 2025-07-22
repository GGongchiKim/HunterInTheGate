using Inventory;
using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    public static GameContext Instance { get; private set; }

    [Header("플레이어")]
    public AcademyPlayer academyPlayer;
    public GameObject academyPlayerPrefab;

    [Header("인벤토리")]
    public PlayerInventory inventory;

    public enum SceneType { Academy, Combat, Inventory, Event }
    public SceneType currentScene = SceneType.Academy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeAcademyPlayer();
            Debug.Log($"[GameContext] AcademyPlayer 생성 완료: {academyPlayer}");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAcademyPlayer()
    {
        if (academyPlayer == null && academyPlayerPrefab != null)
        {
            GameObject playerInstance = Instantiate(academyPlayerPrefab);
            academyPlayer = playerInstance.GetComponent<AcademyPlayer>();
            academyPlayer.InitializeStats();
            DontDestroyOnLoad(playerInstance);
        }
    }

    public void EnterCombatScene(List<Enemy> enemies)
    {
        currentScene = SceneType.Combat;

        // CombatPlayer 생성 및 세팅
        GameObject combatPlayerGO = new GameObject("CombatPlayer");
        CombatPlayer combatPlayer = combatPlayerGO.AddComponent<CombatPlayer>();
        combatPlayer.LoadFromAcademy(academyPlayer);
        DontDestroyOnLoad(combatPlayerGO);

        CombatContext.Instance.InitializeFromAcademy(combatPlayer, enemies);
    }


    public void ExitCombatScene()
    {
        currentScene = SceneType.Academy;
    }

    public void ChangeScene(SceneType nextScene)
    {
        currentScene = nextScene;
    }
}
