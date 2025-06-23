using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 적 1명에 대한 HUD (체력바, 상태효과, 행동 예고)를 관리하는 핸들러.
/// EnemyHUD 프리팹에 부착됨.
/// </summary>
public class EnemyHUDHandler : MonoBehaviour
{
    [Header("HUD 요소")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private RectTransform hudRoot; // 전체 UI 루트 위치 갱신용
    [SerializeField] private Transform statusEffectPanel;

    [Header("행동 예고 (Intent UI)")]
    [SerializeField] private EnemyIntentUI intentUI;

    private Enemy targetEnemy;
    private Camera mainCam;

    private Transform intentAnchor; // 중복 방지를 위한 앵커 캐시

    /// <summary>
    /// 적 유닛 연결 및 카메라 초기화
    /// </summary>
    public void Initialize(Enemy enemy)
    {
        targetEnemy = enemy;
        mainCam = Camera.main;

        // 체력바는 발 위치 기준으로 LateUpdate에서 위치 갱신

        // 의도 UI의 대상 위치 설정 (머리 위 기준)
        if (intentUI != null)
        {
            SpriteRenderer sr = enemy.GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                Vector3 headPos = new Vector3(sr.bounds.center.x, sr.bounds.max.y + 0.2f, sr.bounds.center.z);
                intentAnchor = CreateAnchor(headPos);
                intentUI.targetEnemyWorldPosition = intentAnchor;
            }
            else
            {
                intentUI.targetEnemyWorldPosition = enemy.transform;
            }
        }
    }

    private void LateUpdate()
    {
        if (targetEnemy == null || mainCam == null || hudRoot == null) return;

        SpriteRenderer sr = targetEnemy.GetComponentInChildren<SpriteRenderer>();
        if (sr != null && sr.bounds.size != Vector3.zero)
        {
            Vector3 foot = new Vector3(sr.bounds.center.x, sr.bounds.min.y - 0.05f, sr.bounds.center.z);
            hudRoot.position = mainCam.WorldToScreenPoint(foot);
        }
    }

    /// <summary>
    /// 체력 정보 갱신
    /// </summary>
    public void UpdateHealth(int current, int max, int shield = 0)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = max;
            healthBar.value = current;
            healthBar.fillRect.GetComponent<Image>().color =
                shield > 0 ? new Color(0.3f, 0.8f, 1f) : Color.red;
        }

        if (healthText != null)
        {
            healthText.text = shield > 0
                ? $"{current} / {max} + {shield}"
                : $"{current} / {max}";
        }
    }

    /// <summary>
    /// 상태이상 패널 Transform 반환
    /// </summary>
    public Transform GetStatusPanel() => statusEffectPanel;

    /// <summary>
    /// 행동 예고 UI 갱신
    /// </summary>
    public void UpdateIntent(EnemyActionPattern pattern)
    {
        intentUI?.UpdateIntent(pattern);
    }

    /// <summary>
    /// HUD 전체를 숨김 처리하는 함수 (적 사망 시 호출)
    /// </summary>
    public void HideHUD()
    {
        if (hudRoot != null)
            hudRoot.gameObject.SetActive(false);
    }

    /// <summary>
    /// 지정한 월드 위치에 고정된 UI용 앵커를 생성합니다.
    /// </summary>
    private Transform CreateAnchor(Vector3 worldPos)
    {
        if (intentAnchor != null) return intentAnchor; // 이미 있다면 재사용

        GameObject anchor = new GameObject("IntentAnchor");
        anchor.transform.SetParent(targetEnemy.transform); // 부모 설정
        anchor.transform.position = worldPos;
        intentAnchor = anchor.transform;
        return intentAnchor;
    }
}