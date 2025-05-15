using System;
using System.Collections.Generic;

/// <summary>
/// 덱 프리셋의 데이터를 나타내는 클래스.
/// 덱 이름과 카드 ID 리스트를 포함.
/// </summary>
[Serializable]
public class DeckData
{
    public string deckName;
    public List<string> cardIds = new();

    public DeckData() { }

    public DeckData(string name)
    {
        deckName = name;
        cardIds = new List<string>();
    }

    /// <summary>
    /// 임시 복사본을 생성하여 반환 (취소 기능 대비용)
    /// </summary>
    public DeckData DeepCopy()
    {
        return new DeckData
        {
            deckName = this.deckName,
            cardIds = new List<string>(this.cardIds)
        };
    }
}