using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Inventory;
using System.Linq;
using UnityEngine.EventSystems;

public class DeckPresetUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI 참조")]
    [SerializeField] private Button headerButton;
    [SerializeField] private GameObject recipePanel;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Transform recipeCardSlot;
    [SerializeField] private GameObject recipeCardPrefab;
    [SerializeField] private TMP_InputField deckNameInput;

    private DeckData originalData;
    private DeckData currentData;

    private DeckPanelManager panelManager;
    private bool isOpen = false;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private LayoutElement layoutElement;
    private int originalSiblingIndex;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement == null)
        {
            layoutElement = gameObject.AddComponent<LayoutElement>();
        }
    }

    public void Initialize(DeckData data, DeckPanelManager manager)
    {
        originalData = data.DeepCopy();
        currentData = data.DeepCopy();
        panelManager = manager;

        deckNameInput.text = currentData.deckName;
        RefreshRecipeCardUI();
    }

    public void InitializeEmpty(DeckPanelManager manager)
    {
        panelManager = manager;
        currentData = new DeckData("New Deck");
        originalData = currentData.DeepCopy();

        deckNameInput.text = currentData.deckName;
        RefreshRecipeCardUI();

        isOpen = false;
        recipePanel.SetActive(false);
        deckNameInput.interactable = false;
    }

    public void OnClickHeaderToggle()
    {
        isOpen = !isOpen;
        recipePanel.SetActive(isOpen);
        Debug.Log($"덱 패널 {(isOpen ? "열림" : "닫힘")}");
        deckNameInput.interactable = isOpen;

        if (isOpen)
        {
            if (panelManager == null)
            {
                Debug.LogWarning("panelManager가 null입니다!");
            }
            else
            {
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
        currentData.deckName = deckNameInput.text;
        originalData = currentData.DeepCopy();
        CloseRecipePanel();
        deckNameInput.interactable = false;
        panelManager?.OnAllDecksClosed();
    }

    public void Cancel()
    {
        currentData = originalData.DeepCopy();
        deckNameInput.text = currentData.deckName;
        RefreshRecipeCardUI();
        CloseRecipePanel();
        deckNameInput.interactable = false;
        panelManager?.OnAllDecksClosed();
    }

    public void Delete()
    {
        panelManager?.RemoveDeck(this);
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
            .FirstOrDefault(c => c.cardName == cardId);

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        layoutElement.ignoreLayout = true;
        originalSiblingIndex = transform.GetSiblingIndex();
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.delta / panelManager.GetCanvasScaleFactor();
        rectTransform.anchoredPosition += new Vector2(0f, delta.y);
        panelManager.UpdateDeckInsertVisual(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        layoutElement.ignoreLayout = false;

        panelManager.ReorderDeck(this, transform.GetSiblingIndex(), originalSiblingIndex);
    }
}
