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

        [Header("보유 중인 덱 프리셋")]
        [SerializeField] private List<DeckSaveData> allDeckPresets = new(); // <- 변경됨

        [Header("현재 선택된 전투 덱")]
        private string activeDeckId = null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (GameContext.Instance != null)
            {
                GameContext.Instance.inventory = this;
                DontDestroyOnLoad(gameObject);
                Debug.Log("[PlayerInventory] GameContext에 연결 완료");
            }


        }

        private void Start()
        {
            Debug.Log("[PlayerInventory] 전역 인벤토리 초기화 완료");
        }

        // 장비 관련
        public EquipmentData GetEquipped(EquipType type) =>
            equippedItems.TryGetValue(type, out var item) ? item : null;

        public void EquipItem(EquipType type, EquipmentData data) =>
            equippedItems[type] = data;

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

        // 덱 프리셋 관련
        public List<DeckSaveData> GetAllDecks() => allDeckPresets;
        public void AddDeck(DeckSaveData deck) => allDeckPresets.Add(deck);

        public List<DeckSaveData> GetAllDeckPresets()
        {
            return allDeckPresets;
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
                    count = 1,
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

            // 덱 저장
            foreach (var deck in allDeckPresets)
            {
                saveData.decks.Add(new DeckSaveData
                {
                    deckName = deck.deckName,
                    cardIds = new List<string>(deck.cardIds)
                });
            }

            return saveData;
        }

        public void ApplyDeckPresets(List<DeckSaveData> presets)
        {
            allDeckPresets.Clear();

            foreach (var preset in presets)
            {
                allDeckPresets.Add(new DeckSaveData
                {
                    deckName = preset.deckName,
                    cardIds = new List<string>(preset.cardIds)
                });
            }
        }

        public void LoadFromData(PlayerInventorySaveData data)
        {
            allCards.Clear();
            allEquipments.Clear();
            allArtifacts.Clear();
            equippedItems.Clear();
            allDeckPresets.Clear();

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

            // 덱 복원
            foreach (var deckSave in data.decks)
            {
                allDeckPresets.Add(new DeckSaveData
                {
                    deckName = deckSave.deckName,
                    cardIds = new List<string>(deckSave.cardIds)
                });
            }           
        Debug.Log("[PlayerInventory] 인벤토리 로드 완료");
        }

        public void UpdateDeck(DeckSaveData newData)
        {
            var existing = allDeckPresets.Find(d => d.slotIndex == newData.slotIndex);
            if (existing != null)
            {
                // 덮어쓰기
                existing.deckName = newData.deckName;
                existing.cardIds = new List<string>(newData.cardIds);
                existing.isFavorite = newData.isFavorite;
                existing.isSelected = newData.isSelected;
                existing.note = newData.note;
            }
            else
            {
                // 새로 추가
                allDeckPresets.Add(new DeckSaveData
                {
                    deckName = newData.deckName,
                    cardIds = new List<string>(newData.cardIds),
                    isFavorite = newData.isFavorite,
                    isSelected = newData.isSelected,
                    slotIndex = newData.slotIndex,
                    note = newData.note
                });
            }
        }
        // 현재 선택된 덱 ID 설정
        public void SetActiveDeck(DeckSaveData deck)
        {
            if (deck == null)
            {
                Debug.LogWarning("[PlayerInventory] null 덱을 선택하려고 했습니다.");
                activeDeckId = null;
                return;
            }

            activeDeckId = deck.deckId;  // 이 필드는 DeckSaveData에 존재한다고 가정
            Debug.Log($"[PlayerInventory] 선택된 덱 ID 설정됨: {activeDeckId}");
        }

        // 현재 선택된 덱 가져오기
        public DeckSaveData GetActiveDeck()
        {
            if (string.IsNullOrEmpty(activeDeckId)) return null;
            return allDeckPresets.Find(d => d.deckId == activeDeckId);
        }

        public DeckSaveData GetDefaultDeck()
        {
            if (allDeckPresets.Count > 0)
                return allDeckPresets[0]; // 첫 번째 덱을 기본으로 간주
            else
                return null;
        }


    }
}