using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }

    [SerializeField] private List<CardData> permanentDeck = new List<CardData>();
    [SerializeField] private List<CardData> temporaryDeck = new List<CardData>();
    [SerializeField] private List<CardData> discardPile = new List<CardData>();
    [SerializeField] private List<CardData> combatDeck = new List<CardData>();

    public event System.Action OnDeckChanged;
    public event System.Action OnDiscardChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void InitializeCombatDeck()
    {
        combatDeck.Clear();
        combatDeck.AddRange(permanentDeck);
        combatDeck.AddRange(temporaryDeck);
        ShuffleList(combatDeck);
        Debug.Log("이니셜 컴뱃 - 덱변화 인보크");
        OnDeckChanged?.Invoke();
        OnDiscardChanged?.Invoke();
    }

    public List<CardData> DrawCards(int count)
    {
        List<CardData> drawnCards = new List<CardData>();

        for (int i = 0; i < count; i++)
        {
            if (combatDeck.Count == 0)
            {
                if (discardPile.Count > 0)
                    ReshuffleDiscardIntoCombatDeck();
            }
            if (combatDeck.Count > 0)
            {
                drawnCards.Add(combatDeck[0]);
                
                combatDeck.RemoveAt(0);
                Debug.Log("드로우카드 - 덱변화 인보크");
                OnDeckChanged?.Invoke();
            }
            else
            {
                break;
            }
        }

        return drawnCards;
    }

    public void DiscardCard(CardData card)
    {
        if (card == null) return;

        discardPile.Add(card);
        Debug.Log($"[DiscardPile] 카드 [{card.cardName}]가 버려졌습니다. 현재 버린 더미 수: {discardPile.Count}");
        Debug.Log("디스카드 - 버림패변화 인보크");
        OnDiscardChanged?.Invoke();
       
    }

    public void DiscardCards(List<CardData> cards)
    {
        if (cards == null || cards.Count == 0) return;

        discardPile.AddRange(cards);
        Debug.Log("디스카드카드즈 - 버림패변화 인보크");
        OnDiscardChanged?.Invoke();
    }

    private void ReshuffleDiscardIntoCombatDeck()
    {
        combatDeck.AddRange(discardPile);
        discardPile.Clear();
        ShuffleList(combatDeck);
        Debug.Log("리셔플 - 덱변화 인보크");
        OnDeckChanged?.Invoke();
        OnDiscardChanged?.Invoke();
    }

    private void ShuffleList(List<CardData> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    public void ExhaustCard(CardData card)
    {
        if (card == null)
        {
            Debug.LogWarning("ExhaustCard() - 카드가 null입니다.");
            return;
        }

        Debug.Log($"[Exhaust] 카드 [{card.cardName}]가 완전히 제거됩니다.");

        bool changed = false;

        if (combatDeck.Remove(card)) changed = true;
        if (discardPile.Remove(card)) changed = true;
        if (temporaryDeck.Remove(card)) changed = true;

        if (changed)
        {
          
            OnDeckChanged?.Invoke();
            OnDiscardChanged?.Invoke();
        }
    }

    public List<CardData> GetCombatDeck() => combatDeck;
    public List<CardData> GetDiscardPile() => discardPile;
}
