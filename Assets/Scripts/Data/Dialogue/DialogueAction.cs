using System;

[Serializable]
public class DialogueAction
{
    public string actionType; // ��: "StartBattle", "LoadScene"
    public string parameter;  // ��: "GateBattleScene"
}
