using UnityEngine;

/// <summary>
/// 상태 효과 타입 (버프, 디버프, 지속피해 등)
/// </summary>
public enum StatusEffectType
{
    Buff,
    Debuff,
    DamageOverTime
}

/// <summary>
/// 상태 효과 데이터 (ScriptableObject)
/// 순수 데이터만 관리 (로직 실행은 StatusEffectLogic 쪽 담당)
/// </summary>
[CreateAssetMenu(fileName = "NewStatusEffect", menuName = "Status/StatusEffect")]
public class StatusEffect : ScriptableObject
{
    [Header("기본 정보")]
    public string effectId;                      // 고유 식별자 (ex: "BLEED", "POISON")
    public string effectName;                    // UI용 이름
    public StatusEffectType effectType;          // 버프/디버프/지속피해 구분
    [TextArea(2, 3)] 
    public string description;                   // 효과 설명
    
    [Header("UI 표시")]
    public Sprite icon;                           // UI용 아이콘
    public Color iconColor = Color.white;  // 아이콘 색상 (기본은 흰색)

    [Header("기본 속성")]
    public int defaultDuration = 3;              // 기본 지속 턴 수
    public bool canStack = false;                // 중첩 가능 여부
    public int maxStack = 0;                     // 0이면 무제한 스택 허용

    [Header("수치 옵션 (선택적)")]
    public int flatBonusDamage = 0;               // 고정 추가 피해 (ex: 힘모으기)
    public float percentBonusDamage = 0f;         // 퍼센트 기반 추가 피해 (ex: 연격)
    public float receivedDamageMultiplier = 1f;  // 받는 피해량 배율 (ex: 취약)
    public float givenDamageReduction = 0f;       // 주는 피해량 감소 비율 (ex: 약화)
    public float healReductionRate = 0f;          // 회복량 감소 비율 (ex: 화상)
    public float dotPercentDamage = 0f;           // DOT (지속피해) 비율 (ex: 출혈, 맹독)

    [Header("로직 연결 (필수)")]
    public StatusEffectLogic logicTemplate;       // 상태 효과 실행용 로직 (기본: GenericStatusEffectLogic)

    /// <summary>
    /// 이 StatusEffect를 기반으로 실행용 Logic 인스턴스를 생성한다.
    /// 실제 적용과 계산은 Logic에서 처리
    /// </summary>
    public StatusEffectLogic CreateLogicInstance()
    {
        if (logicTemplate != null)
        {
            return Instantiate(logicTemplate);
        }
        else
        {
            Debug.LogWarning($"[StatusEffect] {effectName}의 LogicTemplate이 설정되어 있지 않습니다.");
            return null;
        }
    }

    public override string ToString()
    {
        return $"[{effectId}] {effectName} - {effectType}, {defaultDuration}턴 {(canStack ? "(중첩 가능)" : "")}";
    }
}