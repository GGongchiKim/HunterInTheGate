using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("기본 스탯")]
    public string playerName;
    public int health;
    public int maxHealth;
    private int _currentShield;
    public int currentShield
    {
        get => _currentShield;
        set
        {
            _currentShield = value;
            HUDManager.Instance?.UpdatePlayerHealth(health, maxHealth, _currentShield);
        }
    }

    [Header("능력치")]
    public int strength;
    public int agility;
    public int magic;
    public int insight;
    public int willPower;
    public int wit;
    public int charisma;
    public int luck;

    [Header("행동력")]
    public int actionPoints;
    public int maxActionPoints;

    [Header("숙련도 시스템")]
    public Dictionary<CardData, CardProgress> cardProgressMap = new();

    [Header("컴포넌트")]
    private Animator animator;
    public EffectHandler effectHandler;

    // 🔹 기본 생성자
    public Player() { }

    public Player(string name, int health, int strength, int agility, int magic, int insight, int willPower, int wit, int charisma, int luck)
    {
        this.playerName = name;
        this.health = this.maxHealth = health;
        this.strength = strength;
        this.agility = agility;
        this.magic = magic;
        this.insight = insight;
        this.willPower = willPower;
        this.wit = wit;
        this.charisma = charisma;
        this.luck = luck;

        this.maxActionPoints = 5;
        this.actionPoints = maxActionPoints;
        this.cardProgressMap = new Dictionary<CardData, CardProgress>();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        effectHandler = GetComponent<EffectHandler>();
    }

    private void Start()
    {
        health = maxHealth = 100;
    }

    // -----------------
    // 카드 숙련도 관련
    // -----------------

    public int GetCardLevel(CardData card)
    {
        if (card == null)
        {
            Debug.LogWarning("GetCardLevel() - cardData가 null입니다.");
            return 1;
        }

        if (!cardProgressMap.ContainsKey(card))
        {
            cardProgressMap[card] = new CardProgress { cardData = card, usageCount = 0 };
        }

        return cardProgressMap[card].GetLevel();
    }

    public void RegisterCardUsage(CardData card)
    {
        if (card == null)
        {
            Debug.LogWarning("RegisterCardUsage() - cardData가 null입니다.");
            return;
        }

        if (!cardProgressMap.ContainsKey(card))
        {
            cardProgressMap[card] = new CardProgress { cardData = card, usageCount = 0 };
        }

        cardProgressMap[card].usageCount++;
        Debug.Log($"[{card.cardName}] 사용됨 → 숙련도 {cardProgressMap[card].usageCount}회");
    }

    // -----------------
    // 상태 관리
    // -----------------

    public void ResetActionPoints()
    {
        actionPoints = maxActionPoints;
        Debug.Log($"{playerName} resets action points to {actionPoints}");
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"🧩 [TakeDamage 시작] 순수 Damage: {damage}");

        int modifiedDamage = damage;

        // 🔹 현재 걸린 디버프 상태 출력
        if (effectHandler != null)
        {
            var activeEffects = effectHandler.GetActiveEffects();
            Debug.Log($"[TakeDamage] 현재 걸린 상태효과 수: {activeEffects.Count}");

            foreach (var effect in activeEffects)
            {
                Debug.Log($"- 걸린 효과: {effect.BaseEffect.effectName} (타입: {effect.BaseEffect.effectType})");
            }

            foreach (var effect in activeEffects)
            {
                if (effect.BaseEffect.effectType == StatusEffectType.Debuff)
                {
                    float multiplier = effect.BaseEffect.receivedDamageMultiplier;
                    if (multiplier > 0f)
                    {
                        modifiedDamage = Mathf.RoundToInt(modifiedDamage * (1f + multiplier));
                        Debug.Log($"[TakeDamage] 디버프 {effect.BaseEffect.effectName} 적용됨 → 최종 피해 {modifiedDamage}");
                        break;
                    }
                }
            }
        }

        int damageAfterShield = modifiedDamage;

        if (currentShield > 0)
        {
            int shieldUsed = Mathf.Min(currentShield, damageAfterShield);
            currentShield -= shieldUsed;
            damageAfterShield -= shieldUsed;

            Debug.Log($"[TakeDamage] 실드 {shieldUsed} 소모 → 남은 실드 {currentShield}");
        }

        if (damageAfterShield > 0)
        {
            health -= damageAfterShield;
            Debug.Log($"[TakeDamage] 최종 체력 감소: {damageAfterShield} → HP: {health}/{maxHealth}");
        }

        HUDManager.Instance.UpdatePlayerHealth(health, maxHealth, currentShield);

        PlayAttackedAnimation(0.28f);

        if (health <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
        HUDManager.Instance.UpdatePlayerHealth(health, maxHealth);
        Debug.Log($"{playerName} heals {amount}. Current HP: {health} / {maxHealth}");
    }

    public void AddShield(int amount)
    {
        currentShield += amount;
        Debug.Log($"{playerName} gains {amount} shield. Total Shield: {currentShield}");
    }

    public void ResetShield()
    {
        currentShield = 0;
        Debug.Log($"{playerName}'s shield is reset.");
    }

    private void Die()
    {
        animator?.SetTrigger("OnDeath");
        StartCoroutine(ShowDefeatAfterDelay(1.2f));
    }

    private IEnumerator ShowDefeatAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HUDManager.Instance?.ShowDefeatPanel();
        Time.timeScale = 0f;
    }

    // -----------------
    // 상태이상 처리
    // -----------------

    public void ApplyEffect(StatusEffect effect, int sourceDamage = 0)
    {
        if (effectHandler != null)
        {
            effectHandler.AddEffect(effect, sourceDamage);
            Debug.Log($"[ApplyEffect] {playerName}에 {effect.effectName} 적용됨 (sourceDamage={sourceDamage})");
        }
        else
        {
            Debug.LogWarning($"[ApplyEffect] {playerName}에 EffectHandler가 없습니다.");
        }
    }

    // -----------------
    // 애니메이션 관련
    // -----------------

    public void PlayAttackAnimation()
    {
        animator?.SetTrigger("OnAttack");
    }

    public void PlayDefenseAnimation()
    {
        animator?.SetTrigger("OnDefense");
    }

    public void PlayAttackedAnimation(float delay = 0.28f)
    {
        StartCoroutine(DelayedAttackedCoroutine(delay));
    }

    private IEnumerator DelayedAttackedCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator?.SetTrigger("OnAttacked");
    }
}