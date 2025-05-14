using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckViewerManager : MonoBehaviour
{


    public static DeckViewerManager Instance { get; private set; }

    [Header("UI Parents")]
    public Transform deckParent;          // 전투 덱 카드들이 나올 부모 오브젝트
    public Transform discardParent;       // 버려진 덱 카드들이 나올 부모 오브젝트

    [Header("Card UI Prefab")]
    public GameObject cardUIPrefab;        // 카드 하나당 보여줄 UI 프리팹

    [Header("Close Button")]
    public Button closeButton;             // 덱뷰어 닫기 버튼 (선택)

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
    /// 전투 덱만 열람
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
    /// 버린 패만 열람
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
    /// 덱 뷰어를 닫는다
    /// </summary>
    public void CloseViewer()
    {
        ClearAllCards();
        deckPanel.SetActive(false);
    }

    /// <summary>
    /// 전투 덱만 갱신
    /// </summary>
    private void RefreshDeck()
    {
        List<CardData> combatDeck = DeckManager.Instance.GetCombatDeck();
        RefreshList(combatDeck, deckParent);
    }

    /// <summary>
    /// 버림패만 갱신
    /// </summary>
    private void RefreshDiscard()
    {
        List<CardData> discardPile = DeckManager.Instance.GetDiscardPile();
        RefreshList(discardPile, discardParent);
    }

    /// <summary>
    /// 카드 리스트를 부모 아래에 갱신
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
                cardUI.Setup(card); // 카드 데이터로 초기화
                cardUI.SetInteractable(false);
            }
            else
            {
                Debug.LogWarning("CardUI 컴포넌트가 프리팹에 없습니다!");
            }
        }
    }

    /// <summary>
    /// 덱뷰어를 닫을 때 기존 카드 오브젝트 제거
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
            Debug.Log($"[UpdateDeckCount] 호출됨: 현재 덱 카드 수 = {count}");
            deckCountText.text = count.ToString();
        }
        else
        {
            Debug.LogWarning("[UpdateDeckCount] deckCountText가 null입니다!");
        }
    }

    private void UpdateDiscardCount()
    {
        if (discardCountText != null)
        {
            int count = DeckManager.Instance.GetDiscardPile().Count;
            Debug.Log($"[UpdateDiscardCount] 호출됨: 현재 버린 패 카드 수 = {count}");
            discardCountText.text = count.ToString();
        }
        else
        {
            Debug.LogWarning("[UpdateDiscardCount] discardCountText가 null입니다!");
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
