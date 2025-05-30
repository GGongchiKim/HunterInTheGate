using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform cardAreaTF;

    public void CreateCard(CardData data)
    {
        if (data == null)
        {
            Debug.LogError("CardManager: CardData가 null입니다.");
            return;
        }

        GameObject newCard = Instantiate(cardPrefab, cardAreaTF);
        CardUI cardUI = newCard.GetComponent<CardUI>();

        if (cardUI != null)
        {
            cardUI.Setup(data);
        }
    }

}
