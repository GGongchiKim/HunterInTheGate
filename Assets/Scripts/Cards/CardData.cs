using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CardRarity { Common, Uncommon, Rare, Legendary }
public enum CardType { Strike, Grace, Tactic, Item }
// Strike: ���� ī��
// Grace: ��� & ȸ�� ī��
// Tactic: ��ο�, �ڽ�Ʈ, �� ���� ��ȭ ��� Ư���� ȿ���� ���� ī��
// Item: �ѹ� ����ϸ� ������ ������� 1ȸ�� ī��

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
