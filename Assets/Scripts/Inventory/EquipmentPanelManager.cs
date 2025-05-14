using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Inventory
{
    public class EquipmentPanelManager : MonoBehaviour
    {
        [Header("���� ��ư")]
        [SerializeField] private Button weaponSlotButton;
        [SerializeField] private Button armorSlotButton;
        [SerializeField] private Button toolSlotButton;

        [Header("��Ÿ ��ư")]
        [SerializeField] private Button clearFilterButton;

        [Header("��� ī�� UI ����")]
        [SerializeField] private Transform cardParent;

        private EquipType? currentSlot = null; // null�̸� ��ü ��� ǥ��
        private readonly List<GameObject> spawnedCards = new();

        private void Start()
        {
            weaponSlotButton.onClick.AddListener(() => OnSlotClicked(EquipType.Weapon));
            armorSlotButton.onClick.AddListener(() => OnSlotClicked(EquipType.Armor));
            toolSlotButton.onClick.AddListener(() => OnSlotClicked(EquipType.Tool));
            clearFilterButton.onClick.AddListener(ClearFilter);

            RefreshPanel(); // ���� �� ��ü ��� ǥ��
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

        // ���� �� ���� Ŭ�� �� ȣ�� �� ��ü ��� ǥ��
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
            Debug.Log($"���õ� ���: {selected.displayName}");
            // ���� ���� ���� ���� ����
        }
    }
}