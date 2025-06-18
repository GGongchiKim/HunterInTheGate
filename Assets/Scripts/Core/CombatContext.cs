using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �� ���ؽ�Ʈ. CombatPlayer�� �� ���, ���� �̺�Ʈ, �ӽ� ���� ������ ����.
/// </summary>
public class CombatContext : MonoBehaviour
{
    public static CombatContext Instance { get; private set; }

    public CombatPlayer combatPlayer;
    public List<Enemy> allEnemies = new List<Enemy>();
    public Enemy selectedEnemy;

    [Header("���� �̺�Ʈ")]
    public CombatEventData currentCombatEvent;

    [Header("���� ����")]
    public int rewardGold;
    public int rewardExp;
    public List<string> rewardCards = new List<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void InitializeFromAcademy(AcademyPlayer source, List<Enemy> enemies, CombatEventData combatEvent = null)
    {
        combatPlayer = new CombatPlayer();
        combatPlayer.LoadFromAcademy(source);
        allEnemies = new List<Enemy>(enemies);
        selectedEnemy = allEnemies.Count > 0 ? allEnemies[0] : null;
        currentCombatEvent = combatEvent;
        ResetRewards();
    }

    public void ResetRewards()
    {
        rewardGold = 0;
        rewardExp = 0;
        rewardCards.Clear();
    }
}