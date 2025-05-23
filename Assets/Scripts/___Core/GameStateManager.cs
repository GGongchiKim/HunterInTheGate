using System.Collections.Generic;
using UnityEngine;

// 게임 페이즈 열거형 (Management, Combat, Event 등)
public enum GamePhase { Management, Combat, Event }


public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public GamePhase CurrentPhase { get; set; } = GamePhase.Management;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPhase(GamePhase newPhase)
    {
        CurrentPhase = newPhase;
        Debug.Log($"[GameStateManager] 페이즈가 {newPhase}로 변경됨");
    }


}

