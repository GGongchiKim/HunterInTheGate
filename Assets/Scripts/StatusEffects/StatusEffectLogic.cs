using UnityEngine;

/// <summary>
/// ����ȿ�� ���� ������ �����ϴ� �߻� Ŭ����.
/// ������ ����ȿ���� �̸� ����Ͽ� ��ü�� ����/����/���� ������ �����Ѵ�.
/// ���� �⺻�� GenericStatusEffectLogic ���.
/// </summary>
public abstract class StatusEffectLogic : ScriptableObject
{
    /// <summary>
    /// ����ȿ���� ó�� ����� �� ȣ��ȴ�.
    /// ex: ���� �ɷ�ġ �ο�, ����� ��� ���� ��
    /// </summary>
    /// <param name="handler">����ȿ���� ������ ����� EffectHandler</param>
    /// <param name="instance">���� ����� ����ȿ�� �ν��Ͻ� ������</param>
    public abstract void OnApply(EffectHandler handler, StatusEffectInstance instance);

    /// <summary>
    /// ����� �� ���� �� ȣ��ȴ�.
    /// ex: DOT ������ ó��, ����/����� ȿ�� ����
    /// </summary>
    /// <param name="handler">����ȿ���� ������ ����� EffectHandler</param>
    /// <param name="instance">���� ����� ����ȿ�� �ν��Ͻ� ������</param>
    public abstract void OnTurnStart(EffectHandler handler, StatusEffectInstance instance);

    /// <summary>
    /// ����ȿ���� ����ǰų� ���ŵ� �� ȣ��ȴ�.
    /// ex: ���� ȿ�� ����, ����� ���� ó��
    /// </summary>
    /// <param name="handler">����ȿ���� ������ ����� EffectHandler</param>
    /// <param name="instance">���� ����� ����ȿ�� �ν��Ͻ� ������</param>
    public abstract void OnExpire(EffectHandler handler, StatusEffectInstance instance);
}