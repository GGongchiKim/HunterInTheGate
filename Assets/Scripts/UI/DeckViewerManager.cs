using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckViewerManager : MonoBehaviour
{


    public static DeckViewerManager Instance { get; private set; }

    [Header("UI Parents")]
    public Transform deckParent;          // ���� �� ī����� ���� �θ� ������Ʈ
    public Transform discardParent;       // ������ �� ī����� ���� �θ� ������Ʈ

    [Header("Card UI Prefab")]
    public GameObject cardUIPrefab;        // ī�� �ϳ��� ������ UI ������

    [Header("Close Button")]
    public Button closeButton;             // ����� �ݱ� ��ư (����)

    [Header("Deck&Discard Panel")]
    public GameObject deckPanel;

    [Header("Texts")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    [Header("Count Texts")]
    public TextMeshProUGUI deckCountText;
    public TextMeshProUGUI discardCountText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseViewer);
    }

    private void Start()
    {
        if (DeckManager.Instance != null)
        {
            DeckManager.Instance.OnDeckChanged += UpdateDeckCount;
            DeckManager.Instance.OnDiscardChanged += UpdateDiscardCount;
        }

    }

    private void OnDestroy()
    {
        if (DeckManager.Instance != null)
        {
            DeckManager.Instance.OnDeckChanged -= UpdateDeckCount;
            DeckManager.Instance.OnDiscardChanged -= UpdateDiscardCount;
        }
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    public void OpenDeckOnly()
    {
        deckPanel.SetActive(true);
        ClearAllCards();
        RefreshDeck();

        if (titleText != null) titleText.text = "Combat Deck";
        if (descriptionText != null) descriptionText.text = "This is a list of cards you can use in the current battle.";
    }

    /// <summary>
    /// ���� �и� ����
    /// </summary>
    public void OpenDiscardOnly()
    {
        deckPanel.SetActive(true);
        ClearAllCards();
        RefreshDiscard();

        if (titleText != null) titleText.text = "Discarded List";
        if (descriptionText != null) descriptionText.text = "This is a list of cards used or discarded during this battle.";
    }

    /// <summary>
    /// �� �� �ݴ´�
    /// </summary>
    public void CloseViewer()
    {
        ClearAllCards();
        deckPanel.SetActive(false);
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    private void RefreshDeck()
    {
        List<CardData> combatDeck = DeckManager.Instance.GetCombatDeck();
        RefreshList(combatDeck, deckParent);
    }

    /// <summary>
    /// �����и� ����
    /// </summary>
    private void RefreshDiscard()
    {
        List<CardData> discardPile = DeckManager.Instance.GetDiscardPile();
        RefreshList(discardPile, discardParent);
    }

    /// <summary>
    /// ī�� ����Ʈ�� �θ� �Ʒ��� ����
    /// </summary>
    private void RefreshList(List<CardData> cards, Transform parent)
    {
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
                cardUI.Setup(card); // ī�� �����ͷ� �ʱ�ȭ
                cardUI.SetInteractable(false);
            }
            else
            {
                Debug.LogWarning("CardUI ������Ʈ�� �����տ� �����ϴ�!");
            }
        }
    }

    /// <summary>
    /// ���� ���� �� ���� ī�� ������Ʈ ����
    /// </summary>
    private void ClearAllCards()
    {
        foreach (Transform child in deckParent)
            Destroy(child.gameObject);
        foreach (Transform child in discardParent)
            Destroy(child.gameObject);
    }

    private void UpdateDeckCount()
    {
        if (deckCountText != null)
        {
            int count = DeckManager.Instance.GetCombatDeck().Count;
            Debug.Log($"[UpdateDeckCount] ȣ���: ���� �� ī�� �� = {count}");
            deckCountText.text = count.ToString();
        }
        else
        {
            Debug.LogWarning("[UpdateDeckCount] deckCountText�� null�Դϴ�!");
        }
    }

    private void UpdateDiscardCount()
    {
        if (discardCountText != null)
        {
            int count = DeckManager.Instance.GetDiscardPile().Count;
            Debug.Log($"[UpdateDiscardCount] ȣ���: ���� ���� �� ī�� �� = {count}");
            discardCountText.text = count.ToString();
        }
        else
        {
            Debug.LogWarning("[UpdateDiscardCount] discardCountText�� null�Դϴ�!");
        }
    }

    public void ForceUpdateCounts()
    {           
      UpdateDeckCount();
      UpdateDiscardCount();
    }

    public void ForceRefreshAll()
    {
        RefreshDeck();
        RefreshDiscard();
    }

}
