using UnityEngine;
using System;
using System.Collections.Generic;

public interface IStatChangeNotifier
{
    event Action OnStatsChanged;
    void NotifyChange();
}

[System.Serializable]
public class CombatStats : IStatChangeNotifier
{
    public int strength;
    public int agility;
    public int magic;
    public int insight;
    public int willPower;
    public int wit;

    public event Action OnStatsChanged;
    public void NotifyChange() => OnStatsChanged?.Invoke();
}

[System.Serializable]
public class RelationStats : IStatChangeNotifier
{
    public int charisma;
    public int luck;
    public int fame;

    public event Action OnStatsChanged;
    public void NotifyChange() => OnStatsChanged?.Invoke();
}

[System.Serializable]
public class ConditionStats : IStatChangeNotifier
{
    public int mood;
    public int stress;

    public event Action OnStatsChanged;
    public void NotifyChange() => OnStatsChanged?.Invoke();
}

public enum HunterGrade
{
    F, E, D, C, B, A, S, EX
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
    Luck,
    Fame,
    Mood,
    Stress
}

[System.Serializable]
public struct StatModifier
{
    public StatType statType;
    public int amount;
}