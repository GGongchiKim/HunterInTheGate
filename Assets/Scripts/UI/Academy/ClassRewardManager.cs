using Inventory;
using UnityEngine;

public class ClassRewardManager : MonoBehaviour
{
    [Header("���� UI")]
    [SerializeField] private GameObject rewardPanel; // ClassRewardPanel ��ü
    [SerializeField] private RewardCardSlotUI[] cardSlots;

    private CardData[] selectedCards;

    public void ShowRewards(ClassData classData)
    {
        rewardPanel.SetActive(true);
        selectedCards = new CardData[cardSlots.Length];

        for (int i = 0; i < cardSlots.Length; i++)
        {
            var rarity = GetRandomRarity(classData);
            var candidates = classData.rewardPool.FindAll(c => c.rarity == rarity);
            var card = candidates[UnityEngine.Random.Range(0, candidates.Count)];

            selectedCards[i] = card;
            cardSlots[i].SetCard(card, OnCardSelected);
        }
    }

    private void OnCardSelected(CardData selected)
    {
        PlayerInventory.Instance.AddCard(selected);
        rewardPanel.SetActive(false);
        Debug.Log($"ī�� ���� ȹ��: {selected.cardName}");

        // TODO: ���� �帧 ���� (HUD ����, ���� �� ���� ��)
    }

    private CardRarity GetRandomRarity(ClassData classData)
    {
        float roll = UnityEngine.Random.value;
        if (roll < 0.05f) return CardRarity.Legendary;
        if (roll < 0.20f) return CardRarity.Rare;
        if (roll < 0.50f) return CardRarity.Uncommon;
        return CardRarity.Common;
    }
}