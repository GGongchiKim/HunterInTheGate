using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EffectHandler : MonoBehaviour
{
    private readonly List<StatusEffectInstance> activeEffects = new();

    [Header("UI 연결")]
    public Transform statusEffectPanel;
    public GameObject statusEffectIconPrefab;

    private readonly Dictionary<StatusEffectInstance, GameObject> effectIcons = new();
    private readonly Dictionary<StatusEffectInstance, TextMeshProUGUI> effectTexts = new();

    // ----------------------------
    // 상태이상 추가
    // ----------------------------
    public void AddEffect(StatusEffect newEffect) => AddEffect(newEffect, 0);

    public void AddEffect(StatusEffect newEffect, int sourceDamage)
    {
        StatusEffectInstance existing = activeEffects.Find(e => e.BaseEffect == newEffect);

        if (existing != null)
        {
            if (newEffect.canStack)
                existing.Stack();
            else
                existing.ResetDuration();
        }
        else
        {
            StatusEffectInstance instance = new(newEffect, sourceDamage);
            activeEffects.Add(instance);

            instance.OnApply(this);
            CreateEffectIcon(instance);
        }

        RefreshAllEffectIcons();
    }

    // ----------------------------
    // 턴 갱신
    // ----------------------------
    public void UpdateEffects()
    {
        List<StatusEffectInstance> expired = new();

        foreach (var effect in activeEffects)
        {
            effect.OnTurnStart(this);
            effect.DecreaseDuration();

            if (effect.IsExpired())
                expired.Add(effect);
        }

        foreach (var expiredEffect in expired)
            RemoveEffect(expiredEffect);

        RefreshAllEffectIcons();
    }

    // ----------------------------
    // 상태이상 제거
    // ----------------------------
    public void RemoveEffect(StatusEffectInstance effect)
    {
        if (!activeEffects.Contains(effect)) return;

        effect.OnExpire(this);
        activeEffects.Remove(effect);
        RemoveEffectIcon(effect);
    }

    public void ClearAllEffects()
    {
        foreach (var effect in activeEffects)
            effect.OnExpire(this);

        activeEffects.Clear();

        foreach (var icon in effectIcons.Values)
            Destroy(icon);

        effectIcons.Clear();
        effectTexts.Clear();
    }

    // ----------------------------
    // 조회
    // ----------------------------
    public List<StatusEffectInstance> GetActiveEffects() => activeEffects;

    // ----------------------------
    // UI 관련
    // ----------------------------
    private void CreateEffectIcon(StatusEffectInstance instance)
    {
        if (statusEffectPanel == null)
        {
            Debug.LogWarning($"[EffectHandler] 상태이상 패널이 비어 있습니다: {gameObject.name}");
            return;
        }

        if (statusEffectIconPrefab == null)
        {
            Debug.LogWarning($"[EffectHandler] 상태이상 아이콘 프리팹이 설정되지 않았습니다.");
            return;
        }

        GameObject iconObj = Instantiate(statusEffectIconPrefab, statusEffectPanel);
        iconObj.transform.localScale = Vector3.one;

        // 아이콘 이미지 설정
        var iconImage = iconObj.GetComponent<Image>();
        if (iconImage != null && instance.BaseEffect.icon != null)
        {
            iconImage.sprite = instance.BaseEffect.icon;
            iconImage.color = instance.BaseEffect.iconColor;
        }

        // 텍스트 처리
        var text = iconObj.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = instance.RemainingDuration.ToString();
            effectTexts[instance] = text;
        }

        effectIcons[instance] = iconObj;
    }

    private void RemoveEffectIcon(StatusEffectInstance instance)
    {
        if (effectIcons.TryGetValue(instance, out GameObject icon))
        {
            Destroy(icon);
            effectIcons.Remove(instance);
        }

        effectTexts.Remove(instance);
    }

    private void RefreshAllEffectIcons()
    {
        foreach (var kvp in effectTexts)
            kvp.Value.text = kvp.Key.RemainingDuration.ToString();
    }
}