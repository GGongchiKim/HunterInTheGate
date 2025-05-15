using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 아이템 카드: 1회용. 공격, 회복, 버프/디버프 등 다양한 기능 조합 가능
/// </summary>
[CreateAssetMenu(fileName = "NewItemCardEffect", menuName = "Card Effects/ItemCardEffect")]
public class ItemCardEffect : CardEffect
{
    [Header("공격 관련")]
    public AttackTargetType attackTarget = AttackTargetType.Single;
    public int damage = 0;

    [Header("회복 및 방어")]
    public int heal = 0;
    public int shield = 0;

    [Header("기타 효과")]
    public int drawCards = 0;
    public int energyRestore = 0;

    public override bool AllowsGlobalDrop() => attackTarget == AttackTargetType.All;

    public override bool IsValidTarget(GameObject target)
    {
        if (target == null)
            return AllowsGlobalDrop();

        if (attackTarget == AttackTargetType.Single && target.CompareTag("Enemy"))
            return true;

        if ((heal > 0 || shield > 0 || statusEffect != null) && target.CompareTag("Player"))
            return true;

        return false;
    }

    public override bool ExecuteEffect(CombatContext context, CardData cardData, GameObject target = null)
    {
        if (context.player == null)
        {
            Debug.LogWarning("[ItemCard] 플레이어가 존재하지 않습니다.");
            return false;
        }

        if (context.player.actionPoints < cardData.cardCost)
        {
            Debug.LogWarning("[ItemCard] 행동력 부족으로 카드 사용 실패");
            return false;
        }

        context.player.actionPoints -= cardData.cardCost;
        HUDManager.Instance.UpdateActionPoints(context.player.actionPoints);

        // 🔹 공격 처리
        if (damage > 0)
        {
            List<Enemy> targets = new();

            if (attackTarget == AttackTargetType.Single && target != null)
            {
                Enemy enemy = target.GetComponent<Enemy>() ?? target.GetComponentInParent<Enemy>();
                if (enemy != null)
                    targets.Add(enemy);
            }
            else if (attackTarget == AttackTargetType.All)
            {
                targets.AddRange(context.allEnemies);
            }

            foreach (var enemy in targets)
            {
                enemy.TakeDamage(damage);

                if (statusEffect != null)
                    ApplyStatusEffect(enemy.gameObject, statusEffect, damage);
            }

            if (context.selectedEnemy != null)
                context.selectedEnemy.PlayAttackedAnimation(0.28f);
        }

        // 🔹 회복
        if (heal > 0)
        {
            context.player.Heal(heal);
            Debug.Log($"[ItemCard] 회복: {heal}");
        }

        // 🔹 실드
        if (shield > 0)
        {
            context.player.AddShield(shield);
            Debug.Log($"[ItemCard] 실드: {shield}");
        }

        // 🔹 플레이어 상태이상 부여 (비공격 조건)
        if ((heal > 0 || shield > 0 || (statusEffect != null && damage == 0)) && statusEffect != null)
        {
            ApplyStatusEffect(context.player.gameObject, statusEffect, 0);
        }

        // 🔹 카드 드로우
        if (drawCards > 0)
        {
            TurnManager.Instance.DrawExtraCards(drawCards);
            Debug.Log($"[ItemCard] 드로우: {drawCards}장");
        }

        // 🔹 행동력 회복
        if (energyRestore > 0)
        {
            context.player.actionPoints = Mathf.Min(context.player.actionPoints + energyRestore, context.player.maxActionPoints);
            HUDManager.Instance.UpdateActionPoints(context.player.actionPoints);
            Debug.Log($"[ItemCard] 행동력 {energyRestore} 회복");
        }

        // 🔹 소모형 → 제거
        DeckManager.Instance.ExhaustCard(cardData);

        return true;
    }
}