using UnityEngine;

namespace Inventory
{
    public enum ArtifactRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    [CreateAssetMenu(fileName = "NewArtifact", menuName = "Inventory/Artifact")]
    public class ArtifactData : ScriptableObject
    {
        [Header("�⺻ ����")]
        public string displayName;
        [TextArea(2, 5)]
        public string description;
        public Sprite icon;

        [Header("�Ӽ�")]
        public ArtifactRarity rarity;
        public bool hasPassiveEffect;
    }
}