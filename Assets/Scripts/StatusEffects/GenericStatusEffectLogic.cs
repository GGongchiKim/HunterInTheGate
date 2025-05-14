using UnityEngine;

/// <summary>
/// ��� ����ȿ���� ������ ������� ó���ϴ� ���� ����ȿ�� ����.
/// Player, Enemy ��� ����.
/// </summary>
[CreateAssetMenu(fileName = "GenericStatusEffectLogic", menuName = "StatusLogic/GenericStatusEffectLogic")]
public class GenericStatusEffectLogic : StatusEffectLogic
{
    public override void OnApply(EffectHandler handler, StatusEffectInstance instance)
    {
        var target = handler.gameObject;
        var data = instance.BaseEffect;

        if (target == null || data == null) return;

        switch (data.effectType)
        {
            case StatusEffectType.Buff:
                ApplyBuff(target, data, instance);
                break;

            case StatusEffectType.Debuff:
                ApplyDebuff(target, data, instance);
                break;

            case StatusEffectType.DamageOverTime:
                // DOT�� ���� �� Ư���� ó���� �ʿ� ���� (�� ���۸��� �۵�)
                break;
        }
    }

    public override void OnTurnStart(EffectHandler handler, StatusEffectInstance instance)
    {
        var target = handler.gameObject;
        var data = instance.BaseEffect;

        if (target == null || data == null) return;

        if (data.effectType == StatusEffectType.DamageOverTime)
        {
            ApplyDotDamage(target, data, instance);
        }
    }

    public override void OnExpire(EffectHandler handler, StatusEffectInstance instance)
    {
        var target = handler.gameObject;
        var data = instance.BaseEffect;

        if (target == null || data == null) return;

        switch (data.effectType)
        {
            case StatusEffectType.Buff:
                RemoveBuff(target, data, instance);
                break;

            case StatusEffectType.Debuff:
                RemoveDebuff(target, data, instance);
                break;

            case StatusEffectType.DamageOverTime:
                // DOT�� ���� �� ���� ó�� ����
                break;
        }
    }

    // ------------------------
    // ���� ó��
    // ------------------------

    private void ApplyBuff(GameObject target, StatusEffect effect, StatusEffectInstance instance)
    {
        // ��: �������� (flatBonusDamage) �� ���ݷ� ���
        var player = target.GetComponent<Player>();
        if (player != null)
        {
            player.strength += effect.flatBonusDamage * instance.StackCount;
            Debug.Log($"[����] {player.playerName}�� ���� ����: +{effect.flatBonusDamage * instance.StackCount}");
        }

        var enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.AddShield(effect.flatBonusDamage * instance.StackCount); // ����: ���� ���ݷ��� �ƴ϶� �ǵ带 ��´�
            Debug.Log($"[����] {enemy.enemyName}�� �ǵ带 ����: +{effect.flatBonusDamage * instance.StackCount}");
        }
    }

    private void RemoveBuff(GameObject target, StatusEffect effect, StatusEffectInstance instance)
    {
        var player = target.GetComponent<Player>();
        if (player != null)
        {
            player.strength -= effect.flatBonusDamage * instance.StackCount;
            Debug.Log($"[���� ����] {player.playerName}�� ���� ����: -{effect.flatBonusDamage * instance.StackCount}");
        }

        var enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.AddShield(-effect.flatBonusDamage * instance.StackCount);
            Debug.Log($"[���� ����] {enemy.enemyName}�� �ǵ� ����: {-effect.flatBonusDamage * instance.StackCount}");
        }
    }

    private void ApplyDebuff(GameObject target, StatusEffect effect, StatusEffectInstance instance)
    {
        var player = target.GetComponent<Player>();
        var enemy = target.GetComponent<Enemy>();

        if (player != null)
        {
            Debug.Log($"[����� ����] {player.playerName}���� {effect.effectName} �ο��� (�߰� ������ ����)");
        }
        else if (enemy != null)
        {
            Debug.Log($"[����� ����] {enemy.enemyName}���� {effect.effectName} �ο��� (�߰� ������ ����)");
        }
        else
        {
            Debug.LogWarning($"[����� ���� ����] ����� Player�� Enemy�� �ƴ� ({target.name})");
        }
    }

    private void RemoveDebuff(GameObject target, StatusEffect effect, StatusEffectInstance instance)
    {
        // ����ȭ: ����� ���� �� �߰� ó�� ���� ������� �ڿ� ����
    }

    private void ApplyDotDamage(GameObject target, StatusEffect effect, StatusEffectInstance instance)
    {
        var player = target.GetComponent<Player>();
        var enemy = target.GetComponent<Enemy>();

        int dotDamage = Mathf.RoundToInt(10 * effect.dotPercentDamage * instance.StackCount);
        if (dotDamage <= 0) dotDamage = 1;

        if (player != null)
        {
            player.TakeDamage(dotDamage);
            Debug.Log($"[DOT] {player.playerName}�� DOT�� {dotDamage} ���ظ� ����");
        }
        else if (enemy != null)
        {
            enemy.TakeDamage(dotDamage);
            Debug.Log($"[DOT] {enemy.enemyName}�� DOT�� {dotDamage} ���ظ� ����");
        }
        else
        {
            Debug.LogWarning($"[DOT ���� ����] ����� Player�� Enemy�� �ƴ� ({target.name})");
        }
    }
}