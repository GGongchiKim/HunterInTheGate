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
        public string cardId;         // ScriptableObject �̸��̳� GUID
        public int count;             // �ߺ� ���� ���� (�ִ� 3)
        public int upgradeLevel;      // ��ȭ ���� (Lv1~3)
    }

    [Serializable]
    public class DeckSaveData
    {
        public string deckName;
        public List<string> cardIds = new();  // ī�� ID (�ߺ� ��� ����)

        public bool isSelected = false;       // ���� Ž�迡 ���õ� �� ����
        public bool isFavorite = false;       // ���ã�� ����
        public int slotIndex = -1;            // UI���� ���� ������ ���� ��ȣ
        public string note = "";              // ������ ���� �޸�, ���� �� (���û���)

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
