using UnityEngine;
using UnityEngine.SceneManagement;
using SaveSystem;
using Inventory;

public class GameLoader : MonoBehaviour
{
    public static GameLoader Instance { get; private set; }

    public int SelectedSlotIndex { get; private set; }
    public PlayerSaveData LoadedData { get; private set; }

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

    /// <summary>
    /// 슬롯 인덱스를 지정하고 게임 씬으로 이동
    /// </summary>
    public void StartGameFromSlot(int slotIndex)
    {
        SelectedSlotIndex = slotIndex;

        // 데이터 로드
        LoadedData = SaveManager.Instance.GetSaveData(slotIndex);
        if (LoadedData == null)
        {
            Debug.LogError($"[GameLoader] 슬롯 {slotIndex}의 데이터를 불러오지 못했습니다.");
            return;
        }

        Debug.Log($"[GameLoader] 슬롯 {slotIndex + 1} 로드 성공. 씬 전환 중...");
        SceneManager.LoadScene("AcademyScene");
    }

    /// <summary>
    /// AcademyScene 초기화 시 호출됨
    /// </summary>
    public void ApplyDataToGame()
    {
        if (LoadedData == null)
        {
            Debug.LogError("[GameLoader] 로드된 데이터가 없습니다. 초기화 불가.");
            return;
        }

        // 1. 인벤토리
        PlayerInventory.Instance.LoadFromData(LoadedData.inventory);

        // 2. 능력치
        AcademyPlayer.Instance.ApplyStats(LoadedData.stats);

        // 3. 날짜
       // if (GameDateManager.Instance != null)
         //   GameDateManager.Instance.SetDate(LoadedData.progress.year, LoadedData.progress.week);

        // 4. 헌터 랭크, 골드 등
        AcademyPlayer.Instance.gold = LoadedData.progress.gold;
        AcademyPlayer.Instance.hunterRank = LoadedData.progress.hunterRank;

        // 5. 필요 시 퀘스트 등 추가 초기화
        Debug.Log("[GameLoader] 게임 데이터 적용 완료");
    }
}