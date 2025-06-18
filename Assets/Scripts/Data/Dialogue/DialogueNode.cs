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
    public string nodeId;                     // 이 노드의 고유 ID
    public DialogueNodeType nodeType;         // 노드 유형 (대사, 선택지, 액션)
    public string text;                       // 대사 텍스트 (또는 설명)
    public string speakerName;                // 화자 이름 ← 추가됨
    public string portraitSpriteId;           // SCG 이미지 리소스 ID ← 추가됨
    public List<DialogueChoice> choices;      // 선택지 리스트 (Choice 타입에서만 사용)
    public string nextNodeId;                 // 다음 노드의 ID
    public DialogueAction action;             // Action 타입일 때 실행할 특수 액션
}