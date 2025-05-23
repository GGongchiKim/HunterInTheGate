using System.Collections.Generic;
using UnityEngine;

public class AcademyPlayer
{
    public string playerName;

    [Header("�⺻ �ɷ�ġ")]
    public int strength;
    public int agility;
    public int magic;
    public int insight;
    public int willPower;
    public int wit;
    public int charisma;
    public int luck;

    [Header("���� ���")]
    public DeckData deck;
    public List<string> inventoryCards = new();
    //public EquipmentSet equipment;

    [Header("����, �� ��")]
    public int reputation;
    public Dictionary<string, int> npcAffinity = new();

    public void ApplyModifiers(List<StatModifier> modifiers)
    {
        foreach (var mod in modifiers)
        {
            switch (mod.statType)
            {
                case StatType.Strength: strength += mod.amount; break;
                case StatType.Agility: agility += mod.amount; break;
                case StatType.Magic: magic += mod.amount; break;
                case StatType.Insight: insight += mod.amount; break;
                case StatType.WillPower: willPower += mod.amount; break;
                case StatType.Wit: wit += mod.amount; break;
                case StatType.Charisma: charisma += mod.amount; break;
                case StatType.Luck: luck += mod.amount; break;
            }
        }
    }

    public void CollectRewards(List<string> rewards)
    {
        inventoryCards.AddRange(rewards);
    }

    public void ReceiveCombatRewards(int gold, int exp, List<string> cards)
    {
        // TODO: ���, ����ġ, ī�� �ݿ� ��� ���� �ʿ�
        inventoryCards.AddRange(cards);
    }
}
