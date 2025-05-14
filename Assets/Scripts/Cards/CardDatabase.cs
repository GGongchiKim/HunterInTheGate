using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class CardDatabase : MonoBehaviour
    {
        public static CardDatabase Instance { get; private set; }

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
            allCards.AddRange(Resources.LoadAll<CardData>("Cards"));
        }

        public List<CardData> GetAllCards()
        {
            return new List<CardData>(allCards);
        }

        public List<CardData> GetCardsByType(CardType type)
        {
            return allCards.FindAll(card => card.cardType == type);
        }

        public List<CardData> FilterByRarity(CardRarity rarity)
        {
            return allCards.FindAll(card => card.rarity == rarity);
        }

        public List<CardData> FilterByCost(int cost)
        {
            return allCards.FindAll(card => card.cardCost == cost);
        }
    }
}