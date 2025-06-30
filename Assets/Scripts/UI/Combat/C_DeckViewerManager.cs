using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class C_DeckViewerManager : MonoBehaviour
{
    public static C_DeckViewerManager Instance { get; private set; }

    [Header("UI Parents")]
    [SerializeField] private Transform deckParent;
    [SerializeField] private Transform discardParent;

    [Header("Card UI Prefab")]
    [SerializeField] private GameObject cardUIPrefab;

    [Header("Close Button")]
    [SerializeField] private Button closeButton;

    [Header("Deck&Discard Panel")]
    [SerializeField] private GameObject deckPanel;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private void Awake()
    {
        // ✅ Singleton 보장
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseViewer);
        }
    }

    private void OnEnable()
    {
        ForceRefreshAll(); // 버튼으로 활성화될 때마다 새로 갱신
    }

    /// <summary>
    /// 전투 덱만 열람
    /// </summary>
    public void OpenDeckOnly()
    {
        if (deckPanel != null) deckPanel.SetActive(true);
        ClearAllCards();
        RefreshDeck();

        if (titleText != null) titleText.text = "Combat Deck";
        if (descriptionText != null) descriptionText.text = "This is a list of cards you can use in the current battle.";
    }

    /// <summary>
    /// 버린 패만 열람
    /// </summary>
    public void OpenDiscardOnly()
    {
        if (deckPanel != null) deckPanel.SetActive(true);
        ClearAllCards();
        RefreshDiscard();

        if (titleText != null) titleText.text = "Discarded List";
        if (descriptionText != null) descriptionText.text = "This is a list of cards used or discarded during this battle.";
    }

    public void CloseViewer()
    {
        ClearAllCards();
        if (deckPanel != null) deckPanel.SetActive(false);
    }

    private void RefreshDeck()
    {
        if (deckParent == null || DeckManager.Instance == null) return;

        List<CardData> combatDeck = DeckManager.Instance.GetCombatDeck();
        RefreshList(combatDeck, deckParent);
    }

    private void RefreshDiscard()
    {
        if (discardParent == null || DeckManager.Instance == null) return;

        List<CardData> discardPile = DeckManager.Instance.GetDiscardPile();
        RefreshList(discardPile, discardParent);
    }

    private void RefreshList(List<CardData> cards, Transform parent)
    {
        if (parent == null || cardUIPrefab == null) return;

        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        foreach (CardData card in cards)
        {
            GameObject cardObj = Instantiate(cardUIPrefab, parent);
            CardUI cardUI = cardObj.GetComponent<CardUI>();

            if (cardUI != null)
            {
                cardUI.Setup(card);
                cardUI.SetInteractable(false);
            }
            else
            {
                Debug.LogWarning("[C_DeckViewerManager] CardUI 컴포넌트가 프리팹에 없습니다!");
            }
        }
    }

    private void ClearAllCards()
    {
        if (deckParent != null)
        {
            foreach (Transform child in deckParent)
                Destroy(child.gameObject);
        }

        if (discardParent != null)
        {
            foreach (Transform child in discardParent)
                Destroy(child.gameObject);
        }
    }

    public void ForceRefreshAll()
    {
        RefreshDeck();
        RefreshDiscard();
    }
}