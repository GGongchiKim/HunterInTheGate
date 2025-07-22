using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }

    [Header("덱 구성")]
    [SerializeField] private List<CardData> permanentDeck = new();
    [SerializeField] private List<CardData> temporaryDeck = new();

    [Header("전투 중 덱 상태")]
    [SerializeField] private List<CardData> combatDeck = new();
    [SerializeField] private List<CardData> discardPile = new();

    public event System.Action OnDeckChanged;
    public event System.Action OnDiscardChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// 전투 시작 시 전투용 덱을 초기화한다
    /// </summary>
    public void InitializeCombatDeck(List<CardData> selectedCards)
    {
        combatDeck.Clear();
        discardPile.Clear();

        if (selectedCards == null || selectedCards.Count == 0)
        {
            Debug.LogWarning("[DeckManager] 선택된 덱 카드가 비어 있습니다. 기본 임시 덱을 사용할 수 있습니다.");
            // 필요하다면 여기에 fallback 로직 삽입 가능
        }
        else
        {
            combatDeck.AddRange(selectedCards);
        }

        ShuffleList(combatDeck);

        Debug.Log($"[DeckManager] 전투 덱 초기화 완료 ({combatDeck.Count}장)");
        OnDeckChanged?.Invoke();
        OnDiscardChanged?.Invoke();
    }

    /// <summary>
    /// 덱에서 카드를 드로우한다
    /// </summary>
    public List<CardData> DrawCards(int count)
    {
        List<CardData> drawnCards = new();

        for (int i = 0; i < count; i++)
        {
            if (combatDeck.Count == 0 && discardPile.Count > 0)
            {
                ReshuffleDiscardIntoCombatDeck();
            }

            if (combatDeck.Count > 0)
            {
                CardData drawn = combatDeck[0];
                combatDeck.RemoveAt(0);
                drawnCards.Add(drawn);

                Debug.Log($"[Draw] 카드 [{drawn.cardName}] 드로우됨");
                OnDeckChanged?.Invoke();
            }
            else
            {
                Debug.Log("[Draw] 드로우 실패: 덱이 비어 있음");
                break;
            }
        }

        return drawnCards;
    }

    /// <summary>
    /// 단일 카드 버림
    /// </summary>
    public void DiscardCard(CardData card)
    {
        if (card == null)
        {
            Debug.LogWarning("[DiscardCard] 카드가 null입니다.");
            return;
        }

        discardPile.Add(card);
        Debug.Log($"[Discard] 카드 [{card.cardName}]가 버려졌습니다 (현재 {discardPile.Count}장)");
        OnDiscardChanged?.Invoke();
    }

    /// <summary>
    /// 복수 카드 버림
    /// </summary>
    public void DiscardCards(List<CardData> cards)
    {
        if (cards == null || cards.Count == 0)
        {
            Debug.LogWarning("[DiscardCards] 전달된 카드 리스트가 비어 있습니다.");
            return;
        }

        discardPile.AddRange(cards);
        Debug.Log($"[Discard] 카드 {cards.Count}장 버려짐 (총 버림 {discardPile.Count}장)");
        OnDiscardChanged?.Invoke();
    }

    /// <summary>
    /// 버린 더미를 다시 전투 덱으로 셔플
    /// </summary>
    private void ReshuffleDiscardIntoCombatDeck()
    {
        combatDeck.AddRange(discardPile);
        discardPile.Clear();
        ShuffleList(combatDeck);

        Debug.Log("[DeckManager] 버린 패를 덱으로 셔플");
        OnDeckChanged?.Invoke();
        OnDiscardChanged?.Invoke();
    }

    /// <summary>
    /// 카드 완전 제거 (소멸)
    /// </summary>
    public void ExhaustCard(CardData card)
    {
        if (card == null)
        {
            Debug.LogWarning("[ExhaustCard] 카드가 null입니다.");
            return;
        }

        bool changed = false;

        if (combatDeck.Remove(card)) changed = true;
        if (discardPile.Remove(card)) changed = true;
        if (temporaryDeck.Remove(card)) changed = true;

        if (changed)
        {
            Debug.Log($"[Exhaust] 카드 [{card.cardName}] 완전히 제거됨");
            OnDeckChanged?.Invoke();
            OnDiscardChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning($"[Exhaust] 카드 [{card.cardName}]를 제거할 수 없습니다 (덱에 없음)");
        }
    }

    /// <summary>
    /// 리스트 셔플
    /// </summary>
    private void ShuffleList(List<CardData> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public List<CardData> GetCombatDeck() => combatDeck;
    public List<CardData> GetDiscardPile() => discardPile;
}