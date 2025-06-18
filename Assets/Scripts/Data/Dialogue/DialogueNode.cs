using System;
using System.Collections.Generic;

public enum DialogueNodeType
{
    Dialogue,
    Choice,
    Action
}

[Serializable]
public class DialogueNode
{
    public string nodeId;                     // �� ����� ���� ID
    public DialogueNodeType nodeType;         // ��� ���� (���, ������, �׼�)
    public string text;                       // ��� �ؽ�Ʈ (�Ǵ� ����)
    public string speakerName;                // ȭ�� �̸� �� �߰���
    public string portraitSpriteId;           // SCG �̹��� ���ҽ� ID �� �߰���
    public List<DialogueChoice> choices;      // ������ ����Ʈ (Choice Ÿ�Կ����� ���)
    public string nextNodeId;                 // ���� ����� ID
    public DialogueAction action;             // Action Ÿ���� �� ������ Ư�� �׼�
}