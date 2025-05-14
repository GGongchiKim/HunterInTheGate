using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance { get; private set; }
    public Transform handArea;
    public GameObject cardUIPrefab;
    private List<GameObject> handCards = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public int CurrentHandSize()
    {
        return handCards.Count;
    }

    public void ClearHand()
    {
        foreach (GameObject card in handCards)
        {
            CardUI cardUI = card.GetComponent<CardUI>();
            if (cardUI != null)
            {
                CardData data = cardUI.GetCardData();
                if (data != null)
                    DeckManager.Instance.DiscardCard(data);
            }

            Destroy(card);
        }

        handCards.Clear();
    }

    public void AddCardsToHand(List<CardData> cardDatas)
    {
        Debug.Log($"[AddCardsToHand] ȣ��� - ���� �ڵ� ��: {handCards.Count}, �߰� ī�� ��: {cardDatas.Count}");

        foreach (CardData data in cardDatas)
        {
            if (handCards.Count >= 10)
            {
                Debug.Log($"[Hand Full] ī�� [{data.cardName}]�� �������ϴ�.");
                DeckManager.Instance.DiscardCard(data);
                continue;
            }

            GameObject cardUIObj = Instantiate(cardUIPrefab, handArea);
            CardUI cardUI = cardUIObj.GetComponent<CardUI>();
            cardUI.Setup(data);
            handCards.Add(cardUIObj);
        }

       
    }

    public void RemoveCardFromHand(GameObject cardObj, CardData cardData)
    {
        StartCoroutine(DelayedRemove(cardObj, cardData));
    }

    private IEnumerator DelayedRemove(GameObject cardObj, CardData cardData)
    {
        yield return null; // �� ������ ��� (OnEndDrag ���� �� ����)

        if (handCards.Contains(cardObj))
        {
            handCards.Remove(cardObj);

            // ī�� Ÿ�Կ� ���� �б� ó��
            if (cardData.cardType == CardType.Item)
            {
                DeckManager.Instance.ExhaustCard(cardData);
                Debug.Log($"[{cardData.cardName}] �� ������ ī���̹Ƿ� ������ ���ŵ�");
            }
            else
            {
                DeckManager.Instance.DiscardCard(cardData);
                Debug.Log($"[{cardData.cardName}] �� �Ϲ� ī���̹Ƿ� ���� ���̷� �̵�");
            }

            Destroy(cardObj);
        }
    }
}