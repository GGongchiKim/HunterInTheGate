using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    public static GameContext Instance { get; private set; }

    [Header("���� ����")]
    public int currentWeek = 1;
    public int currentDay = 1;
    public bool isCombatScene = false; // ���� ���� ������ ����

    [Header("�÷��̾� ������")]
    public Player player;

    [Header("���� ����")]
    public AcademyContext academyContext; // ��ī���� ���� ����
    public CombatContext combatContext;   // ���� ���� ����

    private void Awake()
    {
        // �̱��� �������� GameContext ����
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // �⺻ ������ �÷��̾� ���� (���� ���� �� �⺻ ������ ����)
        player = new Player("Player1", 100, 10, 10, 10, 10, 10, 10, 10, 10);
        academyContext = AcademyContext.Instance;
        combatContext = CombatContext.Instance;
    }

    // �ְ� ������ ������Ʈ
    public void AdvanceWeek()
    {
        currentDay++;
        if (currentDay > 7)
        {
            currentDay = 1;
            currentWeek++;
        }

        // ��ī���� ������ �ְ� Ȱ�� �� �ɷ�ġ ��ȭ ó��
        if (!isCombatScene)
        {
            academyContext.AdvanceWeek();
        }
    }

    // ���� �� ���� ������Ʈ
    public void EnterCombatScene()
    {
        isCombatScene = true;

        // ���ο� �� ���� �� �� ����Ʈ �غ�
        Enemy enemy = new Enemy(); // �⺻ �� �ϳ� ����
        List<Enemy> enemies = new List<Enemy> { enemy }; // �� ����Ʈ ����

        combatContext.Initialize(player, enemy, enemies); // �ʱ�ȭ
    }

    // ���� ���� ����� ��ī���� ������ ���ư���
    public void ExitCombatScene()
    {
        isCombatScene = false;
        academyContext.ApplyWeeklyActivities(); // ���� �� ��ī���� Ȱ�� �ݿ�
    }

    // �ɷ�ġ ����
    public void ChangePlayerStat(string statName, float value)
    {
        switch (statName)
        {
            case "Strength":
                player.strength += (int)value;
                break;
            case "Agility":
                player.agility += (int)value;
                break;
            case "Insight":
                player.insight += (int)value;
                break;
            case "Magic":
                player.magic += (int)value;
                break;
            case "WillPower":
                player.willPower += (int)value;
                break;
            case "Wit":
                player.wit += (int)value;
                break;
            case "Charisma":
                player.charisma += (int)value;
                break;
            case "Luck":
                player.luck += (int)value;
                break;
        }

        // �ɷ�ġ ��ȭ �� UI ������Ʈ
        ArcademyUIManager.Instance.UpdateStatsUI();
    }

    // ���� �ʱ�ȭ
    public void InitializeGame()
    {
        // ���� �����͸� �ʱ�ȭ�ϰ� �÷��̾� ����
        player = new Player("Player1", 100, 10, 10, 10, 10, 10, 10, 10, 10);
        currentWeek = 1;
        currentDay = 1;
        isCombatScene = false;
    }
}