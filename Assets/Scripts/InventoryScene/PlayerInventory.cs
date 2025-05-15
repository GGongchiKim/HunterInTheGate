using UnityEngine;
using System.Collections.Generic;

namespace Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        public static PlayerInventory Instance { get; private set; }

        [Header("보유 중인 장비")]
        [SerializeField] private List<EquipmentData> allEquipments = new();

        [Header("현재 장착 중인 장비")]
        private Dictionary<EquipType, EquipmentData> equippedItems = new();

        [Header("획득한 아티팩트")]
        [SerializeField] private List<ArtifactData> allArtifacts = new();

        [Header("보유 중인 카드")]
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

        // 장비 관련
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

        // 아티팩트 관련
        public List<ArtifactData> GetAllArtifacts()
        {
            return allArtifacts;
        }

        public void AddArtifact(ArtifactData artifact)
        {
            if (!allArtifacts.Contains(artifact))
                allArtifacts.Add(artifact);
        }

        // 카드 관련
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