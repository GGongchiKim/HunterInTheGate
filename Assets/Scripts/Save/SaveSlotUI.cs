using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using SaveSystem;

public class SaveSlotUI : MonoBehaviour
{
    [Header("UI 요소들")]
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI gameDateText;
    [SerializeField] private TextMeshProUGUI hunterRankText;
    [SerializeField] private TextMeshProUGUI lastSaveTimeText;
    [SerializeField] private Image highlightImage;
    [SerializeField] private Button selectButton;

    private int slotIndex;
    private SaveLoadPanelManager panelManager;

    public void Initialize(int index, SaveLoadPanelManager manager)
    {
        slotIndex = index;
        panelManager = manager;

        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnClick);
        }

        UpdateSlotStatus();
        SetHighlight(false);
    }

    public void UpdateSlotStatus()
    {
        string path = SaveManager.Instance.GetSlotPathPublic(slotIndex);

        if (!File.Exists(path))
        {
            playerNameText.text = "저장 없음";
            gameDateText.text = "";
            hunterRankText.text = "";
            lastSaveTimeText.text = "";
            return;
        }

        var data = SaveManager.Instance.GetSaveData(slotIndex);

        if (data != null && data.progress != null)
        {
            playerNameText.text = data.progress.playerName;
            gameDateText.text = $"{data.progress.inGameYear}년 {data.progress.inGameWeek}주차";
            hunterRankText.text = data.progress.hunterRank.ToString();
            lastSaveTimeText.text = File.GetLastWriteTime(path).ToString("yyyy-MM-dd HH:mm");
        }
        else
        {
            playerNameText.text = "불러오기 실패";
            gameDateText.text = "";
            hunterRankText.text = "";
            lastSaveTimeText.text = "";
        }
    }

    private void OnClick()
    {
        panelManager?.SelectSlot(slotIndex);
    }

    public void SetHighlight(bool isOn)
    {
        if (highlightImage != null)
            highlightImage.enabled = isOn;
    }
}