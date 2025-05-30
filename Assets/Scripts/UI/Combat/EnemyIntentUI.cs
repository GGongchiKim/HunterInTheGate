using TMPro;
using UnityEngine;

public class EnemyIntentUI : MonoBehaviour
{
    [Header("UI Components")]
    public SpriteRenderer iconSprite;
    public TextMeshPro iconText;

    [Header("Intent Icons")]
    public Sprite swordIcon;
    public Sprite shieldIcon;
    public Sprite swordAndShieldIcon;

    [Header("Intent Colors")]
    public Color attackColor = Color.red;
    public Color defenseColor = Color.cyan;
    public Color comboColor = Color.magenta;

    [Header("타겟 적 스프라이트")]
    public Transform targetEnemySprite;  // SpriteRenderer가 붙은 트랜스폼

    public Vector3 offset = new Vector3(0, 1.5f, 0); // 머리 위에 뜨도록 설정

    private void LateUpdate()
    {
        if (targetEnemySprite != null)
        {
            transform.position = targetEnemySprite.position + offset;
        }
    }

    public void UpdateIntent(EnemyActionPattern pattern)
    {
        if (pattern == null || iconSprite == null || iconText == null)
        {
            if (iconSprite != null) iconSprite.enabled = false;
            if (iconText != null) iconText.text = "";
            return;
        }

        bool hasAttack = pattern.damage > 0;
        bool hasShield = pattern.shield > 0;

        // 아이콘 결정
        if (hasAttack && hasShield)
        {
            iconSprite.sprite = swordAndShieldIcon;
            iconSprite.color = comboColor;
            iconSprite.flipX = false;
        }
        else if (hasAttack)
        {
            iconSprite.sprite = swordIcon;
            iconSprite.color = attackColor;
            iconSprite.flipX = true;
        }
        else if (hasShield)
        {
            iconSprite.sprite = shieldIcon;
            iconSprite.color = defenseColor;
            iconSprite.flipX = false;
        }
        else
        {
            iconSprite.enabled = false;
            iconText.text = "";
            return;
        }

        iconSprite.enabled = true;

        // 수치 텍스트
        if (hasAttack && hasShield)
            iconText.text = $"{pattern.damage} / +{pattern.shield}";
        else if (hasAttack)
            iconText.text = $"{pattern.damage}";
        else if (hasShield)
            iconText.text = $"+{pattern.shield}";
        else
            iconText.text = "";
    }
}