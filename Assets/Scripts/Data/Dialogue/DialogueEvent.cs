using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 하나의 대화 이벤트를 ScriptableObject 형태로 정의
/// </summary>
[CreateAssetMenu(menuName = "Dialogue/DialogueEvent", fileName = "NewDialogueEvent")]
public class DialogueEvent : ScriptableObject
{
    public string eventId;                         // 이벤트 고유 ID (디버그, 연동용)
    public List<DialogueNode> nodes = new();       // 이 이벤트에 포함된 모든 노드 리스트

    /// <summary>
    /// nodeId로 노드 찾기
    /// </summary>
    public DialogueNode GetNodeById(string nodeId)
    {
        return nodes.Find(node => node.nodeId == nodeId);
    }

    /// <summary>
    /// 첫 노드 반환 (기본은 첫 번째 노드)
    /// </summary>
    public DialogueNode GetFirstNode()
    {
        return nodes.Count > 0 ? nodes[0] : null;
    }
}