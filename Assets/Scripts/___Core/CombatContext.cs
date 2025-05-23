using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투 씬 컨텍스트. CombatPlayer와 적 목록, 임시 보상 데이터 포함.
/// </summary>
public class CombatContext : MonoBehaviour
{
    public static CombatContext Instance { get; private set; }

    public CombatPlayer combatPlayer;
    public List<Enemy> allEnemies = new List<Enemy>();
    public Enemy selectedEnemy;

    [Header("전투 보상")]
    public int rewardGold;
    public int rewardExp;
    public List<string> rewardCards = new List<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void InitializeFromAcademy(AcademyPlayer source, List<Enemy> enemies)
    {
        combatPlayer = new CombatPlayer();
        combatPlayer.LoadFromAcademy(source);
        allEnemies = new List<Enemy>(enemies);
        selectedEnemy = allEnemies[0];
        ResetRewards();
    }

    public void ResetRewards()
    {
        rewardGold = 0;
        rewardExp = 0;
        rewardCards.Clear();
    }

    public void CommitResultsToAcademy(AcademyPlayer player)
    {
        player.ReceiveCombatRewards(rewardGold, rewardExp, rewardCards);
        ResetRewards();
    }
}