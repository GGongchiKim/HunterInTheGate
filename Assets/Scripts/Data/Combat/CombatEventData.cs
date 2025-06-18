using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCombatEvent", menuName = "Event/Combat Event")]
public class CombatEventData : ScriptableObject
{
    public string eventId;

    [Header("Background")]
    public string backgroundSpriteId;

    [Header("Enemy Setup")]
    public List<EnemyData> enemies = new(); // 기본 초기화

    [Header("Turn Dialogue")]
    public List<CombatDialogue> turnDialogues = new(); // 기본 초기화
}

[Serializable]
public class CombatDialogue
{
    public int turnIndex;
    public CombatDialogueTiming timing; // PlayerTurn or EnemyTurn
    public string speakerName;
    [TextArea]
    public string text;
}

public enum CombatDialogueTiming
{
    PlayerTurn,
    EnemyTurn
}