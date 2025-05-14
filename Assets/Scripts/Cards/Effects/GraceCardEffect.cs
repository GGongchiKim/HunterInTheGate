using UnityEngine;

[CreateAssetMenu(fileName = "NewGraceCardEffect", menuName = "Card Effects/GraceCardEffect")]
public class GraceCardEffect : CardEffect
{
    [Header("수치")]
    public int healAmount = 0;
    public int shieldAmount = 0;

    [Header("능력치 보정 배율")]
    public float magicMultiplier = 1.0f;
    public float willPowerMultiplier = 0.0f;

    public override bool AllowsGlobalDrop() => false;

    public override bool IsValidTarget(GameObject target)
    {
        return target != null && target.CompareTag("Player");
    }

    public override bool ExecuteEffect(GameContext context, CardData cardData, GameObject target = null)
    {
        if (context.player == null)
        {
            Debug.LogWarning("[GraceCard] 플레이어가 존재하지 않습니다.");
            return false;
        }

        if (context.player.actionPoints < cardData.cardCost)
        {
            Debug.LogWarning("[GraceCard] 행동력 부족으로 카드 사용 실패");
            return false;
        }

        // 🔹 행동력 소모
        context.player.actionPoints -= cardData.cardCost;
        HUDManager.Instance.UpdateActionPoints(context.player.actionPoints);

        // 🔹 숙련도 보정
        int level = context.player.GetCardLevel(cardData);
        float levelBonus = (level == 2) ? 1.1f : (level == 3 ? 1.25f : 1f);

        int finalHeal = Mathf.RoundToInt((healAmount + context.player.magic * magicMultiplier) * levelBonus);
        int finalShield = Mathf.RoundToInt((shieldAmount + context.player.willPower * willPowerMultiplier) * levelBonus);

        // 🔹 회복
        if (finalHeal > 0)
        {
            context.player.Heal(finalHeal);
            Debug.Log($"[GraceCard] 회복: {finalHeal}");
        }

        // 🔹 실드
        if (finalShield > 0)
        {
            context.player.AddShield(finalShield);
            context.player.PlayDefenseAnimation();
            Debug.Log($"[GraceCard] 실드: {finalShield}");
        }

        // 🔹 버프 적용 (명시적으로 데미지 0 전달)
        if (statusEffect != null)
        {
            ApplyStatusEffect(context.player.gameObject, statusEffect, 0);
        }

        return true;
    }
}