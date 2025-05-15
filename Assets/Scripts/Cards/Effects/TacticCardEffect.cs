using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 전술형 카드 효과: 카드 드로우, 행동력 회복, 상태이상 부여 등
/// </summary>
[CreateAssetMenu(fileName = "NewTacticCardEffect", menuName = "Card Effects/TacticCardEffect")]
public class TacticCardEffect : CardEffect
{
    [Header("타겟 설정")]
    public AttackTargetType targetType = AttackTargetType.Single;

    [Header("효과 설정")]
    public int drawCards = 0;
    public int recoverAP = 0;

    public override bool AllowsGlobalDrop()
    {
        return targetType == AttackTargetType.All;
    }

    public override bool IsValidTarget(GameObject target)
    {
        if (target == null)
            return targetType == AttackTargetType.All;

        if (targetType == AttackTargetType.Single && target.CompareTag("Enemy"))
            return true;

        return false;
    }

    public override bool ExecuteEffect(CombatContext context, CardData cardData, GameObject target = null)
    {
        if (context.player == null)
        {
            Debug.LogWarning("[TacticCard] 플레이어가 존재하지 않습니다.");
            return false;
        }

        if (context.player.actionPoints < cardData.cardCost)
        {
            Debug.LogWarning("[TacticCard] 행동력 부족으로 카드 사용 실패");
            return false;
        }

        // 🔹 행동력 소모
        context.player.actionPoints -= cardData.cardCost;
        HUDManager.Instance.UpdateActionPoints(context.player.actionPoints);

        // 🔹 행동력 회복
        if (recoverAP > 0)
        {
            context.player.actionPoints = Mathf.Min(context.player.actionPoints + recoverAP, context.player.maxActionPoints);
            HUDManager.Instance.UpdateActionPoints(context.player.actionPoints);
            Debug.Log($"[TacticCard] 행동력 {recoverAP} 회복");
        }

        // 🔹 카드 드로우
        if (drawCards > 0)
        {
            TurnManager.Instance.DrawExtraCards(drawCards);
            Debug.Log($"[TacticCard] 카드 {drawCards}장 추가 드로우");
        }

        // 🔹 상태이상 적용 (필요할 때만)
        if (statusEffect != null)
        {
            List<Enemy> targets = new();

            if (targetType == AttackTargetType.Single && target != null)
            {
                Enemy enemy = target.GetComponent<Enemy>() ?? target.GetComponentInParent<Enemy>();
                if (enemy != null)
                    targets.Add(enemy);
            }
            else if (targetType == AttackTargetType.All)
            {
                targets.AddRange(context.allEnemies);
            }

            foreach (var enemy in targets)
            {
                ApplyStatusEffect(enemy.gameObject, statusEffect, 0); // ✅ 드로우 기반은 baseDamage=0
            }
        }

        return true;
    }
}