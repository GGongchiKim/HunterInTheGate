using System;
using UnityEngine;

/// <summary>
/// 대화 노드에서 실행할 액션의 타입
/// </summary>
public enum DialogueActionType
{
    StartCombat,     // 전투 이벤트 ID를 전달하며 전투씬으로 전환
    BranchDialogue,  // 다른 DialogueEvent로 분기
    ModifyStat,      // (차후 구현) 능력치 변화
    GainItem         // (차후 구현) 아이템 획득
}

/// <summary>
/// DialogueNode가 Action일 경우 실행할 구체적 명령 정보
/// </summary>
[Serializable]
public class DialogueAction
{
    public DialogueActionType actionType;  // 열거형으로 선택
    public string parameter;               // Event ID, Stat/Item ID 등 의미는 타입에 따라 달라짐
}