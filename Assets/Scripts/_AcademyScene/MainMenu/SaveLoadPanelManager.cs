using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using SaveSystem;

public class SaveLoadPanelManager : MonoBehaviour
{
    [Header("���̺� ���� UI (Index 0~2)")]
    [SerializeField] private List<SaveSlotUI> saveSlots;

    [Header("Save/Load ��ư")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;

    private int selectedSlotIndex = -1;

    private void Awake()
    {
        for (int i = 0; i < saveSlots.Count; i++)
        {
            saveSlots[i].Initialize(i, this);
        }

        if (saveButton != null)
            saveButton.onClick.AddListener(OnClickSave);

        if (loadButton != null)
            loadButton.onClick.AddListener(OnClickLoad);

        UpdateAllSlots();
    }

    public void SelectSlot(int index)
    {
        selectedSlotIndex = index;
        Debug.Log($"[SaveLoadPanelManager] ���� {index + 1} ���õ�");

        for (int i = 0; i < saveSlots.Count; i++)
        {
            saveSlots[i].SetHighlight(i == index);
        }
    }

    private void OnClickSave()
    {
        if (selectedSlotIndex < 0)
        {
            Debug.LogWarning("[SaveLoadPanelManager] ������ �����ϼ���.");
            return;
        }

        SaveManager.Instance.SaveGame(selectedSlotIndex);
        Debug.Log($"[SaveLoadPanelManager] ���� {selectedSlotIndex + 1}�� ���� �Ϸ�");

        UpdateAllSlots();
    }

    private void OnClickLoad()
    {
        if (selectedSlotIndex < 0)
        {
            Debug.LogWarning("[SaveLoadPanelManager] ������ �����ϼ���.");
            return;
        }

        SaveManager.Instance.LoadGame(selectedSlotIndex);
        Debug.Log($"[SaveLoadPanelManager] ���� {selectedSlotIndex + 1} �ҷ����� �õ�");
    }

    public void UpdateAllSlots()
    {
        foreach (var slot in saveSlots)
        {
            slot.UpdateSlotStatus();
        }
    }
}