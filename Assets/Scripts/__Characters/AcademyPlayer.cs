using System.Collections.Generic;
using UnityEngine;

public class AcademyPlayer : MonoBehaviour
{
    public string playerName;

    [Header("전투 관련 능력치")]
    public CombatStats combat;

    [Header("관계 및 이벤트 능력치")]
    public RelationStats relation;

    [Header("컨디션")]
    public ConditionStats condition;

    [Header("기타 정보")]
    public int gold;
    public int academyRank;
    public HunterGrade hunterGrade;
    public List<string> inventoryCards = new();
    public DeckData deck;
    public Dictionary<string, int> npcAffinity = new();

    public void ApplyModifiers(List<StatModifier> modifiers)
    {
        bool combatChanged = false, relationChanged = false, conditionChanged = false;

        foreach (var mod in modifiers)
        {
            switch (mod.statType)
            {
                case StatType.Strength: combat.strength += mod.amount; combatChanged = true; break;
                case StatType.Agility: combat.agility += mod.amount; combatChanged = true; break;
                case StatType.Magic: combat.magic += mod.amount; combatChanged = true; break;
                case StatType.Insight: combat.insight += mod.amount; combatChanged = true; break;
                case StatType.WillPower: combat.willPower += mod.amount; combatChanged = true; break;
                case StatType.Wit: combat.wit += mod.amount; combatChanged = true; break;

                case StatType.Charisma: relation.charisma += mod.amount; relationChanged = true; break;
                case StatType.Luck: relation.luck += mod.amount; relationChanged = true; break;
                case StatType.Fame: relation.fame += mod.amount; relationChanged = true; break;

                case StatType.Mood:
                    condition.mood = Mathf.Clamp(condition.mood + mod.amount, 1, 3); // 무드는 1~3단계로 고정
                    conditionChanged = true;
                    break;
                case StatType.Stress:
                    condition.stress = Mathf.Clamp(condition.stress + mod.amount, 0, 100); // 스트레스는 0~100
                    conditionChanged = true;
                    break;
            }
        }

        if (combatChanged) combat.NotifyChange();
        if (relationChanged) relation.NotifyChange();
        if (conditionChanged) condition.NotifyChange();
    }

    public int GetMoodStage()
    {
        if (condition.stress <= 30) return 1;
        if (condition.stress <= 70) return 2;
        return 3;
    }

    public void CollectRewards(List<string> rewards)
    {
        inventoryCards.AddRange(rewards);
    }

    public void ReceiveCombatRewards(int gold, int exp, List<string> cards)
    {
        inventoryCards.AddRange(cards);
    }
}