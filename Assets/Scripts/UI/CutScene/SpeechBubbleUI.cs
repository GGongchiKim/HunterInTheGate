using UnityEngine;
using TMPro;

public class SpeechBubbleUI : MonoBehaviour
{
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI bodyText;

    private RectTransform rectTransform;

    private void Awake()
    {
        // �����ϰ� RectTransform ĳ��
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// ��ǳ�� �ʱ�ȭ (���� ��ǥ ���� ��ġ ����)
    /// </summary>
    public void Setup(string speaker, string body, Vector3 localPosition)
    {
        speakerText.text = speaker;
        bodyText.text = body;

        // Anchor�� Pivot�� �߾� �������� ���� ����
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.0f); // �ϴ� ���� ����, �ƴϸ� (0.5, 0.5)

        rectTransform.anchoredPosition = localPosition;
    }
}