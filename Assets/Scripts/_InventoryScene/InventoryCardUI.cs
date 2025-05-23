using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Inventory;

public class InventoryCardUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image iconImage;

    /// <summary>
    /// 다양한 타입의 인벤토리 카드 데이터를 UI에 바인딩
    /// </summary>
    public void Setup(object data)
    {
        switch (data)
        {
            case EquipmentData equip:
                nameText.text = equip.displayName;
                descriptionText.text = equip.description;
                iconImage.sprite = equip.icon;
                break;

            case ArtifactData artifact:
                nameText.text = artifact.displayName;
                descriptionText.text = artifact.description;
                iconImage.sprite = artifact.icon;
                break;

            // 필요 시 다른 카드 타입 추가
            default:
                Debug.LogWarning($"[InventoryCardUI] 알 수 없는 타입: {data.GetType()}");
                nameText.text = "Unknown";
                descriptionText.text = "Unknown data type.";
                iconImage.sprite = null;
                break;
        }

        if (iconImage != null && iconImage.sprite != null)
        {
            iconImage.preserveAspect = true;
        }
    }
}