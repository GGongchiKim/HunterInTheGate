using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatContext : MonoBehaviour
{
    public static CombatContext Instance { get; private set; }

    [Header("���� ���")]
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

    // ���� ���� �ʱ�ȭ
    public void Initialize(Player newPlayer, Enemy newSelectedEnemy, List<Enemy> enemies)
    {
        player = newPlayer;
        selectedEnemy = newSelectedEnemy;
        allEnemies = enemies;
    }
}