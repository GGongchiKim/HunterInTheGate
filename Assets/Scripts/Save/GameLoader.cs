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

        // ������ �ε�
        var data = SaveManager.Instance.GetSaveData(slotIndex);
        if (data == null)
        {
            Debug.LogError($"[GameLoader] ���� {slotIndex}�� �����͸� �ҷ����� ���߽��ϴ�.");
            return;
        }

        loadedData = data;

        // ���� ������ ��ȯ (��: AcademyScene)
        SceneManager.LoadScene("AcademyScene");
    }

    // AcademyScene �ʱ�ȭ �� ����
    public void ApplyDataToGame()
    {
        if (loadedData == null)
        {
            Debug.LogError("[GameLoader] ����� �����Ͱ� �����ϴ�. �ʱ�ȭ �Ұ�.");
            return;
        }

        PlayerInventory.Instance.LoadFromData(loadedData.inventory);
        AcademyPlayer.Instance.ApplyStats(loadedData.stats);
        //GameDateManager.Instance.SetDate(loadedData.progress.year, loadedData.progress.week);
        // ����Ʈ/�� �� �ʿ��� �߰� ó���� ���⿡
    }
}