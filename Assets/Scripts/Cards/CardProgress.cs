using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardProgress
{
    public CardData cardData;
    public int usageCount;

    public int GetLevel()
    {
        if (usageCount >= 30) return 3;
        if (usageCount >= 10) return 2;
        return 1;
    }
}