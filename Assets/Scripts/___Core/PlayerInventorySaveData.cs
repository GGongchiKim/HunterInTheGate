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
        public string cardId;         // ScriptableObject �̸��̳� GUID
        public int count;             // �ߺ� ���� ���� (�ִ� 3)
        public int upgradeLevel;      // ��ȭ ���� (Lv1~3)
    }

    [Serializable]
    public class DeckPreset
    {
        public string presetName;
        public List<string> cardIds = new();  // ī�� ID (�ߺ� ��� ����)
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
