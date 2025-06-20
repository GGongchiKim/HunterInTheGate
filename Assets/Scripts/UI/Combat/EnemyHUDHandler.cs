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
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    public RectTransform hudRoot; // 전체 UI 루트 위치 갱신용
    public Transform statusEffectPanel;

    [Header("행동 예고 (Intent UI)")]
    public EnemyIntentUI intentUI;

    private Enemy targetEnemy;
    private Camera mainCam;

    /// <summary>
    /// 적 유닛 연결 및 카메라 초기화
    /// </summary>
    public void Initialize(Enemy enemy)
    {
        targetEnemy = enemy;
        mainCam = Camera.main;

        //  의도 UI의 대상 위치 설정
        if (intentUI != null)
        {
            intentUI.targetEnemySprite = enemy.transform;
        }
    }

    private void LateUpdate()
    {
        if (targetEnemy == null || mainCam == null || hudRoot == null) return;

        SpriteRenderer sr = targetEnemy.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
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
}
