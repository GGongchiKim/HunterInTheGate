using UnityEngine;
using UnityEngine.UI;

public class A_HUDManager : MonoBehaviour
{
    public static A_HUDManager Instance { get; private set; }

    [Header("�г� ����")]
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

    // �� ��ư�� ������ ���� �Լ���
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
    /// �κ��丮 ��ư Ŭ�� �� �� ��ȯ (���̵� ����)
    /// </summary>
    public void OnClickInventory()
    {
        SceneTransitionManager.Instance.LoadSceneWithFade("InventoryScene", GamePhase.Management);
    }
}