using System;
using System.Collections.Generic;

public enum DialogueJumpType
{
    Converge,   // 기존 노드로 되돌아감
    Branch,     // 다른 노드로 이동
    Scene,      // 씬 전환
    Battle      // 전투 이벤트 진입
}


[Serializable]
public class DialogueChoice
{
    public string text;
    public string nextNodeId;
    public List<DialogueCondition> conditions;
    public List<DialogueEffect> effects;
    public DialogueJumpType jumpType;
    public string jumpTargetIdOrScene;
}