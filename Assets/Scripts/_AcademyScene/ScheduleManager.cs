using System;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleManager : MonoBehaviour
{
    public static ScheduleManager Instance { get; private set; }

    [Header("UI 참조")]
    public GameObject scheduleMenuPanel;       // ScheduleMenuSlot 패널
    public GameObject schedulePanelGroup;      // SchedulePanelGroup 전체

    [Header("스케쥴 메뉴 버튼")]
    public Button classButton;
    public Button outingButton;
    public Button gateDiveButton;
    public Button restButton;
    public Button closeButton;

    [Header("패널들")]
    public GameObject classPanel;
    public GameObject outingPanel;
    public GameObject restPanel;
    public GameObject gateDivePanel;

    [Header("날짜 처리")]
    public Text dateText;
    [SerializeField] private int currentDay = 1;
    [SerializeField] private int currentWeek = 1;
    [SerializeField] private int currentMonth = 1;

    public Action OnScheduleEnd; // 스케쥴 종료 후 콜백 이벤트

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitScheduleMenuButtons();
        UpdateDateUI();
    }

    private void InitScheduleMenuButtons()
    {
        classButton.onClick.AddListener(() => OnClickScheduleType(ScheduleType.Class));
        outingButton.onClick.AddListener(() => OnClickScheduleType(ScheduleType.Outing));
        gateDiveButton.onClick.AddListener(() => OnClickScheduleType(ScheduleType.GateDive));
        restButton.onClick.AddListener(() => OnClickScheduleType(ScheduleType.Rest));
        closeButton.onClick.AddListener(CloseScheduleMenu);
    }

    public void OpenScheduleMenu()
    {
        scheduleMenuPanel.SetActive(true);
    }

    public void CloseScheduleMenu()
    {
        scheduleMenuPanel.SetActive(false);
        schedulePanelGroup.SetActive(false);
    }

    public void OnClickScheduleType(ScheduleType type)
    {
        schedulePanelGroup.SetActive(true);

        classPanel.SetActive(false);
        outingPanel.SetActive(false);
        gateDivePanel.SetActive(false);
        restPanel.SetActive(false);

        switch (type)
        {
            case ScheduleType.Class:
                classPanel.SetActive(true);
                break;
            case ScheduleType.Outing:
                outingPanel.SetActive(true);
                break;
            case ScheduleType.GateDive:
                gateDivePanel.SetActive(true);
                break;
            case ScheduleType.Rest:
                restPanel.SetActive(true);
                break;
        }
    }

    public void AdvanceDay()
    {
        currentDay++;
        if (currentDay > 7)
        {
            currentDay = 1;
            currentWeek++;
            if (currentWeek > 4)
            {
                currentWeek = 1;
                currentMonth++;
            }
        }
        UpdateDateUI();
        OnScheduleEnd?.Invoke();
    }

    private void UpdateDateUI()
    {
        dateText.text = $"Month {currentMonth}, Week {currentWeek}, Day {currentDay}";
    }

    public int GetCurrentDay() => currentDay;
    public int GetCurrentWeek() => currentWeek;
    public int GetCurrentMonth() => currentMonth;
    public string GetFormattedDate() => $"Month {currentMonth}, Week {currentWeek}, Day {currentDay}";
}

public enum ScheduleType
{
    Class,
    Outing,
    GateDive,
    Rest
}