using UnityEngine;
using System.Collections.Generic;

namespace Inventory
{
    public class EquipmentDatabase : MonoBehaviour
    {
        public static EquipmentDatabase Instance { get; private set; }

        private List<EquipmentData> allEquipments = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            LoadEquipments();
        }

        private void LoadEquipments()
        {
            allEquipments.Clear();
            allEquipments.AddRange(Resources.LoadAll<EquipmentData>("Equipments"));
        }

        public List<EquipmentData> GetAllEquipments()
        {
            return new List<EquipmentData>(allEquipments);
        }

        public List<EquipmentData> GetEquipmentsByType(EquipType type)
        {
            return allEquipments.FindAll(e => e.equipType == type);
        }

        public List<EquipmentData> FilterByRarity(Rarity rarity)
        {
            return allEquipments.FindAll(e => e.rarity == rarity);
        }
    }
}