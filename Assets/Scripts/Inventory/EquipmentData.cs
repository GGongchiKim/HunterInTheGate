using UnityEngine;

namespace Inventory
{
    public enum EquipType
    {
        Weapon,
        Armor,
        Tool
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    [CreateAssetMenu(fileName = "NewEquipData", menuName = "Inventory/Equipment")]
    public class EquipmentData : ScriptableObject
    {
        [Header("�⺻ ����")]
        public string displayName;
        [TextArea]
        public string description;
        public Sprite icon;
        public EquipType equipType;
        public Rarity rarity;

        [Header("�⺻ �ɷ�ġ")]
        public int attack;
        public int defense;
        public int utility; // Ž�� ���� ��� Ȱ��� �� �ִ� ���� ��ġ

        [Header("Ư�� ȿ�� (���� ����)")]
        public string passiveEffectText;
        public bool hasPassiveEffect;
    }
}