using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcademyContext : MonoBehaviour
{
    public static AcademyContext Instance { get; private set; }

    [Header("능력치")]
    public float strength = 10;
    public float agility = 10;
    public float insight = 10;
    public float magic = 10;
    public float willPower = 10;  // 새로 추가된 능력치
    public float wit = 10;        // 새로 추가된 능력치
    public float charisma = 10;   // 새로 추가된 능력치
    public float luck = 10;       // 새로 추가된 능력치

    [Header("주간 스케줄")]
    public int currentWeek = 1;
    public int currentDay = 1;

    private void Awake()
    {
        // 싱글톤 패턴을 사용하여 AcademyContext 인스턴스를 관리
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // 주간 활동을 진행하는 메서드
    public void AdvanceWeek()
    {
        currentDay++;
        if (currentDay > 7) // 7일이 지나면 새로운 주로 넘어감
        {
            currentDay = 1;
            currentWeek++;
        }

        // 주간 활동 후 능력치 변화 적용
        ApplyWeeklyActivities();
    }

    // 주간 활동 후 능력치 변화
    public void ApplyWeeklyActivities()
    {
        // 예시로 훈련 후 능력치 증가
        strength += 2;  // 훈련 후 힘 증가
        agility += 1;   // 훈련 후 민첩 증가
        willPower += 1; // 훈련 후 의지력 증가
        wit += 1;       // 훈련 후 기민함 증가
        charisma += 1;  // 훈련 후 카리스마 증가
        luck += 1;      // 훈련 후 운 증가

        // 능력치 변화가 Player 클래스에 반영되도록 업데이트
        Player player = GameContext.Instance.player;
        player.strength = (int)strength;
        player.agility = (int)agility;
        player.insight = (int)insight;
        player.magic = (int)magic;
        player.willPower = (int)willPower;
        player.wit = (int)wit;
        player.charisma = (int)charisma;
        player.luck = (int)luck;

        // UI 업데이트 (능력치가 변화한 후 UI를 갱신)
        ArcademyUIManager.Instance.UpdateStatsUI();
    }

    // 능력치 변경 메서드
    public void ChangeStat(string statName, float value)
    {
        switch (statName)
        {
            case "Strength":
                strength += value;
                break;
            case "Agility":
                agility += value;
                break;
            case "Insight":
                insight += value;
                break;
            case "Magic":
                magic += value;
                break;
            case "WillPower":
                willPower += value; // 의지력 추가
                break;
            case "Wit":
                wit += value;       // 기민함 추가
                break;
            case "Charisma":
                charisma += value;  // 카리스마 추가
                break;
            case "Luck":
                luck += value;      // 운 추가
                break;
        }

        // Player 클래스에 능력치 반영
        Player player = GameContext.Instance.player;
        player.strength = (int)strength;
        player.agility = (int)agility;
        player.insight = (int)insight;
        player.magic = (int)magic;
        player.willPower = (int)willPower;
        player.wit = (int)wit;
        player.charisma = (int)charisma;
        player.luck = (int)luck;

        // UI 업데이트
        ArcademyUIManager.Instance.UpdateStatsUI();
    }

    // 훈련을 통한 능력치 상승 예시
    public void ApplyTrainingResults()
    {
        strength += 3;  // 훈련 후 strength 증가
        agility += 2;   // 훈련 후 agility 증가
        willPower += 2; // 훈련 후 의지력 증가
        wit += 1;       // 훈련 후 기민함 증가
        charisma += 1;  // 훈련 후 카리스마 증가
        luck += 1;      // 훈련 후 운 증가

        // Player 클래스에 능력치 반영
        Player player = GameContext.Instance.player;
        player.strength = (int)strength;
        player.agility = (int)agility;
        player.insight = (int)insight;
        player.magic = (int)magic;
        player.willPower = (int)willPower;
        player.wit = (int)wit;
        player.charisma = (int)charisma;
        player.luck = (int)luck;

        // UI 업데이트
        ArcademyUIManager.Instance.UpdateStatsUI();
    }
}