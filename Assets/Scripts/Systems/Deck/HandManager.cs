using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance { get; private set; }

    [Header("핸드 UI")]
    public Transform handArea;
    public GameObject cardUIPrefab;

    private List<GameObject> handCards = new();

    private void Awake()
    {
        // ✅ 싱글톤 구조
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // ✅ 자동 연결 시도
        if (handArea == null)
        {
            GameObject found = GameObject.Find("CardArea");
            if (found != null)
            {
                handArea = found.transform;
                Debug.Log("[HandManager] HandArea 자동 연결 완료");
            }
            else
            {
                Debug.LogWarning("[HandManager] HandArea를 씬에서 찾지 못했습니다. 수동 연결 필요");
            }
        }
    }

    /// <summary>
    /// 현재 핸드 카드 개수 반환
    /// </summary>
    public int CurrentHandSize() => handCards.Count;

    /// <summary>
    /// 핸드 정리 및 모든 카드 버림/제거
    /// </summary>
    public void ClearHand()
    {
        foreach (GameObject card in handCards)
        {
            CardUI cardUI = card.GetComponent<CardUI>();
            if (cardUI != null)
            {
                CardData data = cardUI.GetCardData();
                if (data != null)
                {
                    DeckManager.Instance?.DiscardCard(data);
                }
            }

            Destroy(card);
        }

        handCards.Clear();
    }

    /// <summary>
    /// 핸드에 카드 추가
    /// </summary>
    public void AddCardsToHand(List<CardData> cardDatas)
    {
        if (cardDatas == null || handArea == null)
        {
            Debug.LogWarning("[HandManager] 카드 추가 실패: 입력 데이터 또는 handArea 누락");
            return;
        }

        Debug.Log($"[AddCardsToHand] 현재 핸드 수: {handCards.Count}, 추가 카드 수: {cardDatas.Count}");

        foreach (CardData data in cardDatas)
        {
            if (handCards.Count >= 10)
            {
                Debug.Log($"[Hand Full] 카드 [{data.cardName}]는 버려집니다.");
                DeckManager.Instance?.DiscardCard(data);
                continue;
            }

            GameObject cardUIObj = Instantiate(cardUIPrefab, handArea);
            CardUI cardUI = cardUIObj.GetComponent<CardUI>();

            if (cardUI != null)
            {
                cardUI.Setup(data);
            }

            handCards.Add(cardUIObj);
        }
    }

    /// <summary>
    /// 카드 사용/제거 처리
    /// </summary>
    public void RemoveCardFromHand(GameObject cardObj, CardData cardData)
    {
        StartCoroutine(DelayedRemove(cardObj, cardData));
    }

    private IEnumerator DelayedRemove(GameObject cardObj, CardData cardData)
    {
        yield return null; // OnEndDrag 이후 처리

        if (!handCards.Contains(cardObj)) yield break;

        handCards.Remove(cardObj);

        if (cardData != null)
        {
            if (cardData.cardType == CardType.Item)
            {
                DeckManager.Instance?.ExhaustCard(cardData);
                Debug.Log($"[{cardData.cardName}] → 아이템 카드로 완전 제거");
            }
            else
            {
                DeckManager.Instance?.DiscardCard(cardData);
                Debug.Log($"[{cardData.cardName}] → 일반 카드로 버림");
            }
        }

        Destroy(cardObj);
    }
}