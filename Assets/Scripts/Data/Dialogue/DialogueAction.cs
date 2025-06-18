using System;
using UnityEngine;

/// <summary>
/// ��ȭ ��忡�� ������ �׼��� Ÿ��
/// </summary>
public enum DialogueActionType
{
    StartCombat,     // ���� �̺�Ʈ ID�� �����ϸ� ���������� ��ȯ
    BranchDialogue,  // �ٸ� DialogueEvent�� �б�
    ModifyStat,      // (���� ����) �ɷ�ġ ��ȭ
    GainItem         // (���� ����) ������ ȹ��
}

/// <summary>
/// DialogueNode�� Action�� ��� ������ ��ü�� ��� ����
/// </summary>
[Serializable]
public class DialogueAction
{
    public DialogueActionType actionType;  // ���������� ����
    public string parameter;               // Event ID, Stat/Item ID �� �ǹ̴� Ÿ�Կ� ���� �޶���
}