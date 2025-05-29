using System;
using System.IO;
using UnityEngine;
using SaveSystem;
using Inventory;

namespace SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private const string SaveFolderName = "SaveData";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private string GetSaveDirectory()
        {
            string path = Path.Combine(Application.persistentDataPath, SaveFolderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        private string GetSlotPath(int slotIndex)
        {
            return Path.Combine(GetSaveDirectory(), $"slot_{slotIndex + 1}.json");
        }

        // ?? 저장
        public void SaveGame(int slotIndex)
        {
            var saveData = new PlayerSaveData
            {
                stats = new PlayerStatsSaveData(
                    AcademyPlayer.Instance.combat,
                    AcademyPlayer.Instance.relation,
                    AcademyPlayer.Instance.condition
                ),
                inventory = PlayerInventory.Instance.GetSaveData(),
                progress = new GameProgressSaveData
                {
                    playerName = AcademyPlayer.Instance.playerName,
                  //  inGameYear = GameDateManager.Instance.Year,
                  //  inGameWeek = GameDateManager.Instance.Week,
                    hunterRank = AcademyPlayer.Instance.hunterRank,
                   // activeQuests = QuestManager.Instance.GetActiveQuestIds(),
                   // completedQuests = QuestManager.Instance.GetCompletedQuestIds()
                }
            };

            try
            {
                string json = JsonUtility.ToJson(saveData, true);
                string path = GetSlotPath(slotIndex);
                File.WriteAllText(path, json);
                Debug.Log($"[SaveManager] 저장 완료: {path}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] 저장 실패: {ex.Message}");
            }
        }

        // ?? 불러오기
        public void LoadGame(int slotIndex)
        {
            string path = GetSlotPath(slotIndex);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"[SaveManager] 슬롯 {slotIndex + 1}에 저장된 파일이 없습니다.");
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var saveData = JsonUtility.FromJson<PlayerSaveData>(json);
                if (saveData == null)
                {
                    Debug.LogWarning("[SaveManager] 로드된 데이터가 null입니다.");
                    return;
                }

                // 분배
                AcademyPlayer.Instance.ApplyStats(saveData.stats);
                PlayerInventory.Instance.LoadFromData(saveData.inventory);
                AcademyPlayer.Instance.playerName = saveData.progress.playerName;
                AcademyPlayer.Instance.hunterRank = saveData.progress.hunterRank;
                //GameDateManager.Instance.SetDate(saveData.progress.inGameYear, saveData.progress.inGameWeek);
                //QuestManager.Instance.LoadQuests(saveData.progress.activeQuests, saveData.progress.completedQuests);

                Debug.Log($"[SaveManager] 로드 완료: {path}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] 로드 실패: {ex.Message}");
            }
        }

        public PlayerSaveData GetSaveData(int slotIndex)
        {
            string path = GetSlotPath(slotIndex);
            if (!File.Exists(path)) return null;

            try
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<PlayerSaveData>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] 데이터 파싱 실패: {ex.Message}");
                return null;
            }
        }

        public bool SlotExists(int slotIndex)
        {
            return File.Exists(GetSlotPath(slotIndex));
        }

        public string GetSlotPathPublic(int slotIndex)
        {
            return GetSlotPath(slotIndex);
        }
    }
}