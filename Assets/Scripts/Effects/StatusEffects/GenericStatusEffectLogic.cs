using UnityEngine;

/// <summary>
/// 모든 상태효과를 데이터 기반으로 처리하는 범용 상태효과 로직.
/// Player, Enemy 모두 지원.
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
                // DOT는 적용 시 특별히 처리할 필요 없음 (턴 시작마다 작동)
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
                // DOT는 만료 시 별도 처리 없음
                break;
        }
    }

    // ------------------------
    // 세부 처리
    // ------------------------

    private void ApplyBuff(GameObject target, StatusEffect effect, StatusEffectInstance instance)
    {
        // 예: 힘모으기 (flatBonusDamage) → 공격력 상승
        var player = target.GetComponent<CombatPlayer>();
        if (player != null)
        {
            player.combat.strength += effect.flatBonusDamage * instance.StackCount;
            Debug.Log($"[버프] {player.playerName}의 힘이 증가: +{effect.flatBonusDamage * instance.StackCount}");
        }

        var enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.AddShield(effect.flatBonusDamage * instance.StackCount); // 예시: 적은 공격력이 아니라 실드를 얻는다
            Debug.Log($"[버프] {enemy.enemyName}이 실드를 얻음: +{effect.flatBonusDamage * instance.StackCount}");
        }
    }

    private void RemoveBuff(GameObject target, StatusEffect effect, StatusEffectInstance instance)
    {
        var player = target.GetComponent<CombatPlayer>();
        if (player != null)
        {
            player.combat.strength -= effect.flatBonusDamage * instance.StackCount;
            Debug.Log($"[버프 종료] {player.playerName}의 힘이 감소: -{effect.flatBonusDamage * instance.StackCount}");
        }

        var enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.AddShield(-effect.flatBonusDamage * instance.StackCount);
            Debug.Log($"[버프 종료] {enemy.enemyName}의 실드 감소: {-effect.flatBonusDamage * instance.StackCount}");
        }
    }

    private void ApplyDebuff(GameObject target, StatusEffect effect, StatusEffectInstance instance)
    {
        var player = target.GetComponent<CombatPlayer>();
        var enemy = target.GetComponent<Enemy>();

        if (player != null)
        {
            Debug.Log($"[디버프 적용] {player.playerName}에게 {effect.effectName} 부여됨 (추가 데미지 없음)");
        }
        else if (enemy != null)
        {
            Debug.Log($"[디버프 적용] {enemy.enemyName}에게 {effect.effectName} 부여됨 (추가 데미지 없음)");
        }
        else
        {
            Debug.LogWarning($"[디버프 적용 실패] 대상이 Player도 Enemy도 아님 ({target.name})");
        }
    }

    private void RemoveDebuff(GameObject target, StatusEffect effect, StatusEffectInstance instance)
    {
        // 간단화: 디버프 만료 시 추가 처리 없이 디버프만 자연 종료
    }

    private void ApplyDotDamage(GameObject target, StatusEffect effect, StatusEffectInstance instance)
    {
        var player = target.GetComponent<CombatPlayer>();
        var enemy = target.GetComponent<Enemy>();

        int dotDamage = Mathf.RoundToInt(10 * effect.dotPercentDamage * instance.StackCount);
        if (dotDamage <= 0) dotDamage = 1;

        if (player != null)
        {
            player.TakeDamage(dotDamage);
            Debug.Log($"[DOT] {player.playerName}이 DOT로 {dotDamage} 피해를 입음");
        }
        else if (enemy != null)
        {
            enemy.TakeDamage(dotDamage);
            Debug.Log($"[DOT] {enemy.enemyName}이 DOT로 {dotDamage} 피해를 입음");
        }
        else
        {
            Debug.LogWarning($"[DOT 적용 실패] 대상이 Player도 Enemy도 아님 ({target.name})");
        }
    }
}