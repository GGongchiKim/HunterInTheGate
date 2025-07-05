using Inventory;
using UnityEngine;

public class TitleSceneInitializer : MonoBehaviour
{
    [Header("�ʱ�ȭ�� ����� BGM ID")]
    [SerializeField] private string titleBgmId = "TitleBGM";

    private void Awake()
    {
        //  PlayerInventory �ν��Ͻ� ������ ����
        if (PlayerInventory.Instance == null)
        {
            GameObject prefab = Resources.Load<GameObject>("PlayerInventory");
            if (prefab != null)
            {
                Instantiate(prefab);
            }
            else
            {
                Debug.LogError("[TitleSceneInitializer] Resources���� PlayerInventory �������� ã�� �� �����ϴ�.");
            }
        }

        //  AudioManager ������ ����
        if (AudioManager.Instance == null)
        {
            GameObject audioManagerPrefab = Resources.Load<GameObject>("AudioManager");
            if (audioManagerPrefab != null)
            {
                Instantiate(audioManagerPrefab);
            }
            else
            {
                Debug.LogError("[TitleSceneInitializer] Resources���� AudioManager �������� ã�� �� �����ϴ�.");
            }
        }
    }

    private void Start()
    {
        //  ���� ���� ����
        GameStateManager.Instance?.SetPhase(GamePhase.Event);

        //  Ÿ��Ʋ BGM ���
        AudioManager.Instance?.PlayBGM(titleBgmId);
    }
}