using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameContext : MonoBehaviour
{
    public static GameContext Instance { get; private set; }

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

    /// <summary>
    /// �ܺο��� ���� ��� ����
    /// (�� ���� ������Ʈ ���� �ʿ�)
    /// </summary>
    public void Initialize(Player newPlayer, Enemy newSelectedEnemy, List<Enemy> enemies)
    {
        player = newPlayer;
        selectedEnemy = newSelectedEnemy;
        allEnemies = enemies;
    }
}