using System;
using System.Collections.Generic;

namespace SaveSystem
{
    [Serializable]
    public class PlayerInventorySaveData
    {
        public List<CardSaveData> cards = new();
        public List<DeckSaveData> decks = new();
        public List<EquipmentSaveData> equipments = new();
        public List<ArtifactSaveData> artifacts = new();
    }

    [Serializable]
    public class CardSaveData
    {
        public string cardId;         // ScriptableObject 이름이나 GUID
        public int count;             // 중복 보유 수량 (최대 3)
        public int upgradeLevel;      // 강화 정도 (Lv1~3)
    }

    [Serializable]
    public class DeckSaveData
    {
        public string deckId;                   // 고유 식별자 (자동 생성됨)
        public string deckName;
        public List<string> cardIds = new();    // 카드 ID 목록

        public bool isSelected = false;         // 현재 탐험에 선택된 덱 여부
        public bool isFavorite = false;         // 즐겨찾기 여부
        public int slotIndex = -1;              // 정렬용 슬롯 번호
        public string note = "";                // 유저 메모

        public DeckSaveData()
        {
            deckId = Guid.NewGuid().ToString(); // 기본 생성자에도 UUID 부여
        }

        public DeckSaveData(string name, List<string> cardIds, bool isSelected = false, int slotIndex = -1)
        {
            this.deckId = Guid.NewGuid().ToString(); // 고유 ID 자동 부여
            this.deckName = name;
            this.cardIds = new List<string>(cardIds);
            this.isSelected = isSelected;
            this.slotIndex = slotIndex;
        }
    }

    [Serializable]
    public class EquipmentSaveData
    {
        public string equipmentId;
        public bool isEquipped;
        public EquipType slotType;
    }

    [Serializable]
    public class ArtifactSaveData
    {
        public string artifactId;
    }

    public enum EquipType
    {
        Weapon,
        Armor,
        Tool
    }
}
