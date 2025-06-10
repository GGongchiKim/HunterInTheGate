using UnityEngine;
using TMPro;

public class SpeechBubbleUI : MonoBehaviour
{
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI bodyText;

    private RectTransform rectTransform;

    private void Awake()
    {
        // 안전하게 RectTransform 캐싱
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 말풍선 초기화 (로컬 좌표 기준 위치 적용)
    /// </summary>
    public void Setup(string speaker, string body, Vector3 localPosition)
    {
        speakerText.text = speaker;
        bodyText.text = body;

        // Anchor와 Pivot을 중앙 기준으로 강제 설정
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.0f); // 하단 꼬리 기준, 아니면 (0.5, 0.5)

        rectTransform.anchoredPosition = localPosition;
    }
}