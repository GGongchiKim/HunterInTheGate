using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using SaveSystem;

public class SaveLoadPanelManager : MonoBehaviour
{
    [Header("세이브 슬롯 UI (Index 0~2)")]
    [SerializeField] private List<SaveSlotUI> saveSlots;

    [Header("Save/Load 버튼")]
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
        Debug.Log($"[SaveLoadPanelManager] 슬롯 {index + 1} 선택됨");

        for (int i = 0; i < saveSlots.Count; i++)
        {
            saveSlots[i].SetHighlight(i == index);
        }
    }

    private void OnClickSave()
    {
        if (selectedSlotIndex < 0)
        {
            Debug.LogWarning("[SaveLoadPanelManager] 슬롯을 선택하세요.");
            return;
        }

        SaveManager.Instance.SaveGame(selectedSlotIndex);
        Debug.Log($"[SaveLoadPanelManager] 슬롯 {selectedSlotIndex + 1}에 저장 완료");

        UpdateAllSlots();
    }

    private void OnClickLoad()
    {
        if (selectedSlotIndex < 0)
        {
            Debug.LogWarning("[SaveLoadPanelManager] 슬롯을 선택하세요.");
            return;
        }

        SaveManager.Instance.LoadGame(selectedSlotIndex);
        Debug.Log($"[SaveLoadPanelManager] 슬롯 {selectedSlotIndex + 1} 불러오기 시도");
    }

    public void UpdateAllSlots()
    {
        foreach (var slot in saveSlots)
        {
            slot.UpdateSlotStatus();
        }
    }
}