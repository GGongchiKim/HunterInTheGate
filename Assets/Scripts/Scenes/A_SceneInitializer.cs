using UnityEngine;

public class A_SceneInitializer : MonoBehaviour
{
    public static A_SceneInitializer Instance { get; private set; }

    [Header("HUD 패널")]
    public GameObject scg;
    public GameObject informationPanel;
    public GameObject mainMenu;

    [Header("MainMenu 관련")]
    public GameObject saveLoadPanel;//saveLoadPanel 컨트롤러
    public GameObject snLPanel; // saveLoad 하위 UI패널
    public GameObject statusPanel;

    [Header("스케줄 관련")]
    public GameObject schedulePanel;          // 스케줄 시스템 전체 루트
    public GameObject scheduleSelectPanel;    // 활동 종류를 선택하는 메뉴
    public GameObject scheduleMenuPanel;      // 활동 종류를 보관하는 슬롯
    public GameObject schedulePanelGroup;     // 활동별 상세 패널 묶음 (Class, Outing 등)

    [Header("클래스 패널 그룹")]
    public GameObject classPanel;               // ClassPanel 전체
    public GameObject classSelectSlot;          // 수업 목록 (클래스 카드)
    public GameObject classInfoPanel;           // 수업 상세 정보
    public GameObject classAnimationPanel;      // 수업 애니메이션
    public GameObject classAnimationController; // 수업 애니메이션 관리용 클래스
    public GameObject classRewardPanel;         // 수업 보상

    [Header("기타 활동 패널")]
    public GameObject gateDivePanel;
    public GameObject restPanel;
    public GameObject outingPanel;

    [Header("참조 연결")]
    public SchedulePanelManager scheduleManager;   // 스케줄 매니저 연결 필요

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        InitializeScene();

        if (scheduleManager != null)
            scheduleManager.InitScheduleMenuButtons(); // 버튼 초기화는 Start 이후에 안전하게 호출

        AudioManager.Instance?.PlayBGM("AcademyBGM",true);

    }

    public void InitializeScene()
    {
        GameStateManager.Instance.SetPhase(GamePhase.Management);
        InitializeHUD();
        InitializeSchedulePanels();
        InitializeClassPanels();
        InitializeOtherActivityPanels();
        InitializeMainMenuPanel();
    }

    private void InitializeHUD()
    {
        SetActiveGroup(true, scg, informationPanel, mainMenu);
    }

    private void InitializeMainMenuPanel() 
    {
        SetActiveGroup(false,statusPanel,snLPanel);
        
        saveLoadPanel.SetActive(true);
        
    }


    private void InitializeSchedulePanels()
    {
        schedulePanel.SetActive(true);
        scheduleSelectPanel.SetActive(true);
        schedulePanelGroup.SetActive(false);
    }

    private void InitializeClassPanels()
    {
        SetActiveGroup(false, classPanel, classSelectSlot, classInfoPanel, classRewardPanel, classAnimationPanel);

        
        if (classAnimationController != null)
            classAnimationController.SetActive(true);
    }

    private void InitializeOtherActivityPanels()
    {
        SetActiveGroup(false, gateDivePanel, restPanel, outingPanel);
    }

    private void SetActiveGroup(bool state, params GameObject[] targets)
    {
        foreach (var obj in targets)
        {
            if (obj != null) obj.SetActive(state);
        }
    }

    /// <summary>
    /// 클래스 메뉴 직접 열기 (버튼에 연결됨)
    /// </summary>
    public void OpenClassPanelDirectly()
    {
        if (scheduleManager != null)
            scheduleManager.OnClickScheduleType(ScheduleType.Class);

        classSelectSlot.SetActive(true);
        classInfoPanel.SetActive(true);
        scheduleMenuPanel.SetActive(false);
    }

  
}