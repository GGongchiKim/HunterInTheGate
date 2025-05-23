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
        [Header("기본 정보")]
        public string displayName;
        [TextArea]
        public string description;
        public Sprite icon;
        public EquipType equipType;
        public Rarity rarity;

        [Header("기본 능력치")]
        public int attack;
        public int defense;
        public int utility; // 탐험 도구 등에서 활용될 수 있는 보조 수치

        [Header("특수 효과 (선택 사항)")]
        public string passiveEffectText;
        public bool hasPassiveEffect;
    }
}