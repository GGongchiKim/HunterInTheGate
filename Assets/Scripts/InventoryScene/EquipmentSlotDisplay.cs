using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Inventory
{
    public class EquipmentSlotDisplay : MonoBehaviour
    {
        [Header("UI 참조")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text descText;

        // 장비 정보 표시
        public void SetEquipment(EquipmentData data)
        {
            if (nameText != null)
                nameText.text = data.displayName;

            if (iconImage != null)
            {
                iconImage.sprite = data.icon;
                iconImage.color = Color.white; // 투명 상태였다면 보이도록
            }

            if (descText != null)
                descText.text = data.description;
        }

        // 슬롯 초기화 (장비 해제 시)
        public void Clear()
        {
            if (nameText != null)
                nameText.text = "";

            if (iconImage != null)
            {
                iconImage.sprite = null;
                iconImage.color = Color.clear; // 안 보이게 처리
            }

            if (descText != null)
                descText.text = "";
        }
    }
}