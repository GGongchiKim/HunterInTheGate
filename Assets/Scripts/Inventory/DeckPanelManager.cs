using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

namespace Inventory
{
    public class DeckPanelManager : MonoBehaviour
    {
        [Header("Parent Toggles")]
        [SerializeField] private Toggle typeFilterToggle;
        [SerializeField] private Toggle rarityFilterToggle;
        [SerializeField] private Toggle costFilterToggle;

        [Header("Filter Panels")]
        [SerializeField] private GameObject typeFilterPanel;
        [SerializeField] private GameObject rarityFilterPanel;
        [SerializeField] private GameObject costFilterPanel;

        [Header("Filter Toggles")]
        [SerializeField] private Toggle strikeToggle;
        [SerializeField] private Toggle graceToggle;
        [SerializeField] private Toggle tacticToggle;
        [SerializeField] private Toggle itemToggle;

        [SerializeField] private Toggle commonToggle;
        [SerializeField] private Toggle uncommonToggle;
        [SerializeField] private Toggle rareToggle;
        [SerializeField] private Toggle legendaryToggle;

        [SerializeField] private Toggle cost1Toggle;
        [SerializeField] private Toggle cost2Toggle;
        [SerializeField] private Toggle cost3Toggle;
        [SerializeField] private Toggle cost4Toggle;
        [SerializeField] private Toggle cost5Toggle;

        [Header("UI Elements")]
        [SerializeField] private TMP_InputField searchInputField;
        [SerializeField] private Transform cardParent;

        [Header("Deck Preset Elements")]
        [SerializeField] private Button newDeckButton;
        [SerializeField] private Transform recipeSlotParent;
        [SerializeField] private GameObject deckPrefab;

        private readonly List<GameObject> spawnedCards = new();
        private readonly List<DeckPresetUI> deckUIs = new();

        private Dictionary<Toggle, CardType> typeMap;
        private Dictionary<Toggle, CardRarity> rarityMap;
        private Dictionary<Toggle, int> costMap;

        private DeckPresetUI currentOpenDeck = null;

        private void Awake()
        {
            InitializeFilterMaps();
        }

        private void Start()
        {
            typeFilterToggle.onValueChanged.AddListener(isOn => typeFilterPanel.SetActive(isOn));
            rarityFilterToggle.onValueChanged.AddListener(isOn => rarityFilterPanel.SetActive(isOn));
            costFilterToggle.onValueChanged.AddListener(isOn => costFilterPanel.SetActive(isOn));

            RegisterFilterToggleEvents(typeMap.Keys);
            RegisterFilterToggleEvents(rarityMap.Keys);
            RegisterFilterToggleEvents(costMap.Keys);

            searchInputField.onValueChanged.AddListener(_ => ApplyFilters());

            newDeckButton.onClick.AddListener(AddNewDeck);

            RefreshPanel();
        }

        private void InitializeFilterMaps()
        {
            typeMap = new()
            {
                { strikeToggle, CardType.Strike },
                { graceToggle, CardType.Grace },
                { tacticToggle, CardType.Tactic },
                { itemToggle, CardType.Item }
            };

            rarityMap = new()
            {
                { commonToggle, CardRarity.Common },
                { uncommonToggle, CardRarity.Uncommon },
                { rareToggle, CardRarity.Rare },
                { legendaryToggle, CardRarity.Legendary }
            };

            costMap = new()
            {
                { cost1Toggle, 1 },
                { cost2Toggle, 2 },
                { cost3Toggle, 3 },
                { cost4Toggle, 4 },
                { cost5Toggle, 5 }
            };
        }

        private void RegisterFilterToggleEvents(IEnumerable<Toggle> toggles)
        {
            foreach (var toggle in toggles)
                toggle.onValueChanged.AddListener(_ => ApplyFilters());
        }

        public void RefreshPanel()
        {
            ApplyFilters();
        }

        public void ApplyFilters()
        {
            var allCards = PlayerInventory.Instance.GetAllCards();
            var keyword = searchInputField.text.ToLower();

            var activeTypes = typeFilterToggle.isOn ? typeMap.Where(kv => kv.Key.isOn).Select(kv => kv.Value).ToHashSet() : null;
            var activeRarities = rarityFilterToggle.isOn ? rarityMap.Where(kv => kv.Key.isOn).Select(kv => kv.Value).ToHashSet() : null;
            var activeCosts = costFilterToggle.isOn ? costMap.Where(kv => kv.Key.isOn).Select(kv => kv.Value).ToHashSet() : null;

            var filtered = allCards.Where(card =>
                (activeTypes == null || activeTypes.Count == 0 || activeTypes.Contains(card.cardType)) &&
                (activeRarities == null || activeRarities.Count == 0 || activeRarities.Contains(card.rarity)) &&
                (activeCosts == null || activeCosts.Count == 0 || activeCosts.Contains(card.cardCost)) &&
                MatchesSearch(card, keyword)
            ).ToList();

            ShowCards(filtered);
        }

        private bool MatchesSearch(CardData card, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return true;
            return card.cardName.ToLower().Contains(keyword) || card.cardDescription.ToLower().Contains(keyword);
        }

        private void ShowCards(List<CardData> cards)
        {
            ClearCardList();
            foreach (var data in cards)
            {
                var card = InventoryUIManager.Instance.CreateBattleCard(data, cardParent, () => OnCardClicked(data));
                if (card != null)
                    spawnedCards.Add(card);
            }
        }

        private void ClearCardList()
        {
            foreach (var card in spawnedCards)
                Destroy(card);
            spawnedCards.Clear();
        }

        private void OnCardClicked(CardData card)
        {
            if (currentOpenDeck != null)
            {
                currentOpenDeck.AddCard(card.cardName); // cardId 기반으로 사용
            }
            else
            {
                Debug.Log("덱이 열려 있지 않습니다. 먼저 덱을 선택하세요.");
            }
        }

        public void AddNewDeck()
        {
            GameObject go = Instantiate(deckPrefab, recipeSlotParent);
            var deckUI = go.GetComponent<DeckPresetUI>();
            deckUI.InitializeEmpty(this);
            deckUIs.Add(deckUI);

            go.transform.SetSiblingIndex(recipeSlotParent.childCount - 2);
        }

        public void OnAnyDeckOpened(DeckPresetUI openedDeck)
        {
            currentOpenDeck = openedDeck;

            newDeckButton.gameObject.SetActive(false);

            foreach (var deck in deckUIs)
            {
                if (deck == openedDeck)
                {
                    deck.transform.SetSiblingIndex(0);
                    deck.gameObject.SetActive(true);
                }
                else
                {
                    deck.gameObject.SetActive(false);
                }
            }
        }

        public void OnAllDecksClosed()
        {
            currentOpenDeck = null;

          
            newDeckButton.gameObject.SetActive(true);

            foreach (var deck in deckUIs)
            {
                deck.gameObject.SetActive(true);
            }
        }
    }
}