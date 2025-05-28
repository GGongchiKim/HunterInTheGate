using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class CardDatabase : MonoBehaviour
    {
        public static CardDatabase Instance { get; private set; }

        private Dictionary<string, CardData> cardById = new();
        private List<CardData> allCards = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            LoadCards();
        }

        private void LoadCards()
        {
            allCards.Clear();
            cardById.Clear();

            var loadedCards = Resources.LoadAll<CardData>("Cards");
            foreach (var card in loadedCards)
            {
                if (!cardById.ContainsKey(card.cardId))
                {
                    cardById.Add(card.cardId, card);
                    allCards.Add(card);
                }
                else
                {
                    Debug.LogWarning($"[CardDatabase] 중복된 카드 ID 발견: {card.cardId}");
                }
            }
        }

        public List<CardData> GetAllCards() => new(allCards);

        public List<CardData> GetCardsByType(CardType type) =>
            allCards.FindAll(card => card.cardType == type);

        public List<CardData> FilterByRarity(CardRarity rarity) =>
            allCards.FindAll(card => card.rarity == rarity);

        public List<CardData> FilterByCost(int cost) =>
            allCards.FindAll(card => card.cardCost == cost);

        public CardData GetCardById(string id)
        {
            if (cardById.TryGetValue(id, out var card))
                return card;
            Debug.LogWarning($"[CardDatabase] 존재하지 않는 카드 ID: {id}");
            return null;
        }
    }
}
