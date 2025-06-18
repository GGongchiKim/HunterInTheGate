using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCombatEvent", menuName = "Event/CombatEvent")]
public class CombatEvent : ScriptableObject
{
    public string eventId;

    [Header("Background")]
    public string backgroundSpriteId;

    [Header("Enemy Setup")]
    public List<EnemyData> enemies; // EnemyData�� ������ SO

    [Header("Turn Dialogue")]
    public List<CombatDialogue> turnDialogues;
}