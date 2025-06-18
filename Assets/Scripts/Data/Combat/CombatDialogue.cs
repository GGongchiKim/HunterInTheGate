using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombatDialogue
{
    public string speakerName;
    public string text;

    public CombatDialogueTiming timing; // 언제 출력되는지
    public int turnIndex;               // 몇 번째 턴에 출력되는지 (0부터 시작)
}

public enum CombatDialogueTiming
{
    PlayerTurn,
    EnemyTurn
}