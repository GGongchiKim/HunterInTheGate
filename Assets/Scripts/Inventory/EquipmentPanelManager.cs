using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Inventory
{
    public class EquipmentPanelManager : MonoBehaviour
    {
        [Header("슬롯 버튼")]
        [SerializeField] private Button weaponSlotButton;
        [SerializeField] private Button armorSlotButton;
        [SerializeField] private Button toolSlotButton;

        [Header("기타 버튼")]
        [SerializeField] private Button clearFilterButton;

        [Header("장비 카드 UI 영역")]
        [SerializeField] private Transform cardParent;

        private EquipType? currentSlot = null; // null이면 전체 장비 표시
        private readonly List<GameObject> spawnedCards = new();

        private void Start()
        {
            weaponSlotButton.onClick.AddListener(() => OnSlotClicked(EquipType.Weapon));
            armorSlotButton.onClick.AddListener(() => OnSlotClicked(EquipType.Armor));
            toolSlotButton.onClick.AddListener(() => OnSlotClicked(EquipType.Tool));
            clearFilterButton.onClick.AddListener(ClearFilter);

            RefreshPanel(); // 시작 시 전체 장비 표시
        }

        public void RefreshPanel()
        {
            currentSlot = null;
            ShowEquipments(null);
        }

        private void OnSlotClicked(EquipType slot)
        {
            currentSlot = slot;
            ShowEquipments(slot);
        }

        // 슬롯 외 영역 클릭 시 호출 → 전체 장비 표시
        public void ClearFilter()
        {
            if (currentSlot != null)
            {
                currentSlot = null;
                ShowEquipments(null);
            }
        }

        private void ShowEquipments(EquipType? slot)
        {
            foreach (var card in spawnedCards)
                Destroy(card);
            spawnedCards.Clear();

            List<EquipmentData> filtered = (slot == null)
                ? PlayerInventory.Instance.GetAllEquipments()
                : PlayerInventory.Instance.GetEquipmentsByType(slot.Value);

            foreach (var data in filtered)
            {
                GameObject card = InventoryUIManager.Instance.CreateCard(
                    data: data,
                    type: InventoryCardType.Equipment,
                    parent: cardParent,
                    onClick: () => OnEquipmentCardClicked(data)
                );

                if (card != null)
                    spawnedCards.Add(card);
            }
        }

        private void OnEquipmentCardClicked(EquipmentData selected)
        {
            Debug.Log($"선택된 장비: {selected.displayName}");
            // 추후 장착 로직 구현 예정
        }
    }
}