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
                yield return effect.Play((TransitionType)type, duration);
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
                    // 말풍선 출력 (bubbleAnchor의 이미지 하위에 로컬 좌표로 생성)
                    Transform parent = bubbleAnchor != null ? bubbleAnchor : cutImage.transform;
                    Vector3 localPosition = speech.offset; // 로컬 기준으로 오프셋 적용
                    SpeechBubbleManager.Instance.ShowSpeechBubble(speech.speakerName, speech.text, parent, localPosition);
                }

                // 사용자 입력 대기
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

                // 다음 대사를 위해 말풍선 또는 나레이션 제거
                SpeechBubbleManager.Instance.Hide();
            }
        }

    }
}