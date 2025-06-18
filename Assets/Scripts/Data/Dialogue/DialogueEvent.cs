using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ϳ��� ��ȭ �̺�Ʈ�� ScriptableObject ���·� ����
/// </summary>
[CreateAssetMenu(menuName = "Dialogue/DialogueEvent", fileName = "NewDialogueEvent")]
public class DialogueEvent : ScriptableObject
{
    public string eventId;                         // �̺�Ʈ ���� ID (�����, ������)
    public List<DialogueNode> nodes = new();       // �� �̺�Ʈ�� ���Ե� ��� ��� ����Ʈ

    /// <summary>
    /// nodeId�� ��� ã��
    /// </summary>
    public DialogueNode GetNodeById(string nodeId)
    {
        return nodes.Find(node => node.nodeId == nodeId);
    }

    /// <summary>
    /// ù ��� ��ȯ (�⺻�� ù ��° ���)
    /// </summary>
    public DialogueNode GetFirstNode()
    {
        return nodes.Count > 0 ? nodes[0] : null;
    }
}