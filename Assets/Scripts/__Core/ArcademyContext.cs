using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcademyContext : MonoBehaviour
{
    public static AcademyContext Instance { get; private set; }

    [Header("�ɷ�ġ")]
    public float strength = 10;
    public float agility = 10;
    public float insight = 10;
    public float magic = 10;
    public float willPower = 10;  // ���� �߰��� �ɷ�ġ
    public float wit = 10;        // ���� �߰��� �ɷ�ġ
    public float charisma = 10;   // ���� �߰��� �ɷ�ġ
    public float luck = 10;       // ���� �߰��� �ɷ�ġ

    [Header("�ְ� ������")]
    public int currentWeek = 1;
    public int currentDay = 1;

    private void Awake()
    {
        // �̱��� ������ ����Ͽ� AcademyContext �ν��Ͻ��� ����
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // �ְ� Ȱ���� �����ϴ� �޼���
    public void AdvanceWeek()
    {
        currentDay++;
        if (currentDay > 7) // 7���� ������ ���ο� �ַ� �Ѿ
        {
            currentDay = 1;
            currentWeek++;
        }

        // �ְ� Ȱ�� �� �ɷ�ġ ��ȭ ����
        ApplyWeeklyActivities();
    }

    // �ְ� Ȱ�� �� �ɷ�ġ ��ȭ
    public void ApplyWeeklyActivities()
    {
        // ���÷� �Ʒ� �� �ɷ�ġ ����
        strength += 2;  // �Ʒ� �� �� ����
        agility += 1;   // �Ʒ� �� ��ø ����
        willPower += 1; // �Ʒ� �� ������ ����
        wit += 1;       // �Ʒ� �� ����� ����
        charisma += 1;  // �Ʒ� �� ī������ ����
        luck += 1;      // �Ʒ� �� �� ����

        // �ɷ�ġ ��ȭ�� Player Ŭ������ �ݿ��ǵ��� ������Ʈ
        Player player = GameContext.Instance.player;
        player.strength = (int)strength;
        player.agility = (int)agility;
        player.insight = (int)insight;
        player.magic = (int)magic;
        player.willPower = (int)willPower;
        player.wit = (int)wit;
        player.charisma = (int)charisma;
        player.luck = (int)luck;

        // UI ������Ʈ (�ɷ�ġ�� ��ȭ�� �� UI�� ����)
        ArcademyUIManager.Instance.UpdateStatsUI();
    }

    // �ɷ�ġ ���� �޼���
    public void ChangeStat(string statName, float value)
    {
        switch (statName)
        {
            case "Strength":
                strength += value;
                break;
            case "Agility":
                agility += value;
                break;
            case "Insight":
                insight += value;
                break;
            case "Magic":
                magic += value;
                break;
            case "WillPower":
                willPower += value; // ������ �߰�
                break;
            case "Wit":
                wit += value;       // ����� �߰�
                break;
            case "Charisma":
                charisma += value;  // ī������ �߰�
                break;
            case "Luck":
                luck += value;      // �� �߰�
                break;
        }

        // Player Ŭ������ �ɷ�ġ �ݿ�
        Player player = GameContext.Instance.player;
        player.strength = (int)strength;
        player.agility = (int)agility;
        player.insight = (int)insight;
        player.magic = (int)magic;
        player.willPower = (int)willPower;
        player.wit = (int)wit;
        player.charisma = (int)charisma;
        player.luck = (int)luck;

        // UI ������Ʈ
        ArcademyUIManager.Instance.UpdateStatsUI();
    }

    // �Ʒ��� ���� �ɷ�ġ ��� ����
    public void ApplyTrainingResults()
    {
        strength += 3;  // �Ʒ� �� strength ����
        agility += 2;   // �Ʒ� �� agility ����
        willPower += 2; // �Ʒ� �� ������ ����
        wit += 1;       // �Ʒ� �� ����� ����
        charisma += 1;  // �Ʒ� �� ī������ ����
        luck += 1;      // �Ʒ� �� �� ����

        // Player Ŭ������ �ɷ�ġ �ݿ�
        Player player = GameContext.Instance.player;
        player.strength = (int)strength;
        player.agility = (int)agility;
        player.insight = (int)insight;
        player.magic = (int)magic;
        player.willPower = (int)willPower;
        player.wit = (int)wit;
        player.charisma = (int)charisma;
        player.luck = (int)luck;

        // UI ������Ʈ
        ArcademyUIManager.Instance.UpdateStatsUI();
    }
}