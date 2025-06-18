using UnityEngine;
using UnityEngine.UI;

public class A_HUDManager : MonoBehaviour
{
    public static A_HUDManager Instance { get; private set; }

    [Header("패널 참조")]
    public GameObject statusPanel;
    public GameObject journalPanel;
    public GameObject settingPanel;
    public GameObject saveLoadPanel;
    public GameObject scheduleMenuPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 각 버튼에 연결할 전용 함수들
    public void OnClickStatus() { statusPanel.SetActive(true); }
    public void OnClickStatusClose() { statusPanel.SetActive(false); }

    public void OnClickJournal() { journalPanel.SetActive(true); }
    public void OnClickJournalClose() { journalPanel.SetActive(false); }

    public void OnClickSetting() { settingPanel.SetActive(true); }
    public void OnClickSettingClose() { settingPanel.SetActive(false); }

    public void OnClickSaveLoad() { saveLoadPanel.SetActive(true); }
    public void OnClickSaveLoadClose() { saveLoadPanel.SetActive(false); }

    public void OnClickSchedule() { scheduleMenuPanel.SetActive(true); }

    /// <summary>
    /// 인벤토리 버튼 클릭 시 씬 전환 (페이드 포함)
    /// </summary>
    public void OnClickInventory()
    {
        SceneTransitionManager.Instance.LoadSceneWithFade("InventoryScene", GamePhase.Management);
    }
}