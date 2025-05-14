using UnityEngine;

/// <summary>
/// 상태효과 실행 로직을 정의하는 추상 클래스.
/// 각각의 상태효과는 이를 상속하여 구체적 적용/유지/종료 로직을 구현한다.
/// 현재 기본은 GenericStatusEffectLogic 사용.
/// </summary>
public abstract class StatusEffectLogic : ScriptableObject
{
    /// <summary>
    /// 상태효과가 처음 적용될 때 호출된다.
    /// ex: 버프 능력치 부여, 디버프 즉시 적용 등
    /// </summary>
    /// <param name="handler">상태효과를 보유한 대상의 EffectHandler</param>
    /// <param name="instance">현재 적용된 상태효과 인스턴스 데이터</param>
    public abstract void OnApply(EffectHandler handler, StatusEffectInstance instance);

    /// <summary>
    /// 대상의 턴 시작 시 호출된다.
    /// ex: DOT 데미지 처리, 버프/디버프 효과 지속
    /// </summary>
    /// <param name="handler">상태효과를 보유한 대상의 EffectHandler</param>
    /// <param name="instance">현재 적용된 상태효과 인스턴스 데이터</param>
    public abstract void OnTurnStart(EffectHandler handler, StatusEffectInstance instance);

    /// <summary>
    /// 상태효과가 만료되거나 제거될 때 호출된다.
    /// ex: 버프 효과 해제, 디버프 종료 처리
    /// </summary>
    /// <param name="handler">상태효과를 보유한 대상의 EffectHandler</param>
    /// <param name="instance">현재 적용된 상태효과 인스턴스 데이터</param>
    public abstract void OnExpire(EffectHandler handler, StatusEffectInstance instance);
}