using UnityEngine;

/// <summary>
/// 실제 적용된 개별 상태효과 인스턴스를 관리.
/// (데이터와 실행 로직 분리)
/// </summary>
public class StatusEffectInstance
{
    public StatusEffect BaseEffect { get; private set; }
    public StatusEffectLogic Logic { get; private set; }
    public int RemainingDuration { get; private set; }
    public int StackCount { get; private set; }
    public int InitialDamage { get; private set; }

    public StatusEffectInstance(StatusEffect baseEffect, int initialDamage = 0)
    {
        BaseEffect = baseEffect;
        Logic = baseEffect.CreateLogicInstance();
        RemainingDuration = baseEffect.defaultDuration;
        StackCount = 1;
        InitialDamage = initialDamage;
    }

    // -------- 상태효과 생명주기 --------

    public void OnApply(EffectHandler handler)
    {
        Logic?.OnApply(handler, this);
    }

    public void OnTurnStart(EffectHandler handler)
    {
        Logic?.OnTurnStart(handler, this);
    }

    public void OnExpire(EffectHandler handler)
    {
        Logic?.OnExpire(handler, this);
    }

    // -------- 턴/스택 관리 --------

    public void DecreaseDuration()
    {
        RemainingDuration--;
    }

    public bool IsExpired()
    {
        return RemainingDuration <= 0;
    }

    public void Stack()
    {
        if (BaseEffect.canStack)
        {
            IncreaseStack();
        }
        ResetDuration();
    }

    private void IncreaseStack()
    {
        StackCount++;
        if (BaseEffect.maxStack > 0)
            StackCount = Mathf.Min(StackCount, BaseEffect.maxStack);
    }

    public void ResetDuration()
    {
        RemainingDuration = BaseEffect.defaultDuration;
    }

    // -------- 추가 기능용 확장 슬롯 --------

    /// <summary>
    /// [Reserved] 매턴 도트 데미지 등을 계산하는 기능용 (추후 고급 DOT 공식 대응)
    /// 현재는 사용하지 않음. 기본 구조만 확보.
    /// </summary>
    public int CalculateDotDamage()
    {
        // 확장용: 나중에 InitialDamage, 스택 수, 별도 공식 적용 가능
        return 0;
    }

    /// <summary>
    /// [Reserved] 스택 수에 따라 효과를 강화하거나 특수 발동 조건을 체크하는 기능용
    /// 현재는 사용하지 않음. 기본 구조만 확보.
    /// </summary>
    public bool ShouldTriggerSpecialEffect()
    {
        // 예: 스택이 5 이상이면 추가 효과 발동 등
        return false;
    }

    // -------- 디버깅 편의성 --------

    public override string ToString()
    {
        return $"[{BaseEffect.effectName}] {StackCount}스택 / {RemainingDuration}턴 남음";
    }
}