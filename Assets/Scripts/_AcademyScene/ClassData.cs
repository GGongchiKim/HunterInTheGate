using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewClassData", menuName = "Data/ClassData")]
public class ClassData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string className;
    public string description;
    public Sprite classIcon;

    [Header("���� �з�")]
    public ClassType classType;

    [Header("�ɷ�ġ ����")]
    public List<StatModifier> statModifiers;

    [Header("���� ī�� �̸����� (���� �гο� ǥ��)")]
    public List<CardData> previewCards;

    [Header("���� ȹ�� ������ ī�� Ǯ")]
    public List<CardData> rewardPool;

    [Header("�ִϸ��̼� ��� ��������Ʈ")]
    public Sprite resultSuccess;
    public Sprite resultGreatSuccess;
    public Sprite resultFailure;
}

[System.Serializable]
public struct StatModifier
{
    public StatType statType;
    public int amount;
}

public enum StatType
{
    Strength,
    Agility,
    Magic,
    Insight,
    WillPower,
    Wit,
    Charisma,
    Luck
}

public enum ClassType
{
    Strike,
    Grace,
    Tactic,
    Etc
}
