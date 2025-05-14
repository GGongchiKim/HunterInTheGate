using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyActionPattern
{
    [Header("패턴 이름 (내부 식별용)")]
    public string patternName;

    [Header("패턴 설명 (UI 출력용)")]
    [TextArea(2, 4)]
    public string description;

    [Header("공격력")]
    public int damage;

    [Header("방어력 (실드)")]
    public int shield;

    [Header("적용할 디버프 (선택)")]
    public StatusEffect statusEffect;

    [Header("패턴 UI 아이콘")]
    public Sprite icon;

    [Header("보스전용액션 애니메이션 트리거")]
    public string animationTrigger;

    /// <summary>
    /// 플레이어에게 이 행동을 실행합니다.
    /// </summary>
    public void Execute(Player player)
    {
        if (player == null)
        {
            Debug.LogWarning($"[{patternName}] 실행 실패 - 플레이어가 null입니다.");
            return;
        }

        // 🔹 공격
        if (damage > 0)
        {
            player.TakeDamage(damage);
            player.PlayAttackedAnimation(0.2f); // 간단한 공격 리액션 연출 추가
        }

        // 🔹 방어
        if (shield > 0)
        {
            // 현재 패턴에는 적 실드 적용 로직은 없음. 필요시 적에게 적용 추가 가능
        }

        // 🔹 상태이상 적용 (공격 이후 적용)
        if (statusEffect != null)
        {
            player.ApplyEffect(statusEffect);
            Debug.Log($"[{patternName}] 상태이상 {statusEffect.effectName} 적용됨");
        }

        Debug.Log($"적 패턴 [{patternName}] 실행 완료");
    }
}