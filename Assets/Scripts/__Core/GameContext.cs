using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameContext : MonoBehaviour
{
    public static GameContext Instance { get; private set; }

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

    /// <summary>
    /// 외부에서 게임 대상 세팅
    /// (씬 상의 오브젝트 연결 필요)
    /// </summary>
    public void Initialize(Player newPlayer, Enemy newSelectedEnemy, List<Enemy> enemies)
    {
        player = newPlayer;
        selectedEnemy = newSelectedEnemy;
        allEnemies = enemies;
    }
}