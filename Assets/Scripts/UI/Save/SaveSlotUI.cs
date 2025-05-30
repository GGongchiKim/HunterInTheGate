using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using SaveSystem;

public class SaveSlotUI : MonoBehaviour
{
    [Header("UI ��ҵ�")]
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

        // ������ ������ ������ ��ȿ�� ���δ� �߿�ġ ����. �ð��� ǥ��.
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