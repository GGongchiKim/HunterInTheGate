using UnityEngine;

/// <summary>
/// 게임의 전체 페이즈를 정의합니다.
/// </summary>
public enum GamePhase
{
    Management, // 육성, 인벤토리 관리, 덱 편집 등
    Combat,     // 전투
    Event,      // 스토리 이벤트, 선택지 등
    Gate        // 탐험 (Gate 내부 탐사, 전투 포함 가능)
}

/// <summary>
/// 현재 게임 페이즈를 관리하는 싱글톤 클래스입니다.
/// </summary>
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    /// <summary>
    /// 현재 게임 페이즈. 기본값은 Event.
    /// </summary>
    public GamePhase CurrentPhase { get; private set; } = GamePhase.Event;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 게임의 페이즈를 변경하고 로그를 출력합니다.
    /// </summary>
    public void SetPhase(GamePhase newPhase)
    {
        if (CurrentPhase != newPhase)
        {
            CurrentPhase = newPhase;
            Debug.Log($"[GameStateManager] 페이즈 전환 → {newPhase}");
        }
    }

    /// <summary>
    /// 현재 페이즈가 전투와 관련된 상태인지 확인합니다.
    /// </summary>
    public bool IsInCombatPhase()
    {
        return CurrentPhase == GamePhase.Combat || CurrentPhase == GamePhase.Gate;
    }
}