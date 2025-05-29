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
    /// ���� �ε����� �����ϰ� ���� ������ �̵�
    /// </summary>
    public void StartGameFromSlot(int slotIndex)
    {
        SelectedSlotIndex = slotIndex;

        // ������ �ε�
        LoadedData = SaveManager.Instance.GetSaveData(slotIndex);
        if (LoadedData == null)
        {
            Debug.LogError($"[GameLoader] ���� {slotIndex}�� �����͸� �ҷ����� ���߽��ϴ�.");
            return;
        }

        Debug.Log($"[GameLoader] ���� {slotIndex + 1} �ε� ����. �� ��ȯ ��...");
        SceneManager.LoadScene("AcademyScene");
    }

    /// <summary>
    /// AcademyScene �ʱ�ȭ �� ȣ���
    /// </summary>
    public void ApplyDataToGame()
    {
        if (LoadedData == null)
        {
            Debug.LogError("[GameLoader] �ε�� �����Ͱ� �����ϴ�. �ʱ�ȭ �Ұ�.");
            return;
        }

        // 1. �κ��丮
        PlayerInventory.Instance.LoadFromData(LoadedData.inventory);

        // 2. �ɷ�ġ
        AcademyPlayer.Instance.ApplyStats(LoadedData.stats);

        // 3. ��¥
       // if (GameDateManager.Instance != null)
         //   GameDateManager.Instance.SetDate(LoadedData.progress.year, LoadedData.progress.week);

        // 4. ���� ��ũ, ��� ��
        AcademyPlayer.Instance.gold = LoadedData.progress.gold;
        AcademyPlayer.Instance.hunterRank = LoadedData.progress.hunterRank;

        // 5. �ʿ� �� ����Ʈ �� �߰� �ʱ�ȭ
        Debug.Log("[GameLoader] ���� ������ ���� �Ϸ�");
    }
}