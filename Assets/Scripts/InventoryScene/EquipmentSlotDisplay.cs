using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Inventory
{
    public class EquipmentSlotDisplay : MonoBehaviour
    {
        [Header("UI ����")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text descText;

        // ��� ���� ǥ��
        public void SetEquipment(EquipmentData data)
        {
            if (nameText != null)
                nameText.text = data.displayName;

            if (iconImage != null)
            {
                iconImage.sprite = data.icon;
                iconImage.color = Color.white; // ���� ���¿��ٸ� ���̵���
            }

            if (descText != null)
                descText.text = data.description;
        }

        // ���� �ʱ�ȭ (��� ���� ��)
        public void Clear()
        {
            if (nameText != null)
                nameText.text = "";

            if (iconImage != null)
            {
                iconImage.sprite = null;
                iconImage.color = Color.clear; // �� ���̰� ó��
            }

            if (descText != null)
                descText.text = "";
        }
    }
}