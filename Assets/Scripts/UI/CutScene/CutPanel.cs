using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CutsceneSystem
{
    public class CutPanel : MonoBehaviour
    {
        [Header("UI 참조")]
        public RectTransform maskRect;
        public Image cutImage;
        public Transform bubbleAnchor;
        public GameObject fxLayer;

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        public void Setup(CutPanelData data)
        {
            cutImage.sprite = data.cutImage;
            rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = data.anchoredPosition;
            rectTransform.sizeDelta = data.sizeDelta;
            rectTransform.SetSiblingIndex(data.sortingOrder);
        }

        public IEnumerator PlayTransition(PanelTransitionType type, float duration)
        {
            var effect = GetComponent<TransitionEffect>();
            if (effect != null)
                yield return effect.Play(type, duration); //캐스팅 제거
        }

        public IEnumerator PlaySpeechSequence(SpeechData[] speeches)
        {
            foreach (var speech in speeches)
            {
                yield return new WaitForSeconds(speech.delayBefore);

                if (speech.isNarration)
                {
                    // 중앙 나레이션 박스 출력
                    SpeechBubbleManager.Instance.ShowNarration(speech.text);
                }
                else
                {
                    // 말풍선 출력
                    Transform parent = bubbleAnchor != null ? bubbleAnchor : cutImage.transform;
                    Vector3 localPosition = speech.offset;
                    SpeechBubbleManager.Instance.ShowSpeechBubble(speech.speakerName, speech.text, parent, localPosition);
                }

                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                SpeechBubbleManager.Instance.Hide();
            }
        }
    }
}