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
        Debug.Log("�̴ϼ� �Ĺ� - ����ȭ �κ�ũ");
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
                Debug.Log("��ο�ī�� - ����ȭ �κ�ũ");
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
        Debug.Log($"[DiscardPile] ī�� [{card.cardName}]�� ���������ϴ�. ���� ���� ���� ��: {discardPile.Count}");
        Debug.Log("��ī�� - �����к�ȭ �κ�ũ");
        OnDiscardChanged?.Invoke();
       
    }

    public void DiscardCards(List<CardData> cards)
    {
        if (cards == null || cards.Count == 0) return;

        discardPile.AddRange(cards);
        Debug.Log("��ī��ī���� - �����к�ȭ �κ�ũ");
        OnDiscardChanged?.Invoke();
    }

    private void ReshuffleDiscardIntoCombatDeck()
    {
        combatDeck.AddRange(discardPile);
        discardPile.Clear();
        ShuffleList(combatDeck);
        Debug.Log("������ - ����ȭ �κ�ũ");
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
            Debug.LogWarning("ExhaustCard() - ī�尡 null�Դϴ�.");
            return;
        }

        Debug.Log($"[Exhaust] ī�� [{card.cardName}]�� ������ ���ŵ˴ϴ�.");

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
