using UnityEngine;

/// <summary>
/// ���� ȿ�� Ÿ�� (����, �����, �������� ��)
/// </summary>
public enum StatusEffectType
{
    Buff,
    Debuff,
    DamageOverTime
}

/// <summary>
/// ���� ȿ�� ������ (ScriptableObject)
/// ���� �����͸� ���� (���� ������ StatusEffectLogic �� ���)
/// </summary>
[CreateAssetMenu(fileName = "NewStatusEffect", menuName = "Status/StatusEffect")]
public class StatusEffect : ScriptableObject
{
    [Header("�⺻ ����")]
    public string effectId;                      // ���� �ĺ��� (ex: "BLEED", "POISON")
    public string effectName;                    // UI�� �̸�
    public StatusEffectType effectType;          // ����/�����/�������� ����
    [TextArea(2, 3)] 
    public string description;                   // ȿ�� ����
    
    [Header("UI ǥ��")]
    public Sprite icon;                           // UI�� ������
    public Color iconColor = Color.white;  // ������ ���� (�⺻�� ���)

    [Header("�⺻ �Ӽ�")]
    public int defaultDuration = 3;              // �⺻ ���� �� ��
    public bool canStack = false;                // ��ø ���� ����
    public int maxStack = 0;                     // 0�̸� ������ ���� ���

    [Header("��ġ �ɼ� (������)")]
    public int flatBonusDamage = 0;               // ���� �߰� ���� (ex: ��������)
    public float percentBonusDamage = 0f;         // �ۼ�Ʈ ��� �߰� ���� (ex: ����)
    public float receivedDamageMultiplier = 1f;  // �޴� ���ط� ���� (ex: ���)
    public float givenDamageReduction = 0f;       // �ִ� ���ط� ���� ���� (ex: ��ȭ)
    public float healReductionRate = 0f;          // ȸ���� ���� ���� (ex: ȭ��)
    public float dotPercentDamage = 0f;           // DOT (��������) ���� (ex: ����, �͵�)

    [Header("���� ���� (�ʼ�)")]
    public StatusEffectLogic logicTemplate;       // ���� ȿ�� ����� ���� (�⺻: GenericStatusEffectLogic)

    /// <summary>
    /// �� StatusEffect�� ������� ����� Logic �ν��Ͻ��� �����Ѵ�.
    /// ���� ����� ����� Logic���� ó��
    /// </summary>
    public StatusEffectLogic CreateLogicInstance()
    {
        if (logicTemplate != null)
        {
            return Instantiate(logicTemplate);
        }
        else
        {
            Debug.LogWarning($"[StatusEffect] {effectName}�� LogicTemplate�� �����Ǿ� ���� �ʽ��ϴ�.");
            return null;
        }
    }

    public override string ToString()
    {
        return $"[{effectId}] {effectName} - {effectType}, {defaultDuration}�� {(canStack ? "(��ø ����)" : "")}";
    }
}