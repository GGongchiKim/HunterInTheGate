using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Inventory;
using System.Linq;

public class DeckPresetUI : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private Button headerButton;
    [SerializeField] private GameObject recipePanel;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Transform recipeCardSlot;
    [SerializeField] private GameObject recipeCardPrefab;
    [SerializeField] private TMP_Text deckNameText;

    private DeckData originalData;
    private DeckData currentData;

    private DeckPanelManager panelManager;
    private bool isOpen = false;

    public void Initialize(DeckData data, DeckPanelManager manager)
    {
        originalData = data.DeepCopy();
        currentData = data.DeepCopy();
        panelManager = manager;

        deckNameText.text = currentData.deckName;
        RefreshRecipeCardUI();
    }

    public void InitializeEmpty(DeckPanelManager manager)
    {
        panelManager = manager;
        currentData = new DeckData("New Deck");
        originalData = currentData.DeepCopy();

        deckNameText.text = currentData.deckName;
        RefreshRecipeCardUI();
    }

    public void OnClickHeaderToggle()
    {
        isOpen = !isOpen;
        recipePanel.SetActive(isOpen);
        Debug.Log($"덱 패널 {(isOpen ? "열림" : "닫힘")}");

        if (isOpen)
        {
            if (panelManager == null)
            {
                Debug.LogWarning("panelManager가 null입니다!");
            }
            else
            {
                // 🔼 덱을 가장 위로 이동
                transform.SetSiblingIndex(0);
                panelManager.OnAnyDeckOpened(this);
            }
        }
        else
        {
            panelManager?.OnAllDecksClosed();
        }
    }

    public void AddCard(string cardId)
    {
        currentData.cardIds.Add(cardId);
        CreateRecipeCardUI(cardId);
    }

    public void RemoveCard(string cardId, GameObject cardGO)
    {
        currentData.cardIds.Remove(cardId);
        Destroy(cardGO);
    }

    public void Save()
    {
        originalData = currentData.DeepCopy();
    }

    public void Cancel()
    {
        currentData = originalData.DeepCopy();
        RefreshRecipeCardUI();
    }

    private void RefreshRecipeCardUI()
    {
        foreach (Transform child in recipeCardSlot)
        {
            Destroy(child.gameObject);
        }

        foreach (string cardId in currentData.cardIds)
        {
            CreateRecipeCardUI(cardId);
        }
    }

    private void CreateRecipeCardUI(string cardId)
    {
        GameObject go = Instantiate(recipeCardPrefab, recipeCardSlot);
        go.name = cardId;

        TMP_Text nameText = go.transform.Find("CardName")?.GetComponent<TMP_Text>();
        TMP_Text costText = go.transform.Find("CardCost")?.GetComponent<TMP_Text>();
        Image icon = go.transform.Find("CardSprite")?.GetComponent<Image>();

        var cardData = PlayerInventory.Instance.GetAllCards()
            .FirstOrDefault(c => c.cardName == cardId); // 고유 ID 있다면 cardId로 비교

        if (cardData != null)
        {
            if (nameText != null) nameText.text = cardData.cardName;
            if (costText != null) costText.text = cardData.cardCost.ToString();
            if (icon != null) icon.sprite = cardData.cardSprite;
        }
        else
        {
            Debug.LogWarning($"카드 데이터를 찾을 수 없음: {cardId}");
        }

        Button cardButton = go.GetComponent<Button>();
        if (cardButton != null)
        {
            cardButton.onClick.AddListener(() => RemoveCard(cardId, go));
        }
    }

    public void CloseRecipePanel()
    {
        isOpen = false;
        recipePanel.SetActive(false);
    }
}