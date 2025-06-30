using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    [Header("카드 생성 프리팹")]
    public GameObject cardPrefab;

    [Header("카드가 생성될 영역")]
    public Transform cardAreaTF;

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

       
        if (cardAreaTF == null)
        {
            GameObject found = GameObject.Find("CardArea");
            if (found != null)
            {
                cardAreaTF = found.transform;
                Debug.Log("[CardManager] CardArea 자동 연결 완료");
            }
            else
            {
                Debug.LogWarning("[CardManager] CardArea 객체를 씬에서 찾을 수 없습니다. 수동 연결 필요");
            }
        }
    }

    /// <summary>
    /// 카드 데이터를 기반으로 카드 생성
    /// </summary>
    public void CreateCard(CardData data)
    {
        if (data == null)
        {
            Debug.LogError("[CardManager] CardData가 null입니다.");
            return;
        }

        if (cardPrefab == null || cardAreaTF == null)
        {
            Debug.LogError("[CardManager] 카드 생성에 필요한 참조(cardPrefab/cardAreaTF)가 설정되지 않았습니다.");
            return;
        }

        GameObject newCard = Instantiate(cardPrefab, cardAreaTF);
        CardUI cardUI = newCard.GetComponent<CardUI>();

        if (cardUI != null)
        {
            cardUI.Setup(data);
            Debug.Log($"[CardManager] 카드 생성 완료: {data.cardName}");
        }
        else
        {
            Debug.LogWarning("[CardManager] 생성된 카드에 CardUI 컴포넌트가 없습니다.");
        }
    }
}