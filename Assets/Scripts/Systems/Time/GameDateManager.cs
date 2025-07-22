using System;
using UnityEngine;

namespace SystemManager
{
    public class GameDateManager : MonoBehaviour
    {
        public static GameDateManager Instance { get; private set; }

        public event Action<int> OnWeekAdvanced;  // 주차 변경 시
        public event Action<int> OnDayAdvanced;   // 일자 변경 시

        private const int StartYear = 2048; //시작년도
        private const int WeeksPerMonth = 4;
        private const int MonthsPerYear = 12;
        private const int DaysPerWeek = 7;
        private const int DaysPerMonth = WeeksPerMonth * DaysPerWeek;
        private const int TotalWeeks = 136;
        private const int TotalDays = TotalWeeks * DaysPerWeek;

        private int currentDayIndex = 0; // 0부터 시작 (0 = 1년차 3월 1일)

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

        // 날짜 조회용 프로퍼티
        public int CurrentDayIndex => currentDayIndex;
        public int CurrentWeekIndex => currentDayIndex / DaysPerWeek + 1;
        public int GetTotalDaysPassed() => currentDayIndex;

        public int CurrentYear
        {
            get
            {
                int totalMonths = currentDayIndex / DaysPerMonth;
                return StartYear + (totalMonths + 2) / MonthsPerYear;
            }
        }

        public int CurrentMonth
        {
            get
            {
                int totalMonths = currentDayIndex / DaysPerMonth;
                return (totalMonths + 2) % MonthsPerYear + 1;
            }
        }

        public int CurrentDayInMonth => (currentDayIndex % DaysPerMonth) + 1;
        public int CurrentWeekInMonth => (CurrentWeekIndex - 1) % WeeksPerMonth + 1;

        public string GetFormattedDate() => $"{CurrentYear}년 {CurrentMonth}월 {CurrentWeekInMonth}주차";
        public string GetFormattedFullDate() => $"{CurrentYear}년 {CurrentMonth}월 {CurrentDayInMonth}일";

        public (int year, int month, int day) GetFullDate(int offset = 0)
        {
            int totalDays = currentDayIndex + offset;
            int totalMonths = totalDays / DaysPerMonth;
            int day = (totalDays % DaysPerMonth) + 1;
            int month = (totalMonths + 2) % MonthsPerYear + 1;
            int year = StartYear + (totalMonths + 2) / MonthsPerYear;
            return (year, month, day);
        }

        // 날짜 진행
        public void AdvanceWeek()
        {
            AdvanceDays(DaysPerWeek);
        }

        public void AdvanceDay()
        {
            AdvanceDays(1);
        }

        public void AdvanceDays(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (currentDayIndex >= TotalDays) return;

                currentDayIndex++;
                OnDayAdvanced?.Invoke(currentDayIndex);

                if (currentDayIndex % DaysPerWeek == 0)
                {
                    OnWeekAdvanced?.Invoke(CurrentWeekIndex);
                }
            }

            Debug.Log($"[GameDateManager] 날짜 진행: {GetFormattedFullDate()} ({GetFormattedDate()})");
        }

        public void SetDay(int dayIndex)
        {
            currentDayIndex = Mathf.Clamp(dayIndex, 0, TotalDays - 1);
        }

        public void SetWeek(int weekIndex)
        {
            SetDay((weekIndex - 1) * DaysPerWeek);
        }

        // 휴일 체크 (추후 데이터 기반으로 전환 가능)
        public bool IsHoliday(int dayIndex)
        {
            // 예시: 4의 배수날짜는 휴일로 간주
            return (dayIndex + 1) % 14 == 0;
        }

        public bool HasHolidayInWeek(int weekIndex)
        {
            int start = (weekIndex - 1) * DaysPerWeek;
            for (int i = 0; i < DaysPerWeek; i++)
            {
                if (IsHoliday(start + i))
                    return true;
            }
            return false;
        }

        public bool IsFinalWeek() => CurrentWeekIndex >= TotalWeeks;
    }
}