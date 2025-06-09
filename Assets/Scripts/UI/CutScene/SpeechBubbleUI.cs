using UnityEngine;
using TMPro;

public class SpeechBubbleUI : MonoBehaviour
{
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI bodyText;
    public RectTransform rectTransform;

    public void Setup(string speaker, string body, Vector3 screenPos)
    {
        speakerText.text = speaker;
        bodyText.text = body;

        rectTransform.position = screenPos;
    }
}