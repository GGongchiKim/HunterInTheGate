using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewStrikeCardEffect", menuName = "Card Effects/StrikeCardEffect")]
public class StrikeCardEffect : CardEffect
{
    [Header("타겟 설정")]
    public AttackTargetType targetType = AttackTargetType.Single;

    [Header("피해 수치")]
    public int baseDamage = 10;
    public int attackCount = 1;

    [Header("능력치 보정 배율")]
    public float strengthMultiplier = 1.0f;
    public float agilityMultiplier = 0.0f;
    public float insightMultiplier = 0.0f;

    public override bool AllowsGlobalDrop() => targetType == AttackTargetType.All;

    public override bool IsValidTarget(GameObject target)
    {
        if (target == null) return targetType == AttackTargetType.All;
        return targetType == AttackTargetType.Single && target.CompareTag("Enemy");
    }

    public override bool ExecuteEffect(GameContext context, CardData cardData, GameObject target = null)
    {
        if (context.player == null || context.player.actionPoints < cardData.cardCost)
        {
            Debug.LogWarning("[StrikeCard] 행동력 부족 또는 플레이어 없음");
            return false;
        }

        context.player.actionPoints -= cardData.cardCost;
        HUDManager.Instance.UpdateActionPoints(context.player.actionPoints);

        List<Enemy> targets = new();
        if (targetType == AttackTargetType.Single && target != null)
        {
            Enemy enemy = target.GetComponent<Enemy>() ?? target.GetComponentInParent<Enemy>();
            if (enemy != null) targets.Add(enemy);
        }
        else if (targetType == AttackTargetType.All)
        {
            targets.AddRange(context.allEnemies);
        }

        if (targets.Count == 0)
        {
            Debug.LogWarning("[StrikeCard] 유효한 타겟 없음");
            return false;
        }

        int level = context.player.GetCardLevel(cardData);
        float levelBonus = (level == 2) ? 1.1f : (level == 3 ? 1.25f : 1f);

        int totalDamage = Mathf.RoundToInt(
            (baseDamage +
            context.player.strength * strengthMultiplier +
            context.player.agility * agilityMultiplier +
            context.player.insight * insightMultiplier)
            * levelBonus
        );

        foreach (var enemy in targets)
        {
            // 1) 데미지 먼저
            for (int i = 0; i < attackCount; i++)
                enemy.TakeDamage(totalDamage);

            // 2) 상태이상 부여 (필요시 totalDamage 넘겨주기)
            if (statusEffect != null)
                enemy.ApplyEffect(statusEffect, totalDamage);
        }

        context.player.PlayAttackAnimation();
        context.selectedEnemy?.PlayAttackedAnimation(0.28f);

        return true;
    }
}