using System;
using System.Collections.Generic;

/// <summary>
/// �� �������� �����͸� ��Ÿ���� Ŭ����.
/// �� �̸��� ī�� ID ����Ʈ�� ����.
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
    /// �ӽ� ���纻�� �����Ͽ� ��ȯ (��� ��� ����)
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