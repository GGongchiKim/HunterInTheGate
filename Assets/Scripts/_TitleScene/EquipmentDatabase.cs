using UnityEngine;
using System.Collections.Generic;

namespace Inventory
{
    public class EquipmentDatabase : MonoBehaviour
    {
        public static EquipmentDatabase Instance { get; private set; }

        private List<EquipmentData> allEquipments = new();
        private Dictionary<string, EquipmentData> equipmentById = new();

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
            equipmentById.Clear();

            var loadedEquipments = Resources.LoadAll<EquipmentData>("Equipments");
            foreach (var equip in loadedEquipments)
            {
                if (!equipmentById.ContainsKey(equip.id))
                {
                    equipmentById.Add(equip.id, equip);
                    allEquipments.Add(equip);
                }
                else
                {
                    Debug.LogWarning($"[EquipmentDatabase] 중복된 장비 ID 발견: {equip.id}");
                }
            }
        }

        public List<EquipmentData> GetAllEquipments() => new(allEquipments);

        public List<EquipmentData> GetEquipmentsByType(EquipType type) =>
            allEquipments.FindAll(e => e.equipType == type);

        public List<EquipmentData> FilterByRarity(Rarity rarity) =>
            allEquipments.FindAll(e => e.rarity == rarity);

        public EquipmentData GetEquipmentById(string id)
        {
            if (equipmentById.TryGetValue(id, out var equip))
                return equip;

            Debug.LogWarning($"[EquipmentDatabase] 존재하지 않는 장비 ID: {id}");
            return null;
        }
    }
}
