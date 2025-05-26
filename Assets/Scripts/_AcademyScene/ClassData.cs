using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewClassData", menuName = "Data/ClassData")]
public class ClassData : ScriptableObject
{
    [Header("기본 정보")]
    public string className;
    public string description;
    public Sprite classIcon;

    [Header("수업 분류")]
    public ClassType classType;

    [Header("능력치 보상")]
    public List<StatModifier> statModifiers;

    [Header("보상 카드 미리보기 (정보 패널에 표시)")]
    public List<CardData> previewCards;

    [Header("실제 획득 가능한 카드 풀")]
    public List<CardData> rewardPool;

    [Header("애니메이션 결과 스프라이트")]
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
