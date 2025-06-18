using System;
using System.Collections.Generic;

public enum DialogueJumpType
{
    Converge,
    Branch,
    Scene
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