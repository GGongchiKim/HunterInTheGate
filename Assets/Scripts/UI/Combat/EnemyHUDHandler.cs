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
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    public RectTransform hudRoot; // ��ü UI ��Ʈ ��ġ ���ſ�
    public Transform statusEffectPanel;

    [Header("�ൿ ���� (Intent UI)")]
    public EnemyIntentUI intentUI;

    private Enemy targetEnemy;
    private Camera mainCam;

    /// <summary>
    /// �� ���� ���� �� ī�޶� �ʱ�ȭ
    /// </summary>
    public void Initialize(Enemy enemy)
    {
        targetEnemy = enemy;
        mainCam = Camera.main;

        //  �ǵ� UI�� ��� ��ġ ����
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
}
