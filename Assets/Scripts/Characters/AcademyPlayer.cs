using System.Collections.Generic;
using UnityEngine;
using SaveSystem; // PlayerStatsSaveData ���ǵ� ���ӽ����̽�

public class AcademyPlayer : MonoBehaviour
{
    public static AcademyPlayer Instance { get; private set; }

    [Header("�÷��̾� ����")]
    public string playerName;

    [Header("���� ���� �ɷ�ġ")]
    public CombatStats combat;

    [Header("���� �� �̺�Ʈ �ɷ�ġ")]
    public RelationStats relation;

    [Header("�����")]
    public ConditionStats condition;

    [Header("��Ÿ ����")]
    public int gold;
    public HunterRank hunterRank;
    public DeckSaveData deck;
    public Dictionary<string, int> npcAffinity = new();

    private const int MaxStatValue = 999;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeStats()
    {
        // ���� �ɷ�ġ �ʱⰪ
        combat.strength = 50;
        combat.agility = 30;
        combat.magic = 25;
        combat.insight = 40;
        combat.willPower = 100;
        combat.wit = 65;

        // ���� �ɷ�ġ �ʱⰪ
        relation.charisma = 30;
        relation.fame = 0;
        relation.luck = 15;

        // ����� �ʱⰪ
        condition.stress = 10;
        condition.mood = 1;
    }

    public void ApplyModifiers(List<StatModifier> modifiers)
    {
        bool combatChanged = false, relationChanged = false, conditionChanged = false;

        foreach (var mod in modifiers)
        {
            switch (mod.statType)
            {
                case StatType.Strength: combat.strength = Mathf.Clamp(combat.strength + mod.amount, 0, MaxStatValue); combatChanged = true; break;
                case StatType.Agility: combat.agility = Mathf.Clamp(combat.agility + mod.amount, 0, MaxStatValue); combatChanged = true; break;
                case StatType.Magic: combat.magic = Mathf.Clamp(combat.magic + mod.amount, 0, MaxStatValue); combatChanged = true; break;
                case StatType.Insight: combat.insight = Mathf.Clamp(combat.insight + mod.amount, 0, MaxStatValue); combatChanged = true; break;
                case StatType.WillPower: combat.willPower = Mathf.Clamp(combat.willPower + mod.amount, 0, MaxStatValue); combatChanged = true; break;
                case StatType.Wit: combat.wit = Mathf.Clamp(combat.wit + mod.amount, 0, MaxStatValue); combatChanged = true; break;

                case StatType.Charisma: relation.charisma = Mathf.Clamp(relation.charisma + mod.amount, 0, 100); relationChanged = true; break;
                case StatType.Luck: relation.luck = Mathf.Clamp(relation.luck + mod.amount, 0, 100); relationChanged = true; break;
                case StatType.Fame: relation.fame = Mathf.Clamp(relation.fame + mod.amount, 0, MaxStatValue); relationChanged = true; break;

                case StatType.Mood: condition.mood = Mathf.Clamp(condition.mood + mod.amount, 1, 3); conditionChanged = true; break;
                case StatType.Stress: condition.stress = Mathf.Clamp(condition.stress + mod.amount, 0, 100); conditionChanged = true; break;
            }
        }

        if (combatChanged) combat.NotifyChange();
        if (relationChanged) relation.NotifyChange();
        if (conditionChanged) condition.NotifyChange();
    }

    public void ApplyStats(PlayerStatsSaveData saveData)
    {
        if (saveData == null)
        {
            Debug.LogWarning("[AcademyPlayer] ApplyStats: ����� �ɷ�ġ �����Ͱ� null�Դϴ�.");
            return;
        }

        combat.strength = saveData.strength;
        combat.agility = saveData.agility;
        combat.magic = saveData.magic;
        combat.insight = saveData.insight;
        combat.willPower = saveData.willPower;
        combat.wit = saveData.wit;

        relation.charisma = saveData.charisma;
        relation.luck = saveData.luck;
        relation.fame = saveData.fame;

        condition.mood = saveData.mood;
        condition.stress = saveData.stress;

        // �ʿ� �� �̺�Ʈ �߼�
        combat.NotifyChange();
        relation.NotifyChange();
        condition.NotifyChange();
    }

    public int GetMoodStage()
    {
        if (condition.stress <= 30) return 1;
        if (condition.stress <= 70) return 2;
        return 3;
    }
}