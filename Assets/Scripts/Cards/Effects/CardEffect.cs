using UnityEngine;

/// <summary>
/// 모든 카드 효과의 기본 추상 클래스 (ScriptableObject 기반)
/// 하위 클래스는 반드시 ExecuteEffect, IsValidTarget을 구현해야 함.
/// 추가로 StatusEffect를 통합하여 상태이상 적용도 지원.
/// </summary>
public abstract class CardEffect : ScriptableObject
{
    [Header("상태이상 옵션 (선택)")]
    public StatusEffect statusEffect; // 카드 효과와 함께 적용할 수 있는 상태이상 (없으면 무시)

    /// <summary>
    /// 카드 효과를 실행하고 성공 여부 반환 (true: 발동됨, false: 실패)
    /// </summary>
    public abstract bool ExecuteEffect(CombatContext context, CardData cardData, GameObject target = null);

    /// <summary>
    /// 카드가 전역 드롭을 허용하는지 여부 반환 (기본은 false)
    /// 전역 카드만 이 메서드를 override하여 true 반환
    /// </summary>
    public virtual bool AllowsGlobalDrop()
    {
        return false;
    }

    /// <summary>
    /// 해당 GameObject가 이 카드의 유효한 대상인지 여부
    /// 예: 공격카드는 Enemy만, 회복카드는 Player만 대상으로 유효 처리
    /// </summary>
    public abstract bool IsValidTarget(GameObject target);

    /// <summary>
    /// 상태이상(StatusEffect)을 대상(target)에게 적용하는 공통 메서드
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

        Debug.LogWarning($"[CardEffect] {target.name}에 상태효과를 적용할 수 없습니다. (Player/Enemy 컴포넌트 없음)");
    }
}