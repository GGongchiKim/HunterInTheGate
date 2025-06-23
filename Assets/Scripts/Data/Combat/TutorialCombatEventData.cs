using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTutorialCombatEvent", menuName = "Event/Tutorial Combat Event")]
public class TutorialCombatEventData : CombatEventData
{
    [Header("Tutorial ���� ��Ʈ �� ��ǥ")]
    public List<TutorialTurnHint> tutorialHints = new();
    public List<TutorialObjective> tutorialObjectives = new();

    [Tooltip("Ʃ�丮�� ���� ������ ī�常 ����� �� �ְ� �����մϴ�.")]
    public bool restrictCardUse = true;
}

[Serializable]
public class TutorialTurnHint
{
    public int turnIndex;
    public CombatDialogueTiming timing;
    public string speakerName;
    [TextArea] public string text;

    public bool highlightCard;
    public bool highlightEnemy;

    public string forceCardId; 
}

[Serializable]
public class TutorialObjective
{
    public int turnIndex;
    public string objectiveId;
    public string description;
    public bool completed;
}
