using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectHandler : MonoBehaviour
{
    private List<StatusEffectInstance> activeEffects = new();

    [Header("UI 연결")]
    public Transform statusEffectPanel;
    public GameObject statusEffectIconPrefab;

    private Dictionary<StatusEffectInstance, GameObject> effectIcons = new();
    private Dictionary<StatusEffectInstance, TextMeshProUGUI> effectTexts = new();

    // --------------------------------
    // 상태이상 추가
    // --------------------------------

    public void AddEffect(StatusEffect newEffect)
    {
        AddEffect(newEffect, 0); // 기본 데미지 없이
    }

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
            StatusEffectInstance instance = new StatusEffectInstance(newEffect, sourceDamage);
            activeEffects.Add(instance);

            instance.OnApply(this);
            CreateEffectIcon(instance);
        }

        RefreshAllEffectIcons();
    }

    // --------------------------------
    // 턴 갱신
    // --------------------------------

    public void UpdateEffects()
    {
        List<StatusEffectInstance> expiredEffects = new();

        foreach (var effect in activeEffects)
        {
            effect.OnTurnStart(this);
            effect.DecreaseDuration();

            if (effect.IsExpired())
                expiredEffects.Add(effect);
        }

        foreach (var expired in expiredEffects)
        {
            RemoveEffect(expired);
        }

        RefreshAllEffectIcons();
    }

    // --------------------------------
    // 상태이상 제거
    // --------------------------------

    public void RemoveEffect(StatusEffectInstance effect)
    {
        if (activeEffects.Contains(effect))
        {
            effect.OnExpire(this);
            activeEffects.Remove(effect);
            RemoveEffectIcon(effect);
        }
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

    // --------------------------------
    // 현재 상태이상 조회
    // --------------------------------

    public List<StatusEffectInstance> GetActiveEffects() => activeEffects;

    // --------------------------------
    // UI 관련
    // --------------------------------

    private void CreateEffectIcon(StatusEffectInstance instance)
    {
        if (statusEffectPanel == null || statusEffectIconPrefab == null)
        {
            Debug.LogWarning("StatusEffectPanel 또는 IconPrefab이 설정되지 않았습니다.");
            return;
        }

        GameObject iconObj = Instantiate(statusEffectIconPrefab, statusEffectPanel);

        // 아이콘 이미지 설정
        var iconImage = iconObj.GetComponent<UnityEngine.UI.Image>();
        if (iconImage != null)
        {
            if (instance.BaseEffect.icon != null)
                iconImage.sprite = instance.BaseEffect.icon;

            // 🔹 아이콘 색상 적용
            iconImage.color = instance.BaseEffect.iconColor;
        }

        // 텍스트 처리
        var text = iconObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
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