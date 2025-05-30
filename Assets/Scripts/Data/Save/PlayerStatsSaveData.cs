using System;
using UnityEngine;

/// <summary>
/// ���� �ɷ�ġ, ���� �ɷ�ġ, ����� ������ ���� �� �ҷ����� ���� Ŭ����
/// </summary>
[Serializable]
public class PlayerStatsSaveData
{
    // CombatStats
    public int strength;
    public int agility;
    public int magic;
    public int insight;
    public int willPower;
    public int wit;

    // RelationStats
    public int charisma;
    public int luck;
    public int fame;

    // ConditionStats
    public int mood;
    public int stress;

    // ������ (��� �ִ� ���·� ���� ����)
    public PlayerStatsSaveData() { }

    // ���� �ɷ�ġ ���¸� ����� ������ ��ȯ
    public PlayerStatsSaveData(CombatStats combat, RelationStats relation, ConditionStats condition)
    {
        strength = combat.strength;
        agility = combat.agility;
        magic = combat.magic;
        insight = combat.insight;
        willPower = combat.willPower;
        wit = combat.wit;

        charisma = relation.charisma;
        luck = relation.luck;
        fame = relation.fame;

        mood = condition.mood;
        stress = condition.stress;
    }

    /// <summary>
    /// ����� �����͸� ���� ���� �ɷ�ġ ��ü�� �ݿ�
    /// </summary>
    public void ApplyTo(CombatStats combat, RelationStats relation, ConditionStats condition)
    {
        combat.strength = strength;
        combat.agility = agility;
        combat.magic = magic;
        combat.insight = insight;
        combat.willPower = willPower;
        combat.wit = wit;

        relation.charisma = charisma;
        relation.luck = luck;
        relation.fame = fame;

        condition.mood = mood;
        condition.stress = stress;

        combat.NotifyChange();
        relation.NotifyChange();
        condition.NotifyChange();
    }
}