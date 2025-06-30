using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewGateData", menuName = "Gate/GateData")]
public class GateData : ScriptableObject
{
    [Header("기본 정보")]
    public string gateId;
    public string gateName;
    public Sprite gateIcon;

    [Header("등장 몬스터")]
    public List<Sprite> monsterSprites = new List<Sprite>(); // 최대 3개

    [Header("획득 가능한 아티팩트")]
    public List<ArtifactInfo> artifacts = new List<ArtifactInfo>(4);

    [Header("실행 시 진입할 대화이벤트")]
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