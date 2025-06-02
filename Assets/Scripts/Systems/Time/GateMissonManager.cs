using System;
using System.Collections.Generic;
using UnityEngine;
using SystemManager;

namespace MissionSystem
{
    public class GateMissionManager : MonoBehaviour
    {
        public static GateMissionManager Instance { get; private set; }

        [Header("미션 시작 주차 설정")]
        [SerializeField]
        private List<int> missionTriggerWeeks = new()
        {
            4, 16, 28, 40, 52, 64, 76, 88, 100, 112, 124, 136
        };

        [Header("미션 상태 추적")]
        private int failedCount = 0;
        private int lastTriggeredWeek = -1;
        private int currentMissionDeadline = -1;
        private bool missionCleared = false;

        public bool IsInMissionPeriod => currentMissionDeadline > 0 && GameDateManager.Instance.CurrentWeekIndex <= currentMissionDeadline;
        public bool HasActiveMission => currentMissionDeadline > 0 && !missionCleared;

        public int CurrentMissionDeadline => currentMissionDeadline;
        public int FailedCount => failedCount;

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

        private void Start()
        {
            GameDateManager.Instance.OnWeekAdvanced += HandleWeekAdvanced;
        }

        private void OnDestroy()
        {
            if (GameDateManager.Instance != null)
                GameDateManager.Instance.OnWeekAdvanced -= HandleWeekAdvanced;
        }

        private void HandleWeekAdvanced(int week)
        {
            // 미션 시작 주차에 도달했을 때
            if (missionTriggerWeeks.Contains(week))
            {
                TriggerMission(week);
            }

            // 미션 기간이 종료되었는데 실패한 경우
            if (currentMissionDeadline > 0 && week > currentMissionDeadline && !missionCleared)
            {
                HandleMissionFailure();
            }
        }

        private void TriggerMission(int triggerWeek)
        {            
            currentMissionDeadline = triggerWeek + 8; 

            lastTriggeredWeek = triggerWeek;
            missionCleared = false;

            Debug.Log($"[GateMissionManager] 미션 시작! 기한: {currentMissionDeadline}주차까지 클리어 필요");
        }

        public void CompleteMission()
        {
            if (IsInMissionPeriod)
            {
                missionCleared = true;
                failedCount = 0;
                Debug.Log("[GateMissionManager] 미션 클리어!");
            }
        }

        private void HandleMissionFailure()
        {
            failedCount++;
            Debug.LogWarning($"[GateMissionManager] 미션 실패! 누적 실패: {failedCount}");

            if (failedCount >= 2)
            {
                Debug.LogError("[GateMissionManager] 2회 연속 실패 → 제적 (게임오버)!");
                // TODO: 게임오버 처리
            }
            else
            {
                // TODO: 경고 및 명성 감소 등 패널티 처리
                Debug.Log("[GateMissionManager] 경고 상태! 다음 미션은 반드시 성공해야 함");
            }

            // 미션 상태 초기화
            missionCleared = false;
            currentMissionDeadline = -1;
        }
    }
}