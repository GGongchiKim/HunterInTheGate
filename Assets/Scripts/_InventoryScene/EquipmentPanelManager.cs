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

        [Header("장비 슬롯 표시 UI")]
        [SerializeField] private EquipmentSlotDisplay weaponSlotDisplay;
        [SerializeField] private EquipmentSlotDisplay armorSlotDisplay;
        [SerializeField] private EquipmentSlotDisplay toolSlotDisplay;

        private EquipType? currentSlot = null; // null이면 전체 장비 표시
        private readonly List<GameObject> spawnedCards = new();

        private void Start()
        {
            weaponSlotButton.onClick.AddListener(() => OnSlotClicked(EquipType.Weapon));
            armorSlotButton.onClick.AddListener(() => OnSlotClicked(EquipType.Armor));
            toolSlotButton.onClick.AddListener(() => OnSlotClicked(EquipType.Tool));
            clearFilterButton.onClick.AddListener(ClearFilter);

            RefreshPanel();
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

            EquipToSlot(selected, selected.equipType);

            currentSlot = null;
            ShowEquipments(null);
        }

        private void EquipToSlot(EquipmentData data, EquipType slotType)
        {
            switch (slotType)
            {
                case EquipType.Weapon:
                    weaponSlotDisplay.SetEquipment(data);
                    break;
                case EquipType.Armor:
                    armorSlotDisplay.SetEquipment(data);
                    break;
                case EquipType.Tool:
                    toolSlotDisplay.SetEquipment(data);
                    break;
            }

            PlayerInventory.Instance.EquipItem(slotType, data);
        }
    }
}
