using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewGateData", menuName = "Gate/GateData")]
public class GateData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string gateId;
    public string gateName;
    public Sprite gateIcon;

    [Header("���� ����")]
    public List<Sprite> monsterSprites = new List<Sprite>(); // �ִ� 3��

    [Header("ȹ�� ������ ��Ƽ��Ʈ")]
    public List<ArtifactInfo> artifacts = new List<ArtifactInfo>(4);

    [Header("���� �� ������ ��ȭ�̺�Ʈ")]
    public string dialogueEventId;
}

[System.Serializable]
public class ArtifactInfo
{
    public string title;
    public Sprite image;
    [TextArea]
    public string description;
}