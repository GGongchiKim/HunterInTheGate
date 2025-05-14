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

        [Header("장비 카드 UI 영역")]
        [SerializeField] private Transform cardParent;

        private EquipType currentSlot = EquipType.Weapon;
        private readonly List<GameObject> spawnedCards = new();

        private void Start()
        {
            weaponSlotButton.onClick.AddListener(() => OnSlotClicked(EquipType.Weapon));
            armorSlotButton.onClick.AddListener(() => OnSlotClicked(EquipType.Armor));
            toolSlotButton.onClick.AddListener(() => OnSlotClicked(EquipType.Tool));

            RefreshPanel();
        }

        public void RefreshPanel()
        {
            currentSlot = EquipType.Weapon;
            ShowEquipments(currentSlot);
        }

        private void OnSlotClicked(EquipType slot)
        {
            currentSlot = slot;
            ShowEquipments(currentSlot);
        }

        private void ShowEquipments(EquipType slot)
        {
            foreach (var card in spawnedCards)
                Destroy(card);
            spawnedCards.Clear();

            List<EquipmentData> filtered = PlayerInventory.Instance.GetEquipmentsByType(slot);

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
        }
    }
}