using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Base Data")]
    public EnemyData data;
    public string enemyName;

    [Header("Status")]
    public int health;
    public int maxHealth;
    public int currentShield;

    private int currentPatternIndex = 0;
    private bool usedSpecial = false;
    private bool isDead = false;

    [Header("Components")]
    private Animator animator;
    public EffectHandler effectHandler;
    public EnemyHUDHandler enemyHUD;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        effectHandler = GetComponent<EffectHandler>();
        enemyHUD = GetComponentInChildren<EnemyHUDHandler>();
    }

    public void Initialize(EnemyData data)
    {
        this.data = data;
        this.enemyName = data.enemyName;
        this.maxHealth = data.maxHealth;
        this.health = maxHealth;
        usedSpecial = false;
        currentPatternIndex = 0;
        isDead = false;

        enemyHUD?.UpdateIntent(GetNextPattern());
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        int modifiedDamage = damage;

        if (effectHandler != null)
        {
            foreach (var effect in effectHandler.GetActiveEffects())
            {
                if (effect.BaseEffect.effectType == StatusEffectType.Debuff)
                {
                    float multiplier = effect.BaseEffect.receivedDamageMultiplier;
                    if (multiplier > 1.0f)
                    {
                        modifiedDamage = Mathf.RoundToInt(modifiedDamage * multiplier);
                        Debug.Log($"[디버프 적용] {enemyName} 추가 피해 {multiplier}배 → 최종 {modifiedDamage} 피해");
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
        }

        if (damageAfterShield > 0)
        {
            health -= damageAfterShield;
        }

        enemyHUD?.UpdateHealth(health, maxHealth, currentShield);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            PlayAttackedAnimation();
        }
    }

    public void AddShield(int amount)
    {
        currentShield += amount;
        enemyHUD?.UpdateHealth(health, maxHealth, currentShield);
    }

    public void ApplyEffect(StatusEffect effect, int sourceDamage = 0)
    {
        if (effectHandler != null)
        {
            effectHandler.AddEffect(effect, sourceDamage);
            Debug.Log($"[Enemy] {enemyName}에 {effect.effectName} 효과 적용됨 (sourceDamage={sourceDamage})");
        }
        else
        {
            Debug.LogWarning($"[Enemy] {enemyName}에 EffectHandler가 없습니다.");
        }
    }

    public void PlayAttackAnimation() => animator?.SetTrigger("OnAttack");
    public void PlayDefenseAnimation() => animator?.SetTrigger("OnDefense");

    public void PlayAttackedAnimation(float delay = 0.28f)
    {
        StartCoroutine(DelayedAttackedCoroutine(delay));
    }

    private IEnumerator DelayedAttackedCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator?.SetTrigger("OnAttacked");
    }

    public EnemyActionPattern GetNextPattern()
    {
        if (data == null) return null;

        if (data.isBoss && !usedSpecial && health <= maxHealth / 2)
            return data.specialPattern;

        if (data.normalPatterns.Count > 0)
            return data.normalPatterns[currentPatternIndex];

        return null;
    }

    public void PerformTurn()
    {
        if (isDead || data == null) return;

        EnemyActionPattern patternToUse = GetNextPattern();

        if (patternToUse == data.specialPattern)
            usedSpecial = true;

        ExecutePattern(patternToUse);

        if (patternToUse != data.specialPattern && data.normalPatterns.Count > 0)
            currentPatternIndex = (currentPatternIndex + 1) % data.normalPatterns.Count;

        enemyHUD?.UpdateIntent(GetNextPattern());
    }

    private void ExecutePattern(EnemyActionPattern pattern)
    {
        if (pattern == null) return;

        Debug.Log($"{enemyName} uses [{pattern.patternName}]");

        if (pattern.damage > 0)
        {
            CombatContext.Instance.combatPlayer.TakeDamage(pattern.damage);
            PlayAttackAnimation();
        }

        if (pattern.shield > 0)
        {
            AddShield(pattern.shield);
            PlayDefenseAnimation();
        }

        if (pattern.statusEffect != null)
        {
            CombatContext.Instance.combatPlayer.ApplyEffect(pattern.statusEffect);
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        animator?.SetTrigger("OnDeath");
        enemyHUD?.HideHUD();

        // 💡 HUD 정보 정리
        C_HUDManager.Instance?.UnregisterEnemy(this);

        StartCoroutine(DelayedDeathCleanup(1.5f));
    }

    private IEnumerator DelayedDeathCleanup(float delay)
    {
        yield return new WaitForSeconds(delay);

        CombatContext.Instance.allEnemies.Remove(this);
        Destroy(gameObject);

        TurnManager.Instance.CheckVictoryCondition();
    }
}