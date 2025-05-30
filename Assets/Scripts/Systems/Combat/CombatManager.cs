using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public void ProcessCardEffect(CardUI droppedCard)
    {
        if (droppedCard == null)
        {
            Debug.LogWarning("드롭된 카드가 null입니다.");
            return;
        }

        CardData card = droppedCard.GetCardData();
        if (card == null)
        {
            Debug.LogWarning("드롭된 카드에 유효한 데이터가 없습니다.");
            return;
        }

        // 카드 효과 실행 방식 변경
        if (card.cardEffect != null)
        {
            card.cardEffect.ExecuteEffect(CombatContext.Instance, card, null);
        }
        else
        {
            Debug.LogWarning($"{card.cardName} 카드에 효과가 설정되지 않았습니다.");
        }
    }
}