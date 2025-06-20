using System.Collections.Generic;
using UnityEngine;
using CutsceneSystem;

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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Academy ������ �Ѿ�� �����͸� ������� ���� �غ�
    /// </summary>
    public void InitializeFromAcademy(AcademyPlayer source, List<Enemy> enemies, CombatEventData combatEvent = null)
    {
        // �÷��̾� ����
        combatPlayer = new CombatPlayer();
        combatPlayer.LoadFromAcademy(source);

        // �� ����Ʈ ����
        allEnemies = new List<Enemy>(enemies);
        selectedEnemy = allEnemies.Count > 0 ? allEnemies[0] : null;

        // �̺�Ʈ ���� ����
        currentCombatEvent = combatEvent;

        // ���� �ʱ�ȭ
        ResetRewards();
    }

    public void ResetRewards()
    {
        rewardGold = 0;
        rewardExp = 0;
        rewardCards.Clear();
    }
}