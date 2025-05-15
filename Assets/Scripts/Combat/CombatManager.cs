using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public void ProcessCardEffect(CardUI droppedCard)
    {
        if (droppedCard == null)
        {
            Debug.LogWarning("��ӵ� ī�尡 null�Դϴ�.");
            return;
        }

        CardData card = droppedCard.GetCardData();
        if (card == null)
        {
            Debug.LogWarning("��ӵ� ī�忡 ��ȿ�� �����Ͱ� �����ϴ�.");
            return;
        }

        // ī�� ȿ�� ���� ��� ����
        if (card.cardEffect != null)
        {
            card.cardEffect.ExecuteEffect(CombatContext.Instance, card, null);
        }
        else
        {
            Debug.LogWarning($"{card.cardName} ī�忡 ȿ���� �������� �ʾҽ��ϴ�.");
        }
    }
}