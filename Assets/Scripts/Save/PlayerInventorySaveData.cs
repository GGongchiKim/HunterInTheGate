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
        public string deckName;
        public List<string> cardIds = new();  // 카드 ID (중복 허용 가능)

        public bool isSelected = false;       // 현재 탐험에 선택된 덱 여부
        public bool isFavorite = false;       // 즐겨찾기 여부
        public int slotIndex = -1;            // UI에서 정렬 유지용 슬롯 번호
        public string note = "";              // 유저가 적는 메모, 설명 등 (선택사항)

        public DeckSaveData() { }

        public DeckSaveData(string name, List<string> cardIds, bool isSelected = false, int slotIndex = -1)
        {
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
