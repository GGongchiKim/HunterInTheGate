using System.Collections;
using System.Collections.Generic;
using SystemManager;
using TMPro;
using UnityEngine;

public class DateDisplayUpdater : MonoBehaviour
{
    [System.Serializable]
    public class DateTextGroup
    {
        public TMP_Text yearText;
        public TMP_Text monthText;
        public TMP_Text dayText;
        public TMP_Text dayNameText;
        public TMP_Text weekText;
    }

    [Header("날짜 텍스트 쌍 (복수 UI 지원)")]
    public List<DateTextGroup> dateTextGroups = new();

    [Header("D-day 텍스트 쌍")]
    public List<TMP_Text> ddayTexts = new();

    private readonly string[] dayNames = { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };

    public void UpdateDateUI()
    {
        var mgr = GameDateManager.Instance;

        foreach (var group in dateTextGroups)
        {
            if (group.yearText != null)
                group.yearText.text = mgr.CurrentYear.ToString();
            if (group.monthText != null)
                group.monthText.text = mgr.CurrentMonth.ToString();
            if (group.dayText != null)
                group.dayText.text = mgr.CurrentDayInMonth.ToString();
            if (group.dayNameText != null)
                group.dayNameText.text = dayNames[mgr.CurrentDayIndex % 7];
            if (group.weekText != null)
                group.weekText.text = $"{mgr.CurrentWeekInMonth} week";
        }
    }

    public void UpdateDdayUI()
    {
        if (MissionSystem.GateMissionManager.Instance == null) return;

        string text = "-";
        if (MissionSystem.GateMissionManager.Instance.HasActiveMission)
        {
            int currentWeek = GameDateManager.Instance.CurrentWeekIndex;
            int deadline = MissionSystem.GateMissionManager.Instance.CurrentMissionDeadline;
            int dDay = Mathf.Max(0, deadline - currentWeek)*7;
            text = $"D-{dDay}";
        }

        foreach (var ddayText in ddayTexts)
        {
            if (ddayText != null)
                ddayText.text = text;
        }
    }

    public IEnumerator AnimateDateChange(float duration = 0.5f)
    {
        // 날짜 진행 전 상태 저장
        var before = GameDateManager.Instance.GetFullDate(); // 오늘 날짜
        int beforeWeek = GameDateManager.Instance.CurrentWeekIndex;

        //  D-day 감소 연출용 시작값을 날짜 증가 전에 미리 계산
        int missionDeadline = MissionSystem.GateMissionManager.Instance.CurrentMissionDeadline;
        int fromDDay = Mathf.Max(0, missionDeadline - beforeWeek) * 7;

        // 날짜 진행
        GameDateManager.Instance.AdvanceWeek(); // 또는 AdvanceDay()

        // 날짜 진행 후 상태
        var after = GameDateManager.Instance.GetFullDate();
        int afterWeek = GameDateManager.Instance.CurrentWeekIndex;
        int toDDay = Mathf.Max(0, missionDeadline - afterWeek) * 7;

        int fromDay = before.day;
        int toDay = after.day;

        int fromWeek = beforeWeek;
        int toWeek = afterWeek;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / duration);

            int curDay = Mathf.RoundToInt(Mathf.Lerp(fromDay, toDay, progress));
            int curWeek = Mathf.RoundToInt(Mathf.Lerp(fromWeek, toWeek, progress));
            int curDDay = Mathf.RoundToInt(Mathf.Lerp(fromDDay, toDDay, progress));

            foreach (var group in dateTextGroups)
            {
                if (group.dayText != null)
                    group.dayText.text = curDay.ToString();
                if (group.weekText != null)
                    group.weekText.text = $"{curWeek} week";
            }

            foreach (var ddayText in ddayTexts)
            {
                if (ddayText != null)
                    ddayText.text = $"D-{Mathf.Max(curDDay, 0)}";
            }

            yield return null;
        }

        UpdateAll(); // 보정
    }




    public void UpdateAll()
    {
        UpdateDateUI();
        UpdateDdayUI();
    }
}