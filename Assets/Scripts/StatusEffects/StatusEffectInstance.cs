using UnityEngine;

/// <summary>
/// ���� ����� ���� ����ȿ�� �ν��Ͻ��� ����.
/// (�����Ϳ� ���� ���� �и�)
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

    // -------- ����ȿ�� �����ֱ� --------

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

    // -------- ��/���� ���� --------

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

    // -------- �߰� ��ɿ� Ȯ�� ���� --------

    /// <summary>
    /// [Reserved] ���� ��Ʈ ������ ���� ����ϴ� ��ɿ� (���� ��� DOT ���� ����)
    /// ����� ������� ����. �⺻ ������ Ȯ��.
    /// </summary>
    public int CalculateDotDamage()
    {
        // Ȯ���: ���߿� InitialDamage, ���� ��, ���� ���� ���� ����
        return 0;
    }

    /// <summary>
    /// [Reserved] ���� ���� ���� ȿ���� ��ȭ�ϰų� Ư�� �ߵ� ������ üũ�ϴ� ��ɿ�
    /// ����� ������� ����. �⺻ ������ Ȯ��.
    /// </summary>
    public bool ShouldTriggerSpecialEffect()
    {
        // ��: ������ 5 �̻��̸� �߰� ȿ�� �ߵ� ��
        return false;
    }

    // -------- ����� ���Ǽ� --------

    public override string ToString()
    {
        return $"[{BaseEffect.effectName}] {StackCount}���� / {RemainingDuration}�� ����";
    }
}