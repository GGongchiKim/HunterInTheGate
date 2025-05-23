using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CardRarity { Common, Uncommon, Rare, Legendary }
public enum CardType { Strike, Grace, Tactic, Item }
// Strike: 공격 카드
// Grace: 방어 & 회복 카드
// Tactic: 드로우, 코스트, 적 패턴 변화 등등 특수한 효과를 가진 카드
// Item: 한번 사용하면 덱에서 사라지는 1회용 카드

[CreateAssetMenu(fileName = "NewCardData", menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    [Header("Basic Info")]
    public string cardName;
    public int cardCost;
    public CardType cardType;
    public CardRarity rarity;
    public CardEffect cardEffect;
    
    [TextArea(3, 5)]
    public string cardDescription;
    public Sprite cardSprite;

}
