using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombatDialogue
{
    public string speakerName;
    public string text;

    public CombatDialogueTiming timing; // ���� ��µǴ���
    public int turnIndex;               // �� ��° �Ͽ� ��µǴ��� (0���� ����)
}

public enum CombatDialogueTiming
{
    PlayerTurn,
    EnemyTurn
}