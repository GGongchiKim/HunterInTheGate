using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTutorialCombatEvent", menuName = "Event/Tutorial Combat Event")]
public class TutorialCombatEventData : CombatEventData
{
    [Header("Tutorial 전용 힌트 및 목표")]
    public List<TutorialTurnHint> tutorialHints = new();
    public List<TutorialObjective> tutorialObjectives = new();

    [Tooltip("튜토리얼 동안 지정된 카드만 사용할 수 있게 제한합니다.")]
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
