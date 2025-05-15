using UnityEngine;
using System.Collections.Generic;

namespace Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        public static PlayerInventory Instance { get; private set; }

        [Header("���� ���� ���")]
        [SerializeField] private List<EquipmentData> allEquipments = new();

        [Header("���� ���� ���� ���")]
        private Dictionary<EquipType, EquipmentData> equippedItems = new();

        [Header("ȹ���� ��Ƽ��Ʈ")]
        [SerializeField] private List<ArtifactData> allArtifacts = new();

        [Header("���� ���� ī��")]
        [SerializeField] private List<CardData> allCards = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // ��� ����
        public EquipmentData GetEquipped(EquipType type)
        {
            return equippedItems.TryGetValue(type, out var item) ? item : null;
        }

        public void EquipItem(EquipType type, EquipmentData data)
        {
            equippedItems[type] = data;
        }

        public List<EquipmentData> GetAllEquipments()
        {
            return allEquipments;
        }

        public List<EquipmentData> GetEquipmentsByType(EquipType type)
        {
            return allEquipments.FindAll(e => e.equipType == type);
        }

        public void AddEquipment(EquipmentData equipment)
        {
            if (!allEquipments.Contains(equipment))
                allEquipments.Add(equipment);
        }

        // ��Ƽ��Ʈ ����
        public List<ArtifactData> GetAllArtifacts()
        {
            return allArtifacts;
        }

        public void AddArtifact(ArtifactData artifact)
        {
            if (!allArtifacts.Contains(artifact))
                allArtifacts.Add(artifact);
        }

        // ī�� ����
        public List<CardData> GetAllCards()
        {
            return allCards;
        }

        public void AddCard(CardData card)
        {
            if (!allCards.Contains(card))
                allCards.Add(card);
        }
    }
}