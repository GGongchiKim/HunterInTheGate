using System.Collections.Generic;
using UnityEngine;

// ���� ������ ������ (Management, Combat, Event ��)
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
        Debug.Log($"[GameStateManager] ����� {newPhase}�� �����");
    }


}

