using UnityEngine;
using TMPro;

public class NarrationBoxUI : MonoBehaviour
{
    public TextMeshProUGUI bodyText;
    public RectTransform rectTransform;

    public void Setup(string body)
    {
        bodyText.text = body;

        // ¡ﬂæ” ∞Ì¡§
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
    }
}