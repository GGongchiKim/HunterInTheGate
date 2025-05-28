using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RewardCardSlotUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Image spriteImage;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button slotButton;

    private CardData cardData;
    private Action<CardData> onSelected;

    public void SetCard(CardData data, Action<CardData> onSelect)
    {
        cardData = data;
        onSelected = onSelect;

        if (titleText == null || spriteImage == null || descriptionText == null)
        {
            Debug.LogError("[RewardCardSlotUI] UI 요소가 연결되지 않았습니다.");
            return;
        }

        if (cardData == null)
        {
            Debug.LogError("[RewardCardSlotUI] CardData가 null입니다.");
            return;
        }

        titleText.text = data.cardName;
        spriteImage.sprite = data.cardSprite;
        descriptionText.text = data.cardDescription;
        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(() => onSelected?.Invoke(cardData));
    }
}