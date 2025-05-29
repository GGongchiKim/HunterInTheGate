using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using SaveSystem;

public class SaveSlotUI : MonoBehaviour
{
    [Header("UI 요소들")]
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
            lastSaveTimeText.text = "No Data";
            return;
        }

        // 파일은 있으나 데이터 유효성 여부는 중요치 않음. 시간만 표시.
        lastSaveTimeText.text = File.GetLastWriteTime(path).ToString("yyyy-MM-dd HH:mm");
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