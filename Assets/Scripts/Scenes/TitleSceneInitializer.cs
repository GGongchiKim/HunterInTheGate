using Inventory;
using UnityEngine;

public class TitleSceneInitializer : MonoBehaviour
{
    [Header("초기화에 사용할 BGM ID")]
    [SerializeField] private string titleBgmId = "TitleBGM";

    private void Awake()
    {
        //  PlayerInventory 인스턴스 없으면 생성
        if (PlayerInventory.Instance == null)
        {
            GameObject prefab = Resources.Load<GameObject>("PlayerInventory");
            if (prefab != null)
            {
                Instantiate(prefab);
            }
            else
            {
                Debug.LogError("[TitleSceneInitializer] Resources에서 PlayerInventory 프리팹을 찾을 수 없습니다.");
            }
        }

        //  AudioManager 없으면 생성
        if (AudioManager.Instance == null)
        {
            GameObject audioManagerPrefab = Resources.Load<GameObject>("AudioManager");
            if (audioManagerPrefab != null)
            {
                Instantiate(audioManagerPrefab);
            }
            else
            {
                Debug.LogError("[TitleSceneInitializer] Resources에서 AudioManager 프리팹을 찾을 수 없습니다.");
            }
        }
    }

    private void Start()
    {
        //  게임 상태 설정
        GameStateManager.Instance?.SetPhase(GamePhase.Event);

        //  타이틀 BGM 재생
        AudioManager.Instance?.PlayBGM(titleBgmId);
    }
}