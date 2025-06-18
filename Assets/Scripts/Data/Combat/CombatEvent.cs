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
    public List<EnemyData> enemies; // EnemyData는 별도의 SO

    [Header("Turn Dialogue")]
    public List<CombatDialogue> turnDialogues;
}