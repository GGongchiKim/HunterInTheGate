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
        Debug.Log($"[AddCardsToHand] 호출됨 - 현재 핸드 수: {handCards.Count}, 추가 카드 수: {cardDatas.Count}");

        foreach (CardData data in cardDatas)
        {
            if (handCards.Count >= 10)
            {
                Debug.Log($"[Hand Full] 카드 [{data.cardName}]는 버려집니다.");
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
        yield return null; // 한 프레임 대기 (OnEndDrag 종료 후 실행)

        if (handCards.Contains(cardObj))
        {
            handCards.Remove(cardObj);

            // 카드 타입에 따라 분기 처리
            if (cardData.cardType == CardType.Item)
            {
                DeckManager.Instance.ExhaustCard(cardData);
                Debug.Log($"[{cardData.cardName}] 는 아이템 카드이므로 완전히 제거됨");
            }
            else
            {
                DeckManager.Instance.DiscardCard(cardData);
                Debug.Log($"[{cardData.cardName}] 는 일반 카드이므로 버린 더미로 이동");
            }

            Destroy(cardObj);
        }
    }
}