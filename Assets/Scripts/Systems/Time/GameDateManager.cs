using System;
using UnityEngine;


namespace SystemManager
{
    public class GameDateManager : MonoBehaviour
    {
        public static GameDateManager Instance { get; private set; }

        public event Action<int> OnWeekAdvanced; // 추가: 주차가 바뀔 때 알림

        private const int WeeksPerMonth = 4;
        private const int MonthsPerYear = 12;
        private const int DaysPerWeek = 7;
        private const int DaysPerMonth = WeeksPerMonth * DaysPerWeek;
        private const int TotalWeeks = 136;

        private int currentWeekIndex = 1; // 1~136

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public int CurrentWeekIndex => currentWeekIndex;
        public int CurrentDayIndex => (currentWeekIndex - 1) * DaysPerWeek;
        public int GetTotalDaysPassed() => CurrentDayIndex;

        public (int year, int month, int day) GetFullDate(int offset = 0)
        {
            int totalDays = CurrentDayIndex + offset;
            int baseMonth = 3;
            int baseYear = 1;

            int totalMonths = totalDays / DaysPerMonth;
            int day = (totalDays % DaysPerMonth) + 1;

            int month = (baseMonth + totalMonths - 1) % MonthsPerYear + 1;
            int year = baseYear + (baseMonth + totalMonths - 1) / MonthsPerYear;

            return (year, month, day);
        }

        public string GetFormattedFullDate(int offset = 0)
        {
            var (year, month, day) = GetFullDate(offset);
            return $"{year}년 {month}월 {day}일";
        }

        public int CurrentYear
        {
            get
            {
                int monthIndex = (currentWeekIndex - 1) / WeeksPerMonth + 3;
                return 1 + (monthIndex - 1) / MonthsPerYear;
            }
        }

        public int CurrentMonth
        {
            get
            {
                int monthIndex = (currentWeekIndex - 1) / WeeksPerMonth + 3;
                return ((monthIndex - 1) % MonthsPerYear) + 1;
            }
        }

        public int CurrentWeekInMonth => ((currentWeekIndex - 1) % WeeksPerMonth) + 1;

        public bool IsFinalWeek() => currentWeekIndex >= TotalWeeks;

        public void AdvanceWeek()
        {
            if (!IsFinalWeek())
            {
                currentWeekIndex++;
                Debug.Log($"[GameDateManager] 다음 주차로 진행됨: {GetFormattedDate()} ({GetFormattedFullDate()})");

                OnWeekAdvanced?.Invoke(currentWeekIndex); //  이벤트 호출
            }
            else
            {
                Debug.Log("[GameDateManager] 마지막 주차 도달");
            }
        }

        public void SetWeek(int week)
        {
            currentWeekIndex = Mathf.Clamp(week, 1, TotalWeeks);
        }

        public string GetFormattedDate()
        {
            return $"{CurrentYear}년 {CurrentMonth}월 {CurrentWeekInMonth}주차";
        }
    }
}