using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatContext : MonoBehaviour
{
    public static CombatContext Instance { get; private set; }

    [Header("전투 대상")]
    public Player player;
    public Enemy selectedEnemy;
    public List<Enemy> allEnemies = new List<Enemy>();

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

    // 전투 관련 초기화
    public void Initialize(Player newPlayer, Enemy newSelectedEnemy, List<Enemy> enemies)
    {
        player = newPlayer;
        selectedEnemy = newSelectedEnemy;
        allEnemies = enemies;
    }
}