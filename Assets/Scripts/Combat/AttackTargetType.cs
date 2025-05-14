using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackTargetType
{
    Single,         // 단일 대상 (context.selectedEnemy)
    All,            // 전체 적
    RandomMultiple  // 랜덤 다중 대상 (공격 횟수만큼 랜덤하게 선택)
}
