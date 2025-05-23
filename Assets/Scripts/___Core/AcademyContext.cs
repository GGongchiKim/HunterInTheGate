using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 육성 씬 전용 컨텍스트. 임시 활동 정보 및 스탯 변화량, 보상 등을 보관하며 주간 마감 시 AcademyPlayer에 반영.
/// </summary>
public class AcademyContext : MonoBehaviour
{
    public static AcademyContext Instance { get; private set; }

    [Header("이번 주 선택 활동")]
    public string activityName;

    [Header("능력치 변화량")]
    public List<StatModifier> statModifiers = new List<StatModifier>();

    [Header("임시 보상")]
    public List<string> tempRewards = new List<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ResetWeeklyContext()
    {
        activityName = null;
        statModifiers.Clear();
        tempRewards.Clear();
    }

    public void SetActivityResult(string activity, List<StatModifier> modifiers, List<string> rewards = null)
    {
        activityName = activity;
        statModifiers = new List<StatModifier>(modifiers);
        tempRewards = rewards != null ? new List<string>(rewards) : new List<string>();
    }

    public void CommitResults(AcademyPlayer player)
    {
        player.ApplyModifiers(statModifiers);
        player.CollectRewards(tempRewards);
        A_HUDManager.Instance?.UpdateStatsUI();
        ResetWeeklyContext();
    }
}