using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �� 1�� ���� HUD (ü�¹�, ����ȿ��, �ൿ ����)�� �����ϴ� �ڵ鷯.
/// EnemyHUD �����տ� ������.
/// </summary>
public class EnemyHUDHandler : MonoBehaviour
{
    [Header("HUD ���")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private RectTransform hudRoot; // ��ü UI ��Ʈ ��ġ ���ſ�
    [SerializeField] private Transform statusEffectPanel;

    [Header("�ൿ ���� (Intent UI)")]
    [SerializeField] private EnemyIntentUI intentUI;

    private Enemy targetEnemy;
    private Camera mainCam;

    private Transform intentAnchor; // �ߺ� ������ ���� ��Ŀ ĳ��

    /// <summary>
    /// �� ���� ���� �� ī�޶� �ʱ�ȭ
    /// </summary>
    public void Initialize(Enemy enemy)
    {
        targetEnemy = enemy;
        mainCam = Camera.main;

        // ü�¹ٴ� �� ��ġ �������� LateUpdate���� ��ġ ����

        // �ǵ� UI�� ��� ��ġ ���� (�Ӹ� �� ����)
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
    /// ü�� ���� ����
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
    /// �����̻� �г� Transform ��ȯ
    /// </summary>
    public Transform GetStatusPanel() => statusEffectPanel;

    /// <summary>
    /// �ൿ ���� UI ����
    /// </summary>
    public void UpdateIntent(EnemyActionPattern pattern)
    {
        intentUI?.UpdateIntent(pattern);
    }

    /// <summary>
    /// HUD ��ü�� ���� ó���ϴ� �Լ� (�� ��� �� ȣ��)
    /// </summary>
    public void HideHUD()
    {
        if (hudRoot != null)
            hudRoot.gameObject.SetActive(false);
    }

    /// <summary>
    /// ������ ���� ��ġ�� ������ UI�� ��Ŀ�� �����մϴ�.
    /// </summary>
    private Transform CreateAnchor(Vector3 worldPos)
    {
        if (intentAnchor != null) return intentAnchor; // �̹� �ִٸ� ����

        GameObject anchor = new GameObject("IntentAnchor");
        anchor.transform.SetParent(targetEnemy.transform); // �θ� ����
        anchor.transform.position = worldPos;
        intentAnchor = anchor.transform;
        return intentAnchor;
    }
}