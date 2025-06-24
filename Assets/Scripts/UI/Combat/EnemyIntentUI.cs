using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 적의 다음 행동을 표시하는 UI. 월드 공간 기준으로 머리 위에 위치하도록 동기화됨.
/// </summary>
public class EnemyIntentUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI iconText;

    [Header("Intent Icons")]
    [SerializeField] private Sprite swordIcon;
    [SerializeField] private Sprite shieldIcon;
    [SerializeField] private Sprite swordAndShieldIcon;

    [Header("Intent Colors")]
    [SerializeField] private Color attackColor = Color.red;
    [SerializeField] private Color defenseColor = Color.cyan;
    [SerializeField] private Color comboColor = Color.magenta;

    [Header("위치 참조")]
    public Transform targetEnemyWorldPosition; // EnemyHUDHandler에서 지정
    private Camera mainCamera;

    public Enemy linkedEnemy;


    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (targetEnemyWorldPosition == null || mainCamera == null) return;

        // 머리 위 앵커의 월드 위치 → 스크린 위치로 변환
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetEnemyWorldPosition.position);
        transform.position = screenPos;
    }

    /// <summary>
    /// 적의 다음 행동 패턴을 UI로 갱신합니다.
    /// </summary>
    public void UpdateIntent(EnemyActionPattern pattern)
    {
        if (iconImage == null || iconText == null)
        {
            Debug.LogWarning("[EnemyIntentUI] UI 컴포넌트가 연결되지 않았습니다.");
            return;
        }

        if (pattern == null || (pattern.damage <= 0 && pattern.shield <= 0))
        {
            iconImage.enabled = false;
            iconText.text = "";
            return;
        }

        // 아이콘 및 색상 설정
        if (pattern.damage > 0 && pattern.shield > 0)
        {
            iconImage.sprite = swordAndShieldIcon;
            iconImage.color = comboColor;
            iconText.text = $"{pattern.damage} / +{pattern.shield}";
        }
        else if (pattern.damage > 0)
        {
            iconImage.sprite = swordIcon;
            iconImage.color = attackColor;
            iconText.text = $"{pattern.damage}";
        }
        else // shield only
        {
            iconImage.sprite = shieldIcon;
            iconImage.color = defenseColor;
            iconText.text = $"+{pattern.shield}";
        }

        iconImage.enabled = true;
    }
}