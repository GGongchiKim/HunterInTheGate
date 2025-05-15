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
    /// �پ��� Ÿ���� �κ��丮 ī�� �����͸� UI�� ���ε�
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

            // �ʿ� �� �ٸ� ī�� Ÿ�� �߰�
            default:
                Debug.LogWarning($"[InventoryCardUI] �� �� ���� Ÿ��: {data.GetType()}");
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