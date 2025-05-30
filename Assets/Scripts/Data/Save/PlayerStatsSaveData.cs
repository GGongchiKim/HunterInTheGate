using System;
using UnityEngine;

/// <summary>
/// 전투 능력치, 관계 능력치, 컨디션 정보를 저장 및 불러오기 위한 클래스
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

    // 생성자 (비어 있는 상태로 생성 가능)
    public PlayerStatsSaveData() { }

    // 현재 능력치 상태를 저장용 구조로 변환
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
    /// 저장된 데이터를 실제 게임 능력치 객체에 반영
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