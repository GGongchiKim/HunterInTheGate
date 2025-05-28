using System;
using System.Collections.Generic;

namespace SaveSystem
{
    [Serializable]
    public class PlayerInventorySaveData
    {
        public List<CardSaveData> cards = new();
        public List<DeckPreset> decks = new();
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
    public class DeckPreset
    {
        public string presetName;
        public List<string> cardIds = new();  // 카드 ID (중복 허용 가능)
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
