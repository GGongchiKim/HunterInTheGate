using UnityEngine;
using System.Collections.Generic;
using SaveSystem;

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
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Debug.Log("[PlayerInventory] 전역 인벤토리 초기화 완료");
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

        public List<EquipmentData> GetAllEquipments() => allEquipments;

        public List<EquipmentData> GetEquipmentsByType(EquipType type) =>
            allEquipments.FindAll(e => e.equipType == type);

        public void AddEquipment(EquipmentData equipment)
        {
            if (!allEquipments.Contains(equipment))
                allEquipments.Add(equipment);
        }

        // 아티팩트 관련
        public List<ArtifactData> GetAllArtifacts() => allArtifacts;

        public void AddArtifact(ArtifactData artifact)
        {
            if (!allArtifacts.Contains(artifact))
                allArtifacts.Add(artifact);
        }

        // 카드 관련
        public List<CardData> GetAllCards() => allCards;

        public void AddCard(CardData card)
        {
            if (card == null)
            {
                Debug.LogWarning("[PlayerInventory] null 카드 추가 시도됨");
                return;
            }

            if (!allCards.Contains(card))
            {
                allCards.Add(card);
                Debug.Log($"[PlayerInventory] 카드 획득: {card.cardName}");
            }
            else
            {
                Debug.Log($"[PlayerInventory] 이미 보유 중인 카드: {card.cardName}");
            }
        }

        // 세이브 데이터 변환
        public PlayerInventorySaveData GetSaveData()
        {
            var saveData = new PlayerInventorySaveData();

            foreach (var card in allCards)
            {
                saveData.cards.Add(new CardSaveData
                {
                    cardId = card.cardId,
                    count = 1, // 중복 관리 예정 시 수량 누적 로직 필요
                    upgradeLevel = card.level
                });
            }

            foreach (var equip in allEquipments)
            {
                saveData.equipments.Add(new EquipmentSaveData
                {
                    equipmentId = equip.id,
                    isEquipped = equippedItems.ContainsValue(equip),
                    slotType = (SaveSystem.EquipType)equip.equipType
                });
            }

            foreach (var artifact in allArtifacts)
            {
                saveData.artifacts.Add(new ArtifactSaveData
                {
                    artifactId = artifact.id
                });
            }

            return saveData;
        }

        public void LoadFromData(PlayerInventorySaveData data)
        {
            allCards.Clear();
            allEquipments.Clear();
            allArtifacts.Clear();
            equippedItems.Clear();

            foreach (var cardSave in data.cards)
            {
                var card = CardDatabase.Instance.GetCardById(cardSave.cardId);
                if (card != null)
                {
                    for (int i = 0; i < cardSave.count; i++)
                    {
                        allCards.Add(card);
                    }
                }
            }

            foreach (var equipSave in data.equipments)
            {
                var equip = EquipmentDatabase.Instance.GetEquipmentById(equipSave.equipmentId);
                if (equip != null)
                {
                    allEquipments.Add(equip);
                    if (equipSave.isEquipped)
                        equippedItems[(EquipType)equipSave.slotType] = equip;
                }
            }

            foreach (var artifactSave in data.artifacts)
            {
                var artifact = ArtifactDatabase.Instance.GetArtifactById(artifactSave.artifactId);
                if (artifact != null)
                {
                    allArtifacts.Add(artifact);
                }
            }

            Debug.Log("[PlayerInventory] 인벤토리 로드 완료");
        }
    }
}
