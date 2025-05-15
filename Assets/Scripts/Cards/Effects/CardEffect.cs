using UnityEngine;

/// <summary>
/// ��� ī�� ȿ���� �⺻ �߻� Ŭ���� (ScriptableObject ���)
/// ���� Ŭ������ �ݵ�� ExecuteEffect, IsValidTarget�� �����ؾ� ��.
/// �߰��� StatusEffect�� �����Ͽ� �����̻� ���뵵 ����.
/// </summary>
public abstract class CardEffect : ScriptableObject
{
    [Header("�����̻� �ɼ� (����)")]
    public StatusEffect statusEffect; // ī�� ȿ���� �Բ� ������ �� �ִ� �����̻� (������ ����)

    /// <summary>
    /// ī�� ȿ���� �����ϰ� ���� ���� ��ȯ (true: �ߵ���, false: ����)
    /// </summary>
    public abstract bool ExecuteEffect(CombatContext context, CardData cardData, GameObject target = null);

    /// <summary>
    /// ī�尡 ���� ����� ����ϴ��� ���� ��ȯ (�⺻�� false)
    /// ���� ī�常 �� �޼��带 override�Ͽ� true ��ȯ
    /// </summary>
    public virtual bool AllowsGlobalDrop()
    {
        return false;
    }

    /// <summary>
    /// �ش� GameObject�� �� ī���� ��ȿ�� ������� ����
    /// ��: ����ī��� Enemy��, ȸ��ī��� Player�� ������� ��ȿ ó��
    /// </summary>
    public abstract bool IsValidTarget(GameObject target);

    /// <summary>
    /// �����̻�(StatusEffect)�� ���(target)���� �����ϴ� ���� �޼���
    /// </summary>
    protected void ApplyStatusEffect(GameObject target, StatusEffect statusEffect, int baseDamage)
    {
        if (statusEffect == null || target == null) return;

        var handler = target.GetComponent<EffectHandler>();
        if (handler != null)
        {
            handler.AddEffect(statusEffect, baseDamage);
        }

        var player = target.GetComponent<Player>();
        if (player != null)
        {
            player.ApplyEffect(statusEffect);
            return;
        }

        var enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.ApplyEffect(statusEffect);
            return;
        }

        Debug.LogWarning($"[CardEffect] {target.name}�� ����ȿ���� ������ �� �����ϴ�. (Player/Enemy ������Ʈ ����)");
    }
}