using UnityEngine;
using UnityEngine.SceneManagement;
using SaveSystem;
using Inventory;

public class GameLoader : MonoBehaviour
{
    public static GameLoader Instance { get; private set; }

    public int selectedSlotIndex { get; private set; }
    public PlayerSaveData loadedData { get; private set; }

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

    public void StartGameFromSlot(int slotIndex)
    {
        selectedSlotIndex = slotIndex;

        // 데이터 로드
        var data = SaveManager.Instance.GetSaveData(slotIndex);
        if (data == null)
        {
            Debug.LogError($"[GameLoader] 슬롯 {slotIndex}의 데이터를 불러오지 못했습니다.");
            return;
        }

        loadedData = data;

        // 다음 씬으로 전환 (예: AcademyScene)
        SceneManager.LoadScene("AcademyScene");
    }

    // AcademyScene 초기화 시 사용됨
    public void ApplyDataToGame()
    {
        if (loadedData == null)
        {
            Debug.LogError("[GameLoader] 저장된 데이터가 없습니다. 초기화 불가.");
            return;
        }

        PlayerInventory.Instance.LoadFromData(loadedData.inventory);
        AcademyPlayer.Instance.ApplyStats(loadedData.stats);
        //GameDateManager.Instance.SetDate(loadedData.progress.year, loadedData.progress.week);
        // 퀘스트/명성 등 필요한 추가 처리도 여기에
    }
}